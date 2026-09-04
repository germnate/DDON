using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ContextSetContextHandler : StructurePacketHandler<GameClient, C2SContextSetContextNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ContextSetContextHandler));


        public ContextSetContextHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SContextSetContextNtc> packet)
        {
            // Should this be stored and later be sent in the GetSetContextHandler?
            // Or should it be sent immediately?
            // To the client or to all party?
            Tuple<CDataContextSetBase, CDataContextSetAdditional> context = new Tuple<CDataContextSetBase, CDataContextSetAdditional>(packet.Structure.Base, packet.Structure.Additional);

            int index = context.Item2.MasterIndex;

            if (index == -1)
            {
                index = client.Party.ClientIndex(client);
            }

            Tuple<CDataContextSetBase, CDataContextSetAdditional> previousContext = ContextManager.GetContext(client.Party, context.Item1.UniqueId);

            ContextManager.SetContext(client.Party, context.Item1.UniqueId, context);
            ContextManager.AssignMaster(client, packet.Structure.Base.UniqueId, index);

            Logger.Debug(
                $"C2SSetContextNtc: CharacterId={client.Character.CharacterId}, ContextId={context.Item1.ContextId}, UniqueId=0x{context.Item1.UniqueId:x16}, " +
                $"MasterIndex={index}, StageNo={context.Item1.StageNo}, EncountArea={context.Item1.EncountArea}, " +
                $"ActNo={context.Item2.ActNo}, StateLive={context.Item2.StateLive}, CatchType={context.Item2.CatchType}, CatchJointNo={context.Item2.CatchJointNo}, CatchTargetUID=0x{context.Item2.CatchTargetUID:x16}, " +
                $"PreviousActNo={previousContext?.Item2.ActNo.ToString() ?? "null"}, PreviousStateLive={previousContext?.Item2.StateLive.ToString() ?? "null"}, " +
                $"PreviousCatchType={previousContext?.Item2.CatchType.ToString() ?? "null"}, PreviousCatchJointNo={previousContext?.Item2.CatchJointNo.ToString() ?? "null"}, PreviousCatchTargetUID={(previousContext == null ? "null" : $"0x{previousContext.Item2.CatchTargetUID:x16}")}");
        }
    }
}
