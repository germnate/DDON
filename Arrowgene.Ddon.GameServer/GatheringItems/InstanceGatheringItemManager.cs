using Arrowgene.Ddon.GameServer.GatheringItems.Generators;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.GatheringItems
{
    public class InstanceGatheringItemManager
    {
        private readonly Dictionary<StageLayoutId, Dictionary<uint, List<InstancedGatheringItem>>> InstancedItems;
        private readonly Dictionary<(byte Level, ItemSubCategory SubCategory, EquipJobList? JobGroup), byte> TopRankByEquipmentLane;
        private readonly HashSet<(byte Level, ItemSubCategory SubCategory, EquipJobList? JobGroup)> MultiRankEquipmentLanes;

        private readonly GameClient Client;
        private readonly DdonGameServer Server;
        private readonly List<IGatheringGenerator> Generators;

        public InstanceGatheringItemManager(GameClient client, DdonGameServer server)
        {
            Client = client;
            Server = server;
            InstancedItems = new();
            TopRankByEquipmentLane = BuildTopRankByEquipmentLane();
            MultiRankEquipmentLanes = BuildMultiRankEquipmentLanes();
            Generators = new()
            {
                new OneOffGatheringItemGenerator(server),
                new DefaultGatheringItemGenerator(server),
                new GatheringTableGatheringItemGenerator(server),
                new BitterblackGatheringItemGenerator(server),
                new EpitaphRoadGatheringItemGenerator(server),
                new QuestInstanceGatheringItemGenerator(server)
            };
        }

        public Dictionary<Type, List<InstancedGatheringItem>> Generate(StageLayoutId stageId, uint index)
        {
            return Generators
                .Where(x => x.IsEnabled())
                .ToDictionary(key => key.GetType(), val => val.Generate(Client, stageId, index));
        }

        private uint Assign(StageLayoutId stageId, uint index, List<InstancedGatheringItem> items)
        {
            uint currentIndex = index;
            if (InstancedItems.TryGetValue(stageId, out var stageItems))
            {
                InstancedItems[stageId][currentIndex] = items;
            }
            else
            {
                InstancedItems[stageId] = new()
                {
                    { currentIndex, items }
                };
            }
            return currentIndex;
        }

        public (bool New, List<InstancedGatheringItem> Items) FetchOrGenerate(StageLayoutId stageId, uint index)
        {
            if (InstancedItems.TryGetValue(stageId, out var stageItems) 
                && stageItems.TryGetValue(index, out var returnItems))
            {
                return (false, returnItems);
            }
            else
            {
                var items = Generate(stageId, index).SelectMany(x => x.Value).ToList();
                if (IsLockedChest(stageId, index))
                {
                    items = FilterCraftReservedTopRank(items);
                }

                Assign(stageId, index, items);
                return (true, items);
            }
        }

        private Dictionary<(byte Level, ItemSubCategory SubCategory, EquipJobList? JobGroup), byte> BuildTopRankByEquipmentLane()
        {
            return Server.AssetRepository.ClientItemInfos.Values
                .Where(item => item.Category == 3 && item.Level.HasValue)
                .GroupBy(item => (item.Level!.Value, item.SubCategory, item.JobGroup))
                .ToDictionary(group => group.Key, group => group.Max(item => item.Rank));
        }

        private List<InstancedGatheringItem> FilterCraftReservedTopRank(List<InstancedGatheringItem> items)
        {
            return items
                .Where(item => !IsCraftReservedTopRank(item.ItemId))
                .ToList();
        }

        private bool IsCraftReservedTopRank(ItemId itemId)
        {
            if (!Server.AssetRepository.ClientItemInfos.TryGetValue(itemId, out ClientItemInfo itemInfo))
            {
                return false;
            }

            if (itemInfo.Category != 3 || !itemInfo.Level.HasValue)
            {
                return false;
            }

            var lane = (itemInfo.Level.Value, itemInfo.SubCategory, itemInfo.JobGroup);
            if (!MultiRankEquipmentLanes.Contains(lane))
            {
                return false;
            }

            if (!TopRankByEquipmentLane.TryGetValue(lane, out byte topRank))
            {
                return false;
            }

            return itemInfo.Rank == topRank;
        }

        private HashSet<(byte Level, ItemSubCategory SubCategory, EquipJobList? JobGroup)> BuildMultiRankEquipmentLanes()
        {
            return Server.AssetRepository.ClientItemInfos.Values
                .Where(item => item.Category == 3 && item.Level.HasValue)
                .GroupBy(item => (item.Level!.Value, item.SubCategory, item.JobGroup))
                .Where(group => group.Select(item => item.Rank).Distinct().Count() > 1)
                .Select(group => group.Key)
                .ToHashSet();
        }

        private bool IsLockedChest(StageLayoutId stageId, uint index)
        {
            uint stageNo = StageManager.ConvertIdToStageNo(stageId);
            if (!Server.AssetRepository.GatheringSpotInfoAsset.GatheringInfoMap.TryGetValue(stageNo, out var stageSpots))
            {
                return IsHidellCatacombsLockedChestFallback(stageId, index);
            }

            if (!stageSpots.TryGetValue((stageId.GroupId, index), out GatheringSpotInfo spotInfo))
            {
                return false;
            }

            return spotInfo.GatheringType.IsLockedChest();
        }

        private static bool IsHidellCatacombsLockedChestFallback(StageLayoutId stageId, uint index)
        {
            if (index >= 4)
            {
                return false;
            }

            return stageId.Id == Stage.HidellCatacombs0.StageId
                || stageId.Id == Stage.HidellCatacombs1.StageId
                || stageId.Id == Stage.HidellCatacombsDepths.StageId
                || stageId.Id == Stage.HidellCatacombsInnermostDepths.StageId;
        }

        public (bool New, List<InstancedGatheringItem> Items) FetchOrGenerate(CDataStageLayoutId stageLayout, uint index)
        {
            return FetchOrGenerate(stageLayout.AsStageLayoutId(), index);
        }

        public void Clear()
        {
            InstancedItems.Clear();
        }

        public HashSet<byte> GatheredSpots(CDataStageLayoutId stageLayout)
        {
            return [.. InstancedItems.GetValueOrDefault(stageLayout.AsStageLayoutId(), [])
                .Select(x => (byte)x.Key)];
        }

        public HashSet<byte> EmptySpots(CDataStageLayoutId stageLayout)
        {
            return [.. InstancedItems.GetValueOrDefault(stageLayout.AsStageLayoutId(), [])
                .Where(x => x.Value.DefaultIfEmpty().Sum(y => y?.ItemNum) == 0)
                .Select(x => (byte)x.Key)];
        }

        public string Report(StageLayoutId stageId, uint index)
        {
            var infoStrings = FetchOrGenerate(stageId, index).Items.Select(x => $"{Server.AssetRepository.ClientItemInfos[x.ItemId].Name} x{x.ItemNum}");
            return string.Join("\n\t", infoStrings);
        }

        public string Report(Dictionary<Type, List<InstancedGatheringItem>> generateResult)
        {
            var infoStrings = generateResult.SelectMany(t => t.Value.Select(x => $"{Server.AssetRepository.ClientItemInfos[x.ItemId].Name}\tx{x.ItemNum}\t({t.Key.Name})"));
            return string.Join("\n\t", infoStrings);
        }
    }
}
