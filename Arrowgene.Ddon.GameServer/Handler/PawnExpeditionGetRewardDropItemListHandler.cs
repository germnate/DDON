using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionGetRewardDropItemListHandler : GameRequestPacketHandler<C2SPawnExpeditionGetRewardDropItemListReq, S2CPawnExpeditionGetRewardDropItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionGetRewardDropItemListHandler));

        public PawnExpeditionGetRewardDropItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnExpeditionGetRewardDropItemListRes Handle(GameClient client, C2SPawnExpeditionGetRewardDropItemListReq request)
        {
            PawnExpeditionRewardBox box = Server.Database.GetPawnExpeditionRewardBox(client.Character.CharacterId, request.PawnRewardBoxId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_REWARD_BOX_ID_NOT_EXIST, $"Reward box {request.PawnRewardBoxId} does not exist");

            return new S2CPawnExpeditionGetRewardDropItemListRes()
            {
                PawnRewardBoxId = box.BoxId,
                GatheringItemList = box.Items
                    .Where(x => !x.Claimed)
                    .Select(x => new CDataGatheringItemElement()
                    {
                        SlotNo = x.SlotNo,
                        ItemId = x.ItemId,
                        ItemNum = x.ItemNum,
                        Quality = x.Quality,
                        IsHidden = x.IsHidden
                    })
                    .ToList()
            };
        }
    }
}
