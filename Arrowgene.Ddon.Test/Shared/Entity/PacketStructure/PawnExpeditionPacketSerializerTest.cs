using System;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Xunit;

namespace Arrowgene.Ddon.Test.Shared.Entity.PacketStructure
{
    public class PawnExpeditionPacketSerializerTest
    {
        [Fact]
        public void TestAllPacketStructuresHaveSerializers()
        {
            Type[] packetTypes = typeof(C2SPawnExpeditionSallyReq).Assembly
                .GetTypes()
                .Where(type =>
                    type.Namespace == typeof(C2SPawnExpeditionSallyReq).Namespace
                    && type.IsClass
                    && !type.IsAbstract
                    && typeof(IPacketStructure).IsAssignableFrom(type))
                .ToArray();

            Type[] missing = packetTypes
                .Where(type => !EntitySerializer.Contains(type))
                .OrderBy(type => type.Name)
                .ToArray();

            Assert.True(missing.Length == 0, $"Missing packet serializers: {string.Join(", ", missing.Select(type => type.Name))}");
        }

        [Fact]
        public void TestPawnExpeditionSallyRequestCarriesPawnId()
        {
            var request = new C2SPawnExpeditionSallyReq
            {
                PawnId = 123,
                AreaId = 456,
                SpotId = 789
            };

            Assert.Equal(123u, request.PawnId);
            Assert.Equal(456u, request.AreaId);
            Assert.Equal(789u, request.SpotId);
        }

        [Fact]
        public void TestPawnExpeditionRewardResponseSerializes()
        {
            var response = new S2CPawnExpeditionGetSallyRewardRes
            {
                SallySpotInfo = new CDataAreaSpotSet
                {
                    AreaId = QuestAreaId.HidellPlains,
                    SpotId = 1
                },
                SearchSpotType = 1,
                BattleResult = new CDataBattleResultInfo
                {
                    EnemyId = 1,
                    EnemyNum = 3,
                    EnemyLevel = 10
                },
                IsGoldenSally = false
            };

            byte[] serialized = EntitySerializer.Get<S2CPawnExpeditionGetSallyRewardRes>().Write(response);

            Assert.NotNull(serialized);
            Assert.NotEmpty(serialized);
        }

        [Fact]
        public void TestPawnExpeditionRewardResponseInitializesNestedObjects()
        {
            var response = new S2CPawnExpeditionGetSallyRewardRes();

            Assert.NotNull(response.SallySpotInfo);
            Assert.NotNull(response.BattleResult);
        }
    }
}
