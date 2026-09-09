using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.GameServer.Enemies.Generators;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : GameRequestPacketHandler<C2SInstanceGetEnemySetListReq, S2CInstanceGetEnemySetListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));

        // Order in list indicates priority where first item has highest priority and last item has least.
        private readonly List<IEnemySetGenerator> EnemySetGenerators = new List<IEnemySetGenerator>()
        {
            new SpawnTestingGenerator(),
            new QuestEnemySetGenerator(),
            new EpitaphRoadEnemySetGenerator(),
            new CautionSpotEnemyGenerator(),
            new WorldEnemySetGenerator(),
        };

        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceGetEnemySetListRes Handle(GameClient client, C2SInstanceGetEnemySetListReq request)
        {
            StageLayoutId stageLayoutId = request.LayoutId.AsStageLayoutId();
            byte subGroupId = request.SubGroupId;
            client.Character.Stage = stageLayoutId;

            Logger.Info($"StageId={stageLayoutId}, SubGroupId={request.SubGroupId}");

            S2CInstanceGetEnemySetListRes response = new S2CInstanceGetEnemySetListRes()
            {
                LayoutId = stageLayoutId.ToCDataStageLayoutId(),
                SubGroupId = subGroupId,
                RandomSeed = CryptoRandom.Instance.GetRandomUInt32(),
                QuestId = QuestId.None,
            };

            var instancedEnemyList = new List<InstancedEnemy>();
            foreach (var generator in EnemySetGenerators)
            {
                if (generator.GetEnemySet(Server, client, stageLayoutId, subGroupId, /* out */ instancedEnemyList, out QuestId questId))
                {
                    response.QuestId = questId;
                    break;
                }
            }

            for (var i = 0; i < instancedEnemyList.Count; i++)
            {
                var enemy = client.Party.InstanceEnemyManager.GetInstanceEnemy(stageLayoutId, instancedEnemyList[i].Index);

                if (enemy != null && (enemy.QuestScheduleId != instancedEnemyList[i].QuestScheduleId))
                {
                    var uid = ContextManager.CreateEnemyUID(enemy.Index, stageLayoutId.ToCDataStageLayoutId());
                    ContextManager.RemoveContext(client.Party, uid);
                }

                if (enemy == null || (enemy.QuestScheduleId != instancedEnemyList[i].QuestScheduleId))
                {
                    enemy = instancedEnemyList[i].CreateNewInstance();
                    if (Server.GameSettings.GameServerSettings.EnableAutomaticExpCalculationForAll)
                    {
                        enemy.ExpScheme = EnemyExpScheme.Automatic;
                    }

                    foreach (var generator in Server.ScriptManager.InstanceEnemyPropertyGeneratorModule.GetGenerators())
                    {
                        generator.ApplyChanges(client, stageLayoutId, subGroupId, enemy);
                    }
                    client.Party.InstanceEnemyManager.SetInstanceEnemy(stageLayoutId, enemy.Index, enemy);
                }
                enemy.StageLayoutId = stageLayoutId;

                response.EnemyList.Add(new CDataLayoutEnemyData()
                {
                    PositionIndex = enemy.Index,
                    EnemyInfo = enemy.AsCDataStageLayoutEnemyPresetEnemyInfoClient()
                });
            }

            LogEnemyRoster(client, stageLayoutId, subGroupId);

            if (subGroupId > 0 && response.EnemyList.Count > 0)
            {
                S2CInstanceEnemySubGroupAppearNtc subgroupNtc = new S2CInstanceEnemySubGroupAppearNtc()
                {
                    SubGroupId = subGroupId,
                    LayoutId = stageLayoutId.ToCDataStageLayoutId(),
                };

                client.Party.SendToAll(subgroupNtc);
            }

            return response;
        }

        private void LogEnemyRoster(GameClient client, StageLayoutId stageLayoutId, byte subGroupId)
        {
            List<InstancedEnemy> group = client.Party.InstanceEnemyManager.GetInstancedEnemies(stageLayoutId);
            if (group.Count == 0)
            {
                Logger.Info(client, $"[EnemyRoster] Stage={stageLayoutId} (StageId={stageLayoutId.Id} LayerNo={stageLayoutId.LayerNo} GroupId={stageLayoutId.GroupId}) SubGroupId={subGroupId} -> no enemies");
                return;
            }

            int requiredCount = group.Count(x => x.IsRequired);
            Logger.Info(client, $"[EnemyRoster] Stage={stageLayoutId} (StageId={stageLayoutId.Id} LayerNo={stageLayoutId.LayerNo} GroupId={stageLayoutId.GroupId}) SubGroupId={subGroupId} Count={group.Count} Required={requiredCount}");
            foreach (var enemy in group.OrderBy(x => x.Index))
            {
                Logger.Info(client, $"[EnemyRoster]   Index/SetId={enemy.Index} Subgroup={enemy.Subgroup} EnemyId={enemy.EnemyId}(0x{(uint)enemy.EnemyId:X}) Lv={enemy.Lv} Named={enemy.NamedEnemyParams?.Id.ToString() ?? "-"} IsRequired={enemy.IsRequired} IsKilled={enemy.IsKilled} IsAreaBoss={enemy.IsAreaBoss} IsBossGauge={enemy.IsBossGauge} RepopCount={enemy.RepopCount} QuestScheduleId={enemy.QuestScheduleId}");
            }
        }
    }
}
