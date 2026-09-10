using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnExpeditionGetRewardDropItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_REQ;

        public C2SPawnExpeditionGetRewardDropItemReq()
        {
            GatheringItemGetRequestList = new();
        }

        public uint PawnRewardBoxId { get; set; }
        public List<CDataGatheringItemGetRequest> GatheringItemGetRequestList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnExpeditionGetRewardDropItemReq>
        {
            public override void Write(IBuffer buffer, C2SPawnExpeditionGetRewardDropItemReq obj)
            {
                WriteUInt32(buffer, obj.PawnRewardBoxId);
                WriteEntityList(buffer, obj.GatheringItemGetRequestList);
            }

            public override C2SPawnExpeditionGetRewardDropItemReq Read(IBuffer buffer)
            {
                C2SPawnExpeditionGetRewardDropItemReq obj = new C2SPawnExpeditionGetRewardDropItemReq();
                obj.PawnRewardBoxId = ReadUInt32(buffer);
                obj.GatheringItemGetRequestList = ReadEntityList<CDataGatheringItemGetRequest>(buffer);
                return obj;
            }
        }
    }
}
