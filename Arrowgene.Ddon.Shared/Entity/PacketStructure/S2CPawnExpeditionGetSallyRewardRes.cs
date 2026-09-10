using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnExpeditionGetSallyRewardRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_REWARD_RES;

        public S2CPawnExpeditionGetSallyRewardRes()
        {
            SallySpotInfo = new();
            BattleResult = new();
        }

        public CDataAreaSpotSet SallySpotInfo { get; set; }
        public byte SearchSpotType { get; set; }
        public CDataBattleResultInfo BattleResult { get; set; }
        public bool IsGoldenSally { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnExpeditionGetSallyRewardRes>
        {
            public override void Write(IBuffer buffer, S2CPawnExpeditionGetSallyRewardRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.SallySpotInfo);
                WriteByte(buffer, obj.SearchSpotType);
                WriteEntity(buffer, obj.BattleResult);
                WriteBool(buffer, obj.IsGoldenSally);
            }

            public override S2CPawnExpeditionGetSallyRewardRes Read(IBuffer buffer)
            {
                S2CPawnExpeditionGetSallyRewardRes obj = new S2CPawnExpeditionGetSallyRewardRes();
                ReadServerResponse(buffer, obj);
                obj.SallySpotInfo = ReadEntity<CDataAreaSpotSet>(buffer);
                obj.SearchSpotType = ReadByte(buffer);
                obj.BattleResult = ReadEntity<CDataBattleResultInfo>(buffer);
                obj.IsGoldenSally = ReadBool(buffer);
                return obj;
            }
        }
    }
}
