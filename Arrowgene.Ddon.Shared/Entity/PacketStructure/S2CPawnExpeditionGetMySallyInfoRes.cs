using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnExpeditionGetMySallyInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_MY_SALLY_INFO_RES;

        public S2CPawnExpeditionGetMySallyInfoRes()
        {
            SallySpotInfo = new();
        }

        public CDataAreaSpotSet SallySpotInfo { get; set; }
        public bool IsHotSpot { get; set; }
        public byte SallyType { get; set; }
        public byte SallyCount { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnExpeditionGetMySallyInfoRes>
        {
            public override void Write(IBuffer buffer, S2CPawnExpeditionGetMySallyInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.SallySpotInfo);
                WriteBool(buffer, obj.IsHotSpot);
                WriteByte(buffer, obj.SallyType);
                WriteByte(buffer, obj.SallyCount);
            }

            public override S2CPawnExpeditionGetMySallyInfoRes Read(IBuffer buffer)
            {
                S2CPawnExpeditionGetMySallyInfoRes obj = new S2CPawnExpeditionGetMySallyInfoRes();
                ReadServerResponse(buffer, obj);
                obj.SallySpotInfo = ReadEntity<CDataAreaSpotSet>(buffer);
                obj.IsHotSpot = ReadBool(buffer);
                obj.SallyType = ReadByte(buffer);
                obj.SallyCount = ReadByte(buffer);
                return obj;
            }
        }
    }
}
