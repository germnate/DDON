using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest;

public class InstancedLootPoolItem : LootPoolItem
{
    private string? _uid;

    public uint Color { get; set; }
    public uint PlusValue { get; set; }
    public uint SafetySetting { get; set; }
    public List<StagedRewardItemCrest> Crests { get; set; } = new();

    public override string GetUID()
    {
        _uid ??= Guid.CreateVersion7(DateTimeOffset.UtcNow).ToString();
        return _uid;
    }

    public StagedRewardItem ToStagedRewardItem()
    {
        string uid = GetUID();
        var staged = new StagedRewardItem
        {
            Uid = uid,
            ItemId = (uint)ItemId,
            Num = Num,
            Color = Color,
            PlusValue = PlusValue,
            SafetySetting = SafetySetting,
            Crests = new List<StagedRewardItemCrest>(),
        };
        foreach (var crest in Crests)
        {
            staged.Crests.Add(new StagedRewardItemCrest
            {
                Uid = uid,
                Slot = crest.Slot,
                CrestId = crest.CrestId,
                Level = crest.Level,
            });
        }
        return staged;
    }

    public static InstancedLootPoolItem Create(ItemId itemId, ushort num, uint color = 0, uint plusValue = 0, uint safetySetting = 0)
    {
        return new InstancedLootPoolItem
        {
            ItemId = itemId,
            Num = num,
            Color = color,
            PlusValue = plusValue,
            SafetySetting = safetySetting,
        };
    }
}

public class QuestInstancedFixedRewardItem : QuestRewardItem
{
    public QuestInstancedFixedRewardItem(bool isHidden = false) : base(QuestRewardType.Fixed, isHidden)
    {
    }

    public override List<CDataRewardBoxItem> AsCDataRewardBoxItems(QuestRewardType? rewardTypeOverride = null, bool isHelp = false, uint selectGroupId = 0)
    {
        var results = new List<CDataRewardBoxItem>();
        foreach (var poolItem in LootPool)
        {
            var instanced = (InstancedLootPoolItem)poolItem;
            var staged = instanced.ToStagedRewardItem();
            results.Add(new CDataRewardBoxItem
            {
                ItemId = instanced.ItemId,
                Num = instanced.Num,
                UID = staged.Uid,
                Type = (byte)(rewardTypeOverride ?? RewardType),
                IsHelp = isHelp,
                SelectGroupId = selectGroupId,
                IsInstance = true,
                StagedItem = staged,
            });
        }
        return results;
    }

    public static QuestInstancedFixedRewardItem Create(ItemId itemId, ushort num, uint color = 0, uint plusValue = 0, uint safetySetting = 0, bool isHidden = false)
    {
        var reward = new QuestInstancedFixedRewardItem(isHidden);
        reward.LootPool.Add(InstancedLootPoolItem.Create(itemId, num, color, plusValue, safetySetting));
        return reward;
    }
}

public class QuestInstancedSelectRewardItem : QuestRewardItem
{
    public QuestInstancedSelectRewardItem(bool isHidden = false) : base(QuestRewardType.Select, isHidden)
    {
    }

    public override List<CDataRewardBoxItem> AsCDataRewardBoxItems(QuestRewardType? rewardTypeOverride = null, bool isHelp = false, uint selectGroupId = 0)
    {
        var results = new List<CDataRewardBoxItem>();
        foreach (var poolItem in LootPool)
        {
            var instanced = (InstancedLootPoolItem)poolItem;
            var staged = instanced.ToStagedRewardItem();
            results.Add(new CDataRewardBoxItem
            {
                ItemId = instanced.ItemId,
                Num = instanced.Num,
                UID = staged.Uid,
                Type = (byte)(rewardTypeOverride ?? RewardType),
                IsHelp = isHelp,
                SelectGroupId = selectGroupId,
                IsInstance = true,
                StagedItem = staged,
            });
        }
        return results;
    }

    public static QuestInstancedSelectRewardItem Create(List<(ItemId ItemId, ushort Num, uint Color, uint PlusValue, uint SafetySetting)> items, bool isHidden = false)
    {
        var reward = new QuestInstancedSelectRewardItem(isHidden);
        foreach (var item in items)
        {
            reward.LootPool.Add(InstancedLootPoolItem.Create(item.ItemId, item.Num, item.Color, item.PlusValue, item.SafetySetting));
        }
        return reward;
    }
}
