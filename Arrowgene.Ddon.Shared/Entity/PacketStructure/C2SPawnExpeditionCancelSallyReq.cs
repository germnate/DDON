using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnExpeditionCancelSallyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CANCEL_SALLY_REQ;

        public class Serializer : PacketEntitySerializer<C2SPawnExpeditionCancelSallyReq>
        {
            public override void Write(IBuffer buffer, C2SPawnExpeditionCancelSallyReq obj)
            {
            }

            public override C2SPawnExpeditionCancelSallyReq Read(IBuffer buffer)
            {
                C2SPawnExpeditionCancelSallyReq obj = new C2SPawnExpeditionCancelSallyReq();
                return obj;
            }
        }
    }
}
