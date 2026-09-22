using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Npgsql;
using System;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Arrowgene.Ddon.Server.Network
{
    public abstract class RequestPacketHandler<TClient, TReqStruct, TResStruct> : StructurePacketHandler<TClient, TReqStruct>
        where TClient : Client
        where TReqStruct : class, IPacketStructure, new()
        where TResStruct : ServerResponse, new()
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RequestPacketHandler<TClient, TReqStruct, TResStruct>));

        protected RequestPacketHandler(DdonServer<TClient> server) : base(server)
        {
#if DEBUG
            if (!EntitySerializer.Contains(typeof(TResStruct)))
            {
                Logger.Error($"RequestPacketHandler missing serializer for {typeof(TResStruct).Name}");
            }
#endif
        }

        public abstract TResStruct Handle(TClient client, TReqStruct request);

        public sealed override void Handle(TClient client, StructurePacket<TReqStruct> request)
        {
            try
            {
                TResStruct response = Handle(client, request.Structure);
                if (response == null)
                {
                    Logger.Error(client, $"{GetType().Name} returned a null response for {typeof(TReqStruct).Name}");
                    response = new TResStruct
                    {
                        Error = (uint)ErrorCode.ERROR_CODE_FAIL
                    };
                }

                client.Send(response);
            }
            catch (SQLiteException ex)
            {
                TResStruct response;
                if (ex.ErrorCode == (int)SQLiteErrorCode.Busy)
                {
                    response = new TResStruct
                    {
                        Error = (uint)ErrorCode.ERROR_CODE_DB_DEAD_LOCK
                    };
                }
                else
                {
                    response = new TResStruct
                    {
                        Error = (uint)ErrorCode.ERROR_CODE_DB_FAILURE
                    };
                }
                client.Send(response);
                client.Close(); // Do not tolerate SqLiteExceptions because of desync issues.
                throw;
            }
            catch (PostgresException ex)
            {
                TResStruct response = new TResStruct
                {
                    Error = (uint)ErrorCode.ERROR_CODE_DB_FAILURE
                };
                client.Send(response);
                client.Close();
                throw;
            }
            catch (NotImplementedException ex)
            {
                SendResponseError(client, new ResponseErrorException(ErrorCode.ERROR_CODE_NOT_IMPLEMENTED, ex.Message, ex));
            }
            catch (ResponseErrorException ex)
            {
                SendResponseError(client, ex);
            }
            catch (Exception)
            {
                TResStruct response = new TResStruct();
                response.Error = (uint) ErrorCode.ERROR_CODE_FAIL;
                client.Send(response);
                throw;
            }
        }

        private void SendResponseError(TClient client, ResponseErrorException ex)
        {
            TResStruct response = new TResStruct();
            response.Error = (uint)ex.ErrorCode;

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{(ex.Critical ? "!!CRITICAL!! " : "")}{ex.ErrorCode} thrown when handling {typeof(TReqStruct).Name}");
            if (ex.Message.Length > 0)
            {
                stringBuilder.AppendLine($"\tMessage: {ex.Message}");
            }
            stringBuilder.AppendLine(ex.StackTrace?.Split(Environment.NewLine).FirstOrDefault());
            Logger.Error(client, stringBuilder.ToString());

            client.Send(response);

            if (ex.Critical)
            {
                client.Close();
            }
        }

    }
}
