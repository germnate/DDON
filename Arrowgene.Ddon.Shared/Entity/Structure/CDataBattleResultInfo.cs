using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleResultInfo
    {
        public uint EnemyId { get; set; }
        public uint EnemyNum { get; set; }
        public uint EnemyLevel { get; set; }

        public class Serializer : EntitySerializer<CDataBattleResultInfo>
        {
            public override void Write(IBuffer buffer, CDataBattleResultInfo obj)
            {
                WriteUInt32(buffer, obj.EnemyId);
                WriteUInt32(buffer, obj.EnemyNum);
                WriteUInt32(buffer, obj.EnemyLevel);
            }

            public override CDataBattleResultInfo Read(IBuffer buffer)
            {
                CDataBattleResultInfo obj = new CDataBattleResultInfo();
                obj.EnemyId = ReadUInt32(buffer);
                obj.EnemyNum = ReadUInt32(buffer);
                obj.EnemyLevel = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
