namespace Arrowgene.Ddon.Shared.Model.Quest
{
    /// <summary>
    /// Defines the equipment item-level window used to roll loot for treasure chests inside
    /// Extreme Mission and campaign quest instances, bucketed by the quest's base_level.
    ///
    /// Item level (ClientItemInfo.Level, itemlist.csv "Level" column) is used rather than
    /// ClientItemInfo.Rank because Rank does not correlate consistently with a piece of
    /// equipment's intended character level across the full item list (confirmed by direct
    /// inspection of itemlist.csv) - Rank is only a reliable tier signal within the small,
    /// hand-curated item pools BitterblackMazeManager draws from. Level is also only
    /// populated for Equipment (Category 3) and Job items (Category 5) - materials and
    /// consumables have no per-item level data in itemlist.csv, so this generator is
    /// currently scoped to equipment rewards only. See
    /// docs/quests/loot_generator_plan_exm_and_campaign.md for details.
    /// </summary>
    public class QuestLootRange
    {
        /// <summary>Inclusive lower bound of the quest base_level this bucket applies to.</summary>
        public uint MinLevel { get; set; }

        /// <summary>Inclusive upper bound of the quest base_level this bucket applies to.</summary>
        public uint MaxLevel { get; set; }

        /// <summary>Inclusive minimum equipment item level for ordinary (non-boss-room) chests.</summary>
        public byte NormalItemLevelMin { get; set; }

        /// <summary>Inclusive maximum equipment item level for ordinary (non-boss-room) chests.</summary>
        public byte NormalItemLevelMax { get; set; }

        /// <summary>Inclusive minimum equipment item level for Extreme Mission boss-room chests.</summary>
        public byte BossItemLevelMin { get; set; }

        /// <summary>Inclusive maximum equipment item level for Extreme Mission boss-room chests.</summary>
        public byte BossItemLevelMax { get; set; }

        /// <summary>
        /// Bias (0-1, lower favors higher rolls) applied when rolling boss-room chests to
        /// favor the top of the Boss item-level window. Lower values skew rolls further
        /// toward the rarest/highest-level items available in that window.
        /// </summary>
        public double BossRareChanceBonus { get; set; }

        /// <summary>Inclusive minimum ClientItemInfo.Rank for signature Extreme Mission rare
        /// materials (QuestRareMaterials.json) eligible to drop in this bracket.</summary>
        public byte RareMaterialRankMin { get; set; }

        /// <summary>Inclusive maximum ClientItemInfo.Rank for signature Extreme Mission rare
        /// materials (QuestRareMaterials.json) eligible to drop in this bracket.</summary>
        public byte RareMaterialRankMax { get; set; }

        /// <summary>
        /// Chance (0-1) that an ordinary (non-boss-room) Extreme Mission chest rolls a
        /// signature rare material instead of following the normal Equipment/Material split.
        /// Only applies to Extreme Mission quests (QuestUtils.IsExmQuest).
        /// </summary>
        public double RareMaterialChanceNormal { get; set; }

        /// <summary>
        /// Chance (0-1) that an Extreme Mission boss-room chest rolls a signature rare
        /// material instead of following the normal Equipment/Material split. Only applies
        /// to Extreme Mission quests (QuestUtils.IsExmQuest). Should be notably higher than
        /// RareMaterialChanceNormal so the final chests are meaningfully more rewarding.
        /// </summary>
        public double RareMaterialChanceBoss { get; set; }

        public bool ContainsLevel(uint level)
        {
            return level >= MinLevel && level <= MaxLevel;
        }
    }
}
