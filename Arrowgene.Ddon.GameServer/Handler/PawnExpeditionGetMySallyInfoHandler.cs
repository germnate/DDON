using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionGetMySallyInfoHandler : GameRequestPacketHandler<C2SPawnExpeditionGetMySallyInfoReq, S2CPawnExpeditionGetMySallyInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionGetMySallyInfoHandler));

        public PawnExpeditionGetMySallyInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnExpeditionGetMySallyInfoRes Handle(GameClient client, C2SPawnExpeditionGetMySallyInfoReq request)
        {
            PawnExpeditionRecord record = Server.PawnExpeditionManager.GetOrCreateRecord(client);

            return new S2CPawnExpeditionGetMySallyInfoRes()
            {
                SallySpotInfo = new()
                {
                    AreaId = (QuestAreaId)record.AreaId,
                    SpotId = record.SpotId
                },
                IsHotSpot = record.IsHotSpot,
                SallyType = (byte)(record.IsGoldenSally ? 1 : 0),
                SallyCount = record.SallyCount
            };
        }
    }
}
