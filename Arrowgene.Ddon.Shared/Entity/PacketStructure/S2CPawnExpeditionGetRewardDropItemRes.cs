using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnExpeditionGetRewardDropItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_RES;

        public S2CPawnExpeditionGetRewardDropItemRes()
        {
            GatheringItemListAfterRequest = new();
        }

        public uint PawnRewardBoxId { get; set; }
        public List<CDataGatheringItemGetRequest> GatheringItemListAfterRequest { get; set; }
        public PawnExpeditionStatus PawnExpeditionStatus { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnExpeditionGetRewardDropItemRes>
        {
            public override void Write(IBuffer buffer, S2CPawnExpeditionGetRewardDropItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnRewardBoxId);
                WriteEntityList(buffer, obj.GatheringItemListAfterRequest);
                WriteByte(buffer, (byte)obj.PawnExpeditionStatus);
            }

            public override S2CPawnExpeditionGetRewardDropItemRes Read(IBuffer buffer)
            {
                S2CPawnExpeditionGetRewardDropItemRes obj = new S2CPawnExpeditionGetRewardDropItemRes();
                ReadServerResponse(buffer, obj);
                obj.PawnRewardBoxId = ReadUInt32(buffer);
                obj.GatheringItemListAfterRequest = ReadEntityList<CDataGatheringItemGetRequest>(buffer);
                obj.PawnExpeditionStatus = (PawnExpeditionStatus)ReadByte(buffer);
                return obj;
            }
        }
    }
}
