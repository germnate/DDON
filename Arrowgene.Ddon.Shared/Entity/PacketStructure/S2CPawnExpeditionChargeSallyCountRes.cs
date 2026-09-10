using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnExpeditionChargeSallyCountRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CHARGE_SALLY_COUNT_RES;

        public class Serializer : PacketEntitySerializer<S2CPawnExpeditionChargeSallyCountRes>
        {
            public override void Write(IBuffer buffer, S2CPawnExpeditionChargeSallyCountRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnExpeditionChargeSallyCountRes Read(IBuffer buffer)
            {
                S2CPawnExpeditionChargeSallyCountRes obj = new S2CPawnExpeditionChargeSallyCountRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
