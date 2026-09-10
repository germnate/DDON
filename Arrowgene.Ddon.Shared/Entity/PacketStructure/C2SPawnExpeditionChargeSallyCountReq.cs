using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnExpeditionChargeSallyCountReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CHARGE_SALLY_COUNT_REQ;

        public byte Price { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnExpeditionChargeSallyCountReq>
        {
            public override void Write(IBuffer buffer, C2SPawnExpeditionChargeSallyCountReq obj)
            {
                WriteByte(buffer, obj.Price);
            }

            public override C2SPawnExpeditionChargeSallyCountReq Read(IBuffer buffer)
            {
                C2SPawnExpeditionChargeSallyCountReq obj = new C2SPawnExpeditionChargeSallyCountReq();
                obj.Price = ReadByte(buffer);
                return obj;
            }
        }
    }
}
