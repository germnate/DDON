using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnExpeditionChangeGoldenSallyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CHANGE_GOLDEN_SALLY_REQ;

        public byte Price { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnExpeditionChangeGoldenSallyReq>
        {
            public override void Write(IBuffer buffer, C2SPawnExpeditionChangeGoldenSallyReq obj)
            {
                WriteByte(buffer, obj.Price);
            }

            public override C2SPawnExpeditionChangeGoldenSallyReq Read(IBuffer buffer)
            {
                C2SPawnExpeditionChangeGoldenSallyReq obj = new C2SPawnExpeditionChangeGoldenSallyReq();
                obj.Price = ReadByte(buffer);
                return obj;
            }
        }
    }
}
