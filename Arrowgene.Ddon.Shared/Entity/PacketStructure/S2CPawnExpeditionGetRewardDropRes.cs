using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnExpeditionGetRewardDropRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_RES;

        public S2CPawnExpeditionGetRewardDropRes()
        {
            BoxIdVec = new();
            MdlTypeVec = new();
        }

        public uint RewardDropNum { get; set; }
        public List<CDataCommonU32> BoxIdVec { get; set; }
        public List<CDataCommonU8> MdlTypeVec { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnExpeditionGetRewardDropRes>
        {
            public override void Write(IBuffer buffer, S2CPawnExpeditionGetRewardDropRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.RewardDropNum);
                WriteEntityList(buffer, obj.BoxIdVec);
                WriteEntityList(buffer, obj.MdlTypeVec);
            }

            public override S2CPawnExpeditionGetRewardDropRes Read(IBuffer buffer)
            {
                S2CPawnExpeditionGetRewardDropRes obj = new S2CPawnExpeditionGetRewardDropRes();
                ReadServerResponse(buffer, obj);
                obj.RewardDropNum = ReadUInt32(buffer);
                obj.BoxIdVec = ReadEntityList<CDataCommonU32>(buffer);
                obj.MdlTypeVec = ReadEntityList<CDataCommonU8>(buffer);
                return obj;
            }
        }
    }
}
