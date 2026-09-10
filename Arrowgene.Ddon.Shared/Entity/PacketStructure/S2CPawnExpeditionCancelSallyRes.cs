using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnExpeditionCancelSallyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CANCEL_SALLY_RES;

        public class Serializer : PacketEntitySerializer<S2CPawnExpeditionCancelSallyRes>
        {
            public override void Write(IBuffer buffer, S2CPawnExpeditionCancelSallyRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnExpeditionCancelSallyRes Read(IBuffer buffer)
            {
                S2CPawnExpeditionCancelSallyRes obj = new S2CPawnExpeditionCancelSallyRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
