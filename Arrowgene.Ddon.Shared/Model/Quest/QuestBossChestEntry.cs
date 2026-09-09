namespace Arrowgene.Ddon.Shared.Model.Quest
{
    /// <summary>
    /// Identifies a single treasure-chest gathering spot (by quest_id + stage/group/pos) that
    /// should be treated as an Extreme Mission "boss room" chest and rolled against a quest
    /// loot range's BossRank window instead of its NormalRank window. See
    /// QuestBossChests.json and docs/quests/loot_generator_plan_exm_and_campaign.md.
    /// </summary>
    public class QuestBossChestEntry
    {
        public uint QuestId { get; set; }
        public uint StageId { get; set; }
        public uint GroupId { get; set; }
        public uint PosId { get; set; }
    }
}
