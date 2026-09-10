using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnExpeditionGetRewardDropItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_LIST_REQ;

        public uint PawnRewardBoxId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnExpeditionGetRewardDropItemListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnExpeditionGetRewardDropItemListReq obj)
            {
                WriteUInt32(buffer, obj.PawnRewardBoxId);
            }

            public override C2SPawnExpeditionGetRewardDropItemListReq Read(IBuffer buffer)
            {
                C2SPawnExpeditionGetRewardDropItemListReq obj = new C2SPawnExpeditionGetRewardDropItemListReq();
                obj.PawnRewardBoxId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
