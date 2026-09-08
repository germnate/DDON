using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    /// <summary>
    /// Rolls level-appropriate loot for treasure chests inside Extreme Mission and campaign
    /// (World/Board/Clan) quest instances. Chests registered in QuestBossChests.json
    /// (typically the final one or two chests in an Extreme Mission's boss room) roll from a
    /// higher, rarity-biased level window instead of the quest's normal window.
    ///
    /// Each chest rolls a single reward. Extreme Mission chests first get a chance
    /// (<see cref="QuestLootRange.RareMaterialChanceNormal"/> /
    /// <see cref="QuestLootRange.RareMaterialChanceBoss"/>) to roll one of the curated,
    /// signature EXM-only materials in QuestRareMaterials.json (e.g. Rare Netherworld
    /// Crystal) - boss-room chests get a substantially higher chance at these than regular
    /// chests, matching how these materials are the headline reward for clearing an EXM.
    /// Otherwise (or for non-EXM quests), the chest falls back to the normal
    /// Equipment/Material split: Equipment (gear) is comparatively rare
    /// (<see cref="EquipmentDropChance"/>) and Materials/Consumables are the common case -
    /// this keeps gald/crafting materials relevant instead of every chest handing out gear.
    /// Materials/Consumables are drawn from the same hand-curated, level-tagged pool used by
    /// the overworld's DefaultGatheringDropsAsset (DefaultGatheringDrops.json), flattened
    /// across all areas since quest instances have no QuestAreaId of their own to key off of.
    /// See docs/quests/loot_generator_plan_exm_and_campaign.md.
    /// </summary>
    public class QuestInstanceGatheringItemGenerator : IGatheringGenerator
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(QuestInstanceGatheringItemGenerator));

        // Gear should be the minority of chest rewards so gald/materials remain relevant.
        private const double EquipmentDropChance = 0.30;

        // Categories that represent gear rather than materials/consumables - these are rolled
        // via the ClientItemInfo-based Equipment path instead of the DefaultGatheringDrop pool.
        private static readonly HashSet<DropCategory> GearDropCategories = new()
        {
            DropCategory.Equipment,
            DropCategory.Jewelry
        };

        private readonly DdonGameServer Server;
        private readonly Lazy<List<DefaultGatheringDrop>> MaterialPool;

        private static readonly HashSet<QuestType> RelevantQuestTypes = new()
        {
            QuestType.World,
            QuestType.ExtremeMission,
            QuestType.Light // Also covers Board/Clan quests, which are authored with type "Light"
        };

        public QuestInstanceGatheringItemGenerator(DdonGameServer server)
        {
            Server = server;
            MaterialPool = new Lazy<List<DefaultGatheringDrop>>(BuildMaterialPool);
        }

        public override bool IsEnabled()
        {
            return Server.GameSettings.GameServerSettings.EnableQuestInstanceChestDrops;
        }

        public override List<InstancedGatheringItem> Generate(GameClient client, StageLayoutId stageId, uint index)
        {
            // BBM and Epitaph Road have their own dedicated chest-loot generators.
            if (StageManager.IsBitterBlackMazeStageId(stageId) || StageManager.IsEpitaphRoadStageId(stageId))
            {
                return new();
            }

            Quests.Quest quest = ResolveActiveQuest(client, stageId);
            if (quest is null)
            {
                return new();
            }

            QuestLootRange lootRange = Server.AssetRepository.QuestLootRangeAsset
                .FirstOrDefault(range => range.ContainsLevel(quest.BaseLevel));
            if (lootRange is null)
            {
                Logger.Debug($"No QuestLootRange bucket found for quest {quest.QuestId} (BaseLevel={quest.BaseLevel})");
                return new();
            }

            bool isBossChest = Server.AssetRepository.QuestBossChestAsset.Any(entry =>
                entry.QuestId == (uint) quest.QuestId &&
                entry.StageId == stageId.Id &&
                entry.GroupId == stageId.GroupId &&
                entry.PosId == index);

            if (QuestUtils.IsExmQuest(quest.QuestId))
            {
                double rareMaterialChance = isBossChest ? lootRange.RareMaterialChanceBoss : lootRange.RareMaterialChanceNormal;
                if (Random.Shared.NextDouble() < rareMaterialChance)
                {
                    List<InstancedGatheringItem> rareResult = RollRareMaterial(quest, stageId, index, lootRange, isBossChest);
                    if (rareResult.Count > 0)
                    {
                        return rareResult;
                    }
                }
            }

            byte minLevel = isBossChest ? lootRange.BossItemLevelMin : lootRange.NormalItemLevelMin;
            byte maxLevel = isBossChest ? lootRange.BossItemLevelMax : lootRange.NormalItemLevelMax;

            List<InstancedGatheringItem> result;
            if (Random.Shared.NextDouble() < EquipmentDropChance)
            {
                result = RollEquipment(quest, stageId, index, minLevel, maxLevel, isBossChest, lootRange);
                if (result.Count == 0)
                {
                    // Fall back to materials if no equipment fits this level window.
                    result = RollMaterial(quest, stageId, index, minLevel, maxLevel, isBossChest);
                }
            }
            else
            {
                result = RollMaterial(quest, stageId, index, minLevel, maxLevel, isBossChest);
                if (result.Count == 0)
                {
                    result = RollEquipment(quest, stageId, index, minLevel, maxLevel, isBossChest, lootRange);
                }
            }

            return result;
        }

        private List<InstancedGatheringItem> RollRareMaterial(Quests.Quest quest, StageLayoutId stageId, uint index,
            QuestLootRange lootRange, bool isBossChest)
        {
            List<ClientItemInfo> candidates = Server.AssetRepository.QuestRareMaterialAsset
                .Select(itemId => Server.AssetRepository.ClientItemInfos.GetValueOrDefault(itemId))
                .Where(item => item is not null
                    && item.Rank >= lootRange.RareMaterialRankMin && item.Rank <= lootRange.RareMaterialRankMax)
                .ToList();

            if (candidates.Count == 0)
            {
                Logger.Debug($"No rare material candidates found for quest {quest.QuestId} chest {stageId}.{index} " +
                    $"(Rank {lootRange.RareMaterialRankMin}-{lootRange.RareMaterialRankMax}, Boss={isBossChest})");
                return new();
            }

            ClientItemInfo selected = Random.Shared.Choose(candidates);

            Logger.Debug($"Rolled EXM rare material for quest {quest.QuestId} at {stageId}.{index} " +
                $"(Boss={isBossChest}): {selected.Name} <{selected.ItemId}> (Rank={selected.Rank})");

            return new List<InstancedGatheringItem>
            {
                new InstancedGatheringItem
                {
                    ItemId = selected.ItemId,
                    ItemNum = 1,
                    Quality = 1,
                    IsHidden = false
                }
            };
        }

        private List<InstancedGatheringItem> RollEquipment(Quests.Quest quest, StageLayoutId stageId, uint index,
            byte minLevel, byte maxLevel, bool isBossChest, QuestLootRange lootRange)
        {
            List<ClientItemInfo> candidates = Server.AssetRepository.ClientItemInfos.Values
                .Where(item => item.Category == 3 && item.Level.HasValue
                    && item.Level.Value >= minLevel && item.Level.Value <= maxLevel)
                .ToList();

            if (candidates.Count == 0)
            {
                Logger.Debug($"No equipment candidates found for quest {quest.QuestId} chest {stageId}.{index} " +
                    $"(ItemLevel {minLevel}-{maxLevel}, Boss={isBossChest})");
                return new();
            }

            ClientItemInfo selected;
            if (isBossChest)
            {
                // Bias is 0-1, lower favors higher (rarer) rolls, so subtract from 1 to
                // convert BossRareChanceBonus (higher = more bias toward rare) into the bias
                // parameter WeightedNext expects.
                double bias = Math.Clamp(1.0 - lootRange.BossRareChanceBonus, 0.05, 1.0);
                int rolledIndex = Random.Shared.WeightedNext(candidates.Count, bias);
                selected = candidates[rolledIndex];
            }
            else
            {
                selected = Random.Shared.Choose(candidates);
            }

            Logger.Debug($"Rolled quest chest equipment for quest {quest.QuestId} at {stageId}.{index} " +
                $"(Boss={isBossChest}): {selected.Name} <{selected.ItemId}> (ItemLevel={selected.Level})");

            return new List<InstancedGatheringItem>
            {
                new InstancedGatheringItem
                {
                    ItemId = selected.ItemId,
                    ItemNum = 1,
                    Quality = 1,
                    IsHidden = false
                }
            };
        }

        private List<InstancedGatheringItem> RollMaterial(Quests.Quest quest, StageLayoutId stageId, uint index,
            byte minLevel, byte maxLevel, bool isBossChest)
        {
            List<DefaultGatheringDrop> candidates = MaterialPool.Value
                .Where(drop => drop.ItemLevel >= minLevel && drop.ItemLevel <= maxLevel)
                .ToList();

            if (candidates.Count == 0)
            {
                Logger.Debug($"No material candidates found for quest {quest.QuestId} chest {stageId}.{index} " +
                    $"(ItemLevel {minLevel}-{maxLevel}, Boss={isBossChest})");
                return new();
            }

            // Boss chests still favor the higher end of the level window, same as equipment.
            var ordered = candidates.OrderBy(drop => drop.ItemLevel).ToList();
            DefaultGatheringDrop selected = isBossChest
                ? ordered[Random.Shared.WeightedNext(ordered.Count, 0.5)]
                : Random.Shared.Choose(ordered);

            uint itemNum;
            if (selected.MaxAmount == 0)
            {
                itemNum = selected.MinAmount;
            }
            else
            {
                itemNum = (uint) Random.Shared.Next((int) selected.MinAmount, (int) selected.MaxAmount + 1);
            }

            Logger.Debug($"Rolled quest chest material for quest {quest.QuestId} at {stageId}.{index} " +
                $"(Boss={isBossChest}): {selected.ItemId} x{itemNum} (ItemLevel={selected.ItemLevel}, Category={selected.DropCategory})");

            return new List<InstancedGatheringItem>
            {
                new InstancedGatheringItem
                {
                    ItemId = selected.ItemId,
                    ItemNum = itemNum,
                    Quality = selected.Quality,
                    IsHidden = false
                }
            };
        }

        private List<DefaultGatheringDrop> BuildMaterialPool()
        {
            return Server.AssetRepository.DefaultGatheringDropsAsset.AreaDefaultDrops.Values
                .SelectMany(categoryDrops => categoryDrops
                    .Where(kvp => !GearDropCategories.Contains(kvp.Key))
                    .SelectMany(kvp => kvp.Value))
                .GroupBy(drop => drop.ItemId)
                .Select(group => group.First())
                .ToList();
        }

        private static Quests.Quest ResolveActiveQuest(GameClient client, StageLayoutId stageId)
        {
            uint stageNo = StageManager.ConvertIdToStageNo(stageId);

            foreach (QuestType questType in RelevantQuestTypes)
            {
                HashSet<uint> candidateScheduleIds = Characters.QuestManager.GetQuestByStageNo(questType, stageNo);
                if (candidateScheduleIds.Count == 0)
                {
                    continue;
                }

                HashSet<uint> activeScheduleIds = Characters.QuestManager.GetActiveQuestScheduleIds(client, questType);
                foreach (uint scheduleId in candidateScheduleIds)
                {
                    if (!activeScheduleIds.Contains(scheduleId))
                    {
                        continue;
                    }

                    Quests.Quest quest = Characters.QuestManager.GetQuestByScheduleId(scheduleId);
                    if (quest is not null)
                    {
                        return quest;
                    }
                }
            }

            return null;
        }
    }
}
