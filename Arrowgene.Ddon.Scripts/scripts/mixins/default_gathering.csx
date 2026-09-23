#load "libs.csx"

private static class Settings
{
    public static double DefaultGatherDropsRandomBias
    {
        get
        {
            return LibDdon.GetSetting<double>("GameServerSettings", "DefaultGatherDropsRandomBias");
        }
    }

    public static int DefaultGatherDropMaxSlots
    {
        get
        {
            return LibDdon.GetSetting<int>("GameServerSettings", "DefaultGatherDropMaxSlots");
        }
    }

    public static int MaximumDropsPerDefaultGatherRoll
    {
        get
        {
            return LibDdon.GetSetting<int>("GameServerSettings", "MaximumDropsPerDefaultGatherRoll");
        }
    }
}

private class GatheringExtensions
{
    public const int MIN_GATHERING_RANK = 1;
    public const int MAX_GATHERING_RANK = 11;

    private static readonly Dictionary<GatheringType, int> ChestModifierRank = new Dictionary<GatheringType, int>()
    {
        [GatheringType.OM_GATHER_NONE] = 0,
        [GatheringType.OM_GATHER_TREA_OLD] = 0,
        [GatheringType.OM_GATHER_SHIP] = 1,
        [GatheringType.OM_GATHER_TREA_TREE] = 1,
        [GatheringType.OM_GATHER_KEY_LV1] = 1,
        [GatheringType.OM_GATHER_KEY_LV2] = 2,
        [GatheringType.OM_GATHER_TREA_IRON] = 2,
        [GatheringType.OM_GATHER_TREA_SILVER] = 3,
        [GatheringType.OM_GATHER_KEY_LV3] = 3,
        [GatheringType.OM_GATHER_TREA_GOLD] = 4,
        [GatheringType.OM_GATHER_KEY_LV4] = 4,
    };

    private static readonly Dictionary<OmGatheringPoint, int> TreasureChestBaseRank = new Dictionary<OmGatheringPoint, int>()
    {
        [OmGatheringPoint.BrownChest] = 1,
        [OmGatheringPoint.IronChest] = 2,
        [OmGatheringPoint.TreasureChest] = 3,
        [OmGatheringPoint.SmallRoundChest0] = 3,
        [OmGatheringPoint.SmallRoundChest1] = 3,
        [OmGatheringPoint.BronzeChest] = 4,
        [OmGatheringPoint.SilverChest] = 5,
        [OmGatheringPoint.GoldChest] = 6,
        [OmGatheringPoint.PurpleChest] = 7,
    };

    public static int GetTreasureChestRank(GatheringSpotInfo spotInfo)
    {
        if (!spotInfo.UnitId.IsTreasureChest())
        {
            return MIN_GATHERING_RANK;
        }

        var baseRank = TreasureChestBaseRank.GetValueOrDefault(spotInfo.UnitId, MIN_GATHERING_RANK);
        var modifierRank = ChestModifierRank.GetValueOrDefault(spotInfo.GatheringType, 0);
        return baseRank + modifierRank;
    }

    private static readonly Dictionary<GatheringType, int> LumberModifierRank = new Dictionary<GatheringType, int>()
    {
        [GatheringType.OM_GATHER_NONE] = 0,
        [GatheringType.OM_GATHER_TREE_LV1] = 1,
        [GatheringType.OM_GATHER_TREE_LV2] = 2,
        [GatheringType.OM_GATHER_TREE_LV3] = 3,
        [GatheringType.OM_GATHER_TREE_LV4] = 4,
    };

    private static readonly Dictionary<GatheringType, int> GemstoneModifierRank = new Dictionary<GatheringType, int>()
    {
        [GatheringType.OM_GATHER_NONE] = 0,
        [GatheringType.OM_GATHER_JWL_LV1] = 1,
        [GatheringType.OM_GATHER_JWL_LV2] = 2,
        [GatheringType.OM_GATHER_JWL_LV3] = 3,
    };

    private static readonly Dictionary<GatheringType, int> OreModiferRank = new Dictionary<GatheringType, int>()
    {
        [GatheringType.OM_GATHER_NONE] = 0,
        [GatheringType.OM_GATHER_CRST_LV1] = 1,
        [GatheringType.OM_GATHER_CRST_LV2] = 2,
        [GatheringType.OM_GATHER_CRST_LV3] = 3,
        [GatheringType.OM_GATHER_CRST_LV4] = 4,
    };

    private static readonly Dictionary<GatheringType, int> CorpseModifier = new Dictionary<GatheringType, int>()
    {
        [GatheringType.OM_GATHER_NONE] = 0,
        [GatheringType.OM_GATHER_CORPSE] = 1,
        [GatheringType.OM_GATHER_DRAGON] = 2,
    };


    public static int GetGatherSpotRank(GatheringSpotInfo spotInfo)
    {
        switch (spotInfo.UnitId.GetGatheringPointType())
        {
            case GatheringPointType.TreasureChest:
                return GetTreasureChestRank(spotInfo);
            case GatheringPointType.Lumber:
                return MIN_GATHERING_RANK + LumberModifierRank[spotInfo.GatheringType];
            case GatheringPointType.Gemstone:
                return MIN_GATHERING_RANK + GemstoneModifierRank[spotInfo.GatheringType];
            case GatheringPointType.Ore:
                return MIN_GATHERING_RANK + OreModiferRank[spotInfo.GatheringType];
            case GatheringPointType.Corpse:
                return MIN_GATHERING_RANK + CorpseModifier[spotInfo.GatheringType];
        }
        return MIN_GATHERING_RANK;
    }
}

public class Mixin : IDefaultGatherMixin
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(Mixin));
    private const double LockedChestBonusEquipmentDropChance = 0.15;
    private const double VariantQualityBias = 1.6;
    private static readonly Lazy<Dictionary<(byte Level, ItemSubCategory SubCategory, EquipJobList? JobGroup), byte>> TopRankByEquipmentLane =
        new(BuildTopRankByEquipmentLane);
    private static readonly Lazy<HashSet<(byte Level, ItemSubCategory SubCategory, EquipJobList? JobGroup)>> MultiRankEquipmentLanes =
        new(BuildMultiRankEquipmentLanes);

    private static readonly HashSet<GatheringType> TreasureLikeGatheringTypes = new()
    {
        GatheringType.OM_GATHER_KEY_LV1,
        GatheringType.OM_GATHER_KEY_LV2,
        GatheringType.OM_GATHER_KEY_LV3,
        GatheringType.OM_GATHER_KEY_LV4,
        GatheringType.OM_GATHER_TREA_OLD,
        GatheringType.OM_GATHER_TREA_TREE,
        GatheringType.OM_GATHER_TREA_IRON,
        GatheringType.OM_GATHER_TREA_SILVER,
        GatheringType.OM_GATHER_TREA_GOLD,
        GatheringType.OM_GATHER_ANTIQUE,
    };

    private static readonly Dictionary<OmGatheringPoint, double> TreasureChestEquipmentDropChance = new()
    {
        [OmGatheringPoint.IronChest] = 0.15,
        [OmGatheringPoint.BrownChest] = 0.25,
        [OmGatheringPoint.TreasureChest] = 0.35,
        [OmGatheringPoint.BronzeChest] = 0.45,
        [OmGatheringPoint.SilverChest] = 0.55,
        [OmGatheringPoint.GoldChest] = 0.65,
        [OmGatheringPoint.PurpleChest] = 0.80,
        [OmGatheringPoint.SmallRoundChest0] = 0.60,
        [OmGatheringPoint.SmallRoundChest1] = 0.60,
        [OmGatheringPoint.BronzeChest1] = 0.60,
        [OmGatheringPoint.PearlescentChest1] = 0.80,
        [OmGatheringPoint.OrangeSealedChest] = 0.80,
        [OmGatheringPoint.PurpleSealedChest] = 0.80,
        [OmGatheringPoint.PearlescentChest] = 0.80,
    };

    private const double LockedChestBaselineEquipmentDropChance = 0.50;

    public override List<InstancedGatheringItem> GenerateGatheringDrops(GameClient client, StageLayoutId stageLayoutId, uint index)
    {
        if (StageManager.IsBitterBlackMazeStageId(stageLayoutId) || StageManager.IsEpitaphRoadStageId(stageLayoutId))
        {
            return new();
        }

        List<InstancedGatheringItem> results = new();
        if (LibDdon.Assets.DefaultGatheringDropsAsset.SpotDefaultDrops.ContainsKey((stageLayoutId, index)))
        {
            return new();
        }

        return HandleAreaDrops(client, stageLayoutId, index);
    }

    private List<InstancedGatheringItem> HandleAreaDrops(GameClient client, StageLayoutId stageLayoutId, uint index)
    {
        var stage = Stage.StageInfoFromStageLayoutId(stageLayoutId);
        if (!LibDdon.Assets.GatheringSpotInfoAsset.GatheringInfoMap.ContainsKey(stage.StageNo))
        {
            return new();
        }

        var stageSpots = LibDdon.Assets.GatheringSpotInfoAsset.GatheringInfoMap[stage.StageNo];
        if (!stageSpots.ContainsKey((stageLayoutId.GroupId, index)))
        {
            return new();
        }

        var areaId = stage.AreaId;
        if (stage.StageId == Stage.Lestania.StageId)
        {
            areaId = client.Character.AreaId;
            if (areaId == QuestAreaId.None)
            {
                // Default to hidell plains so something can drop
                areaId = QuestAreaId.HidellPlains;
            }
        }

        if (!LibDdon.Assets.DefaultGatheringDropsAsset.AreaDefaultDrops.ContainsKey(areaId))
        {
            return new();
        }

        var spotInfo = stageSpots[(stageLayoutId.GroupId, index)];
        var isTreasureLike = TreasureLikeGatheringTypes.Contains(spotInfo.GatheringType) || spotInfo.UnitId.IsTreasureChest();
        var isLockedChest = spotInfo.GatheringType.IsLockedChest();

        Logger.Debug($"{stageLayoutId}.{index}  OmType={spotInfo.UnitId}, GatheringType={spotInfo.GatheringType}");

        var dropCategories = GetDropCategoriesForSpot(spotInfo);
        var dropsForSpot = dropCategories
            .Select(x => LibDdon.Assets.DefaultGatheringDropsAsset.AreaDefaultDrops[areaId][x])
            .Where(x => x.Count > 0)
            .SelectMany(x => x)
            .ToList();

        if (dropsForSpot.Count == 0 && isTreasureLike)
        {
            // Some treasure-like points are authored with non-chest unit ids.
            // If their mapped categories are empty, fall back to all categories.
            Logger.Debug($"{stageLayoutId}.{index} using treasure-like all-category fallback");
            dropsForSpot = DropCategoryExtension.All
                .Select(x => LibDdon.Assets.DefaultGatheringDropsAsset.AreaDefaultDrops[areaId][x])
                .Where(x => x.Count > 0)
                .SelectMany(x => x)
                .ToList();
        }

        var stageDrops = dropsForSpot.Where(x => x.StageId == stage.StageId).ToList();
        if (stageDrops.Count == 0 && isTreasureLike)
        {
            // Some treasure pile/chest spots are missing explicit stage rows in DefaultGatheringDrops.
            // Fall back to area-level treasure categories instead of returning an empty pile.
            Logger.Debug($"{stageLayoutId}.{index} using treasure-like stage fallback");
            stageDrops = dropsForSpot;
        }

        var dropTable = stageDrops
            .GroupBy(x => x.ItemId)
            .Select(x => x.First())
            .OrderBy(x => x.ItemLevel)
            .ToDictionary(x => x.ItemId, x => x);
        if (dropTable.Count == 0)
        {
            return new();
        }

        var gatherPointRank = GatheringExtensions.GetGatherSpotRank(spotInfo);
        var potentialSlots = Settings.DefaultGatherDropMaxSlots + (int)(gatherPointRank - GatheringExtensions.MIN_GATHERING_RANK);

        // Create a list which holds all the items we can roll
        var rolls = dropTable.Keys.ToList();

        var bias = FindRollBiasForSpot(gatherPointRank);

        // Determine how many items to generate
        var slots = Random.Shared.WeightedNext(1, potentialSlots + 1, Settings.DefaultGatherDropsRandomBias);
        var results = RollDrops(slots, rolls, dropTable, bias);

        var shouldRollEquipment = !HasActiveQuestChest(client, stageLayoutId)
            && Random.Shared.NextDouble() < GetTreasureChestEquipmentDropChance(spotInfo, isTreasureLike, isLockedChest);

        if (shouldRollEquipment)
        {
            // If the spot is backed by a fixed authored drop table, skip the normal material roll
            // to avoid stacking random junk on top of the equipment reward from the same chest.
            if (LibDdon.Assets.DefaultGatheringDropsAsset.SpotDefaultDrops.ContainsKey((stageLayoutId, index)))
            {
                return RollTreasureChestEquipment(client, stageLayoutId, areaId, gatherPointRank);
            }

            results.AddRange(RollTreasureChestEquipment(client, stageLayoutId, areaId, gatherPointRank));
        }

        return results;
    }

    private double FindRollBiasForSpot(int rank)
    {
        var preferenceScore = GatheringExtensions.MAX_GATHERING_RANK - rank + 1;

        double minBias = Settings.DefaultGatherDropsRandomBias; // Higher makes more skewed to common items
        double maxBias = 0.3; // lower makes it more skewed to rarer items

        double t = (rank - 1.0) / (GatheringExtensions.MAX_GATHERING_RANK - 1.0); // Normalized rank [0, 1]
        return minBias * Math.Pow(maxBias / minBias, t);
    }

    private double GetTreasureChestEquipmentDropChance(GatheringSpotInfo spotInfo, bool isTreasureLike, bool isLockedChest)
    {
        double chance = TreasureChestEquipmentDropChance.GetValueOrDefault(spotInfo.UnitId, isTreasureLike ? 0.05 : 0.0);

        if (isLockedChest)
        {
            chance = Math.Max(LockedChestBaselineEquipmentDropChance, chance);
            chance = Math.Min(1.0, chance + LockedChestBonusEquipmentDropChance);
        }

        return chance;
    }

    private List<InstancedGatheringItem> RollDrops(int slots, List<ItemId> rolls, Dictionary<ItemId, DefaultGatheringDrop> dropTable, double rollBias)
    {
        Logger.Debug($"GatheringBias={rollBias}");

        var results = new List<InstancedGatheringItem>();
        for (int i = 0; i < slots && rolls.Count > 0; i++)
        {
            var itemId = rolls[Random.Shared.WeightedNext(rolls.Count, rollBias)];

            var item = dropTable[itemId];

            var drop = new InstancedGatheringItem()
            {
                ItemId = item.ItemId,
                Quality = item.Quality
            };

            if (item.MaxAmount == 0)
            {
                drop.ItemNum = (uint)Random.Shared.WeightedNext((int)item.MinAmount, Settings.MaximumDropsPerDefaultGatherRoll + 1, Settings.DefaultGatherDropsRandomBias);
            }
            else
            {
                drop.ItemNum = (uint)Random.Shared.WeightedNext((int)item.MinAmount, (int)item.MaxAmount + 1, Settings.DefaultGatherDropsRandomBias);
            }

            if (drop.ItemNum == 0)
            {
                // Skip item since none got generated
                // TODO: Is this a configuration issue?
                continue;
            }

            rolls.Remove(itemId);
            results.Add(drop);
        }
        return results;
    }

    private List<InstancedGatheringItem> RollTreasureChestEquipment(GameClient client, StageLayoutId stageLayoutId, QuestAreaId areaId, int gatherPointRank)
    {
        uint equipmentLevel = ResolveStageEquipmentLevel(client, stageLayoutId, areaId, gatherPointRank);
        uint minLevel = equipmentLevel > 3 ? equipmentLevel - 3 : 1;
        uint maxLevel = equipmentLevel + 3;

        var candidates = LibDdon.Assets.ClientItemInfos.Values
            .Where(item => item.Category == 3 && item.Level.HasValue
                && item.Level.Value >= minLevel
                && item.Level.Value <= maxLevel)
            .ToList();

        if (candidates.Count == 0)
        {
            return new();
        }

        var filtered = ExcludeCraftReservedTopRank(candidates)
            .ToList();

        if (filtered.Count == 0)
        {
            filtered = candidates;
        }

        if (filtered.Count == 0)
        {
            return new();
        }

        var nameBuckets = filtered
            .GroupBy(item => item.Name)
            .Select(group => group.ToList())
            .ToList();

        var selectedNameBucket = nameBuckets[Random.Shared.Next(nameBuckets.Count)];
        var selected = SelectVariantPreferUnenhanced(selectedNameBucket);

        Logger.Debug($"Rolled treasure chest equipment: {selected.Name} <{selected.ItemId}> (Rank={selected.Rank}, Level={selected.Level}, Quality={selected.Quality}, Window={minLevel}-{maxLevel})");

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

    private static ClientItemInfo SelectVariantPreferUnenhanced(List<ClientItemInfo> variants)
    {
        if (variants.Count == 1)
        {
            return variants[0];
        }

        // Lower quality values represent less-enhanced variants and should be favored.
        var ordered = variants
            .OrderBy(item => item.Quality ?? 0)
            .ThenBy(item => item.ItemId)
            .ToList();

        int index = Random.Shared.WeightedNext(ordered.Count, VariantQualityBias);
        return ordered[index];
    }

    private uint ResolveStageEquipmentLevel(GameClient client, StageLayoutId stageLayoutId, QuestAreaId areaId, int gatherPointRank)
    {
        var stageLevels = LibDdon.Assets.EnemySpawnAsset.Enemies
            .Where(entry => entry.Key.Id == stageLayoutId.Id)
            .SelectMany(entry => entry.Value)
            .Select(enemy => (uint)enemy.Lv)
            .Where(level => level > 0)
            .ToList();

        if (stageLevels.Count > 0)
        {
            uint resolvedLevel = (uint)Math.Round(stageLevels.Average(level => (double)level));
            Logger.Debug($"Resolved locked chest stage level from enemy spawn data: StageId={stageLayoutId.Id}, Level={resolvedLevel}");
            return resolvedLevel;
        }

        return client.Character.AreaRanks.GetValueOrDefault(areaId)?.Rank ?? (uint)gatherPointRank;
    }

    private static bool HasActiveQuestChest(GameClient client, StageLayoutId stageLayoutId)
    {
        return QuestManager.CollectQuestScheduleIds(client, stageLayoutId).Any();
    }

    private static List<ClientItemInfo> ExcludeCraftReservedTopRank(List<ClientItemInfo> candidates)
    {
        return candidates
            .Where(item =>
            {
                if (!item.Level.HasValue)
                {
                    return false;
                }

                var lane = (item.Level.Value, item.SubCategory, item.JobGroup);
                if (!MultiRankEquipmentLanes.Value.Contains(lane))
                {
                    return true;
                }

                if (!TopRankByEquipmentLane.Value.TryGetValue(lane, out byte topRank))
                {
                    return true;
                }

                return item.Rank < topRank;
            })
            .ToList();
    }

    private static Dictionary<(byte Level, ItemSubCategory SubCategory, EquipJobList? JobGroup), byte> BuildTopRankByEquipmentLane()
    {
        return LibDdon.Assets.ClientItemInfos.Values
            .Where(item => item.Category == 3 && item.Level.HasValue)
            .GroupBy(item => (item.Level!.Value, item.SubCategory, item.JobGroup))
            .ToDictionary(group => group.Key, group => group.Max(item => item.Rank));
    }

    private static HashSet<(byte Level, ItemSubCategory SubCategory, EquipJobList? JobGroup)> BuildMultiRankEquipmentLanes()
    {
        return LibDdon.Assets.ClientItemInfos.Values
            .Where(item => item.Category == 3 && item.Level.HasValue)
            .GroupBy(item => (item.Level!.Value, item.SubCategory, item.JobGroup))
            .Where(group => group.Select(item => item.Rank).Distinct().Count() > 1)
            .Select(group => group.Key)
            .ToHashSet();
    }

    private static Dictionary<GatheringPointType, List<DropCategory>> DropCategories = new Dictionary<GatheringPointType, List<DropCategory>>()
    {
        [GatheringPointType.Alchemy] = [DropCategory.Liquids, DropCategory.Lumber, DropCategory.Other],
        [GatheringPointType.Box] = [
            DropCategory.Consumable, DropCategory.Dye, DropCategory.Thread, DropCategory.Fabric, DropCategory.Ingots, DropCategory.Ore,
            DropCategory.Gemstones, DropCategory.Scrolls, DropCategory.Leather
        ],
        [GatheringPointType.Corpse] = [
            DropCategory.Meat, DropCategory.Claws, DropCategory.Bones, DropCategory.Fang, DropCategory.Hides, DropCategory.Horns,
            DropCategory.Furs, DropCategory.Feathers
        ],
        [GatheringPointType.Furniture] = [DropCategory.Consumable, DropCategory.Dye, DropCategory.Thread, DropCategory.Fabric, DropCategory.CrestArmor, DropCategory.CrestWeapon],
        [GatheringPointType.Gemstone] = [DropCategory.Gemstones],
        [GatheringPointType.Lumber] = [DropCategory.Lumber],
        [GatheringPointType.Mushroom] = [DropCategory.Mushrooms],
        [GatheringPointType.Ore] = [DropCategory.Ore],
        [GatheringPointType.Plants] = [DropCategory.Plants],
        [GatheringPointType.Sand] = [DropCategory.Sand],
        [GatheringPointType.SealedTreasureChest] = [DropCategory.Equipment, DropCategory.Jewelry, DropCategory.Unappraised, DropCategory.Regional],
        [GatheringPointType.Shell] = [DropCategory.Shell],
        [GatheringPointType.TreasureChest] = DropCategoryExtension.All,
        [GatheringPointType.Twinkle] = DropCategoryExtension.All,
        [GatheringPointType.Water] = [DropCategory.Liquids],
    };

    private List<DropCategory> GetDropCategoriesForSpot(GatheringSpotInfo spotInfo)
    {
        var gatheringPointType = spotInfo.UnitId.GetGatheringPointType();
        if (!DropCategories.ContainsKey(gatheringPointType))
        {
            return new();
        }
        return DropCategories[gatheringPointType];
    }
}

return new Mixin();
