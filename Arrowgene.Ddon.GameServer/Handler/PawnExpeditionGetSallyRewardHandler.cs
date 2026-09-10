using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionGetSallyRewardHandler : GameRequestPacketHandler<C2SPawnExpeditionGetSallyRewardReq, S2CPawnExpeditionGetSallyRewardRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionGetSallyRewardHandler));

        public PawnExpeditionGetSallyRewardHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnExpeditionGetSallyRewardRes Handle(GameClient client, C2SPawnExpeditionGetSallyRewardReq request)
        {
            PawnExpeditionRecord record = Server.PawnExpeditionManager.GetOrCreateRecord(client);
            if (record.Status != PawnExpeditionStatus.Returned)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_NOT_SALLY, "No completed sally to retrieve a reward summary for");
            }

            S2CPawnExpeditionGetSallyRewardRes res = new S2CPawnExpeditionGetSallyRewardRes()
            {
                SallySpotInfo = new()
                {
                    AreaId = (QuestAreaId)record.AreaId,
                    SpotId = record.SpotId
                },
                SearchSpotType = (byte)(record.IsHotSpot ? 1 : 0),
                BattleResult = Server.PawnExpeditionManager.GetLastBattleResult(record),
                IsGoldenSally = record.IsGoldenSally
            };

            Server.PawnExpeditionManager.FinishViewingReward(client);

            return res;
        }
    }
}
