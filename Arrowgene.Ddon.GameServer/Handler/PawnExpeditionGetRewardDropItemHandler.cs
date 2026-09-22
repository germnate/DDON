using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionGetRewardDropItemHandler : GameRequestPacketQueueHandler<C2SPawnExpeditionGetRewardDropItemReq, S2CPawnExpeditionGetRewardDropItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnExpeditionGetRewardDropItemHandler));

        public PawnExpeditionGetRewardDropItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SPawnExpeditionGetRewardDropItemReq request)
        {
            PacketQueue packetQueue = new();

            PawnExpeditionRewardBox box = Server.Database.GetPawnExpeditionRewardBox(client.Character.CharacterId, request.PawnRewardBoxId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_REWARD_BOX_ID_NOT_EXIST, $"Reward box {request.PawnRewardBoxId} does not exist");

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.PawnExpeditionDrop
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataGatheringItemGetRequest itemRequest in request.GatheringItemGetRequestList)
                {
                    PawnExpeditionRewardBoxItem boxItem = box.Items.FirstOrDefault(x => x.SlotNo == itemRequest.SlotNo)
                        ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_INVALID_ITEM_POS_ID, $"Invalid reward box item slot {itemRequest.SlotNo}");

                    if (boxItem.Claimed)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_ALREADY_RECIEVE, $"Reward box item slot {itemRequest.SlotNo} was already claimed");
                    }

                    if (itemRequest.Num == 0 || itemRequest.Num > boxItem.ItemNum)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_EXPEDITION_ITEM_REQUEST_NUM_OVER, $"Invalid claim amount for reward box item slot {itemRequest.SlotNo}");
                    }

                    ntc.UpdateItemList.AddRange(Server.ItemManager.AddItem(Server, client.Character, true, boxItem.ItemId, itemRequest.Num, connectionIn: connection));

                    Server.Database.ClaimPawnExpeditionRewardBoxItem(box.BoxId, boxItem.SlotNo, connection);
                    boxItem.Claimed = true;
                }

                if (box.Items.All(x => x.Claimed))
                {
                    Server.Database.SetPawnExpeditionRewardBoxClaimed(box.BoxId, connection);
                }
            });

            client.Enqueue(ntc, packetQueue);

            PawnExpeditionRecord record = Server.PawnExpeditionManager.GetOrCreateRecord(client);

            S2CPawnExpeditionGetRewardDropItemRes res = new S2CPawnExpeditionGetRewardDropItemRes()
            {
                PawnRewardBoxId = box.BoxId,
                GatheringItemListAfterRequest = box.Items
                    .Where(x => !x.Claimed)
                    .Select(x => new CDataGatheringItemGetRequest() { SlotNo = x.SlotNo, Num = x.ItemNum })
                    .ToList(),
                PawnExpeditionStatus = record.Status
            };
            client.Enqueue(res, packetQueue);

            return packetQueue;
        }
    }
}
