/**
 * @brief Breakdown of Order
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.BreakdownOfReason;
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.SpunTogetherHope;

    private class EnemyGroupId
    {
        public const uint BlackSwordElan = 10;
        public const uint BlackSwordSacredDrops = 11;
        public const uint BlackSwordSpiritRoost = 12;
        public const uint ShadoleanDarkness = 13;
        public const uint VortexOfStagnation = 14;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(100));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.ThoseWhoFollowTheDragon));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 1000000);
        AddWalletReward(WalletType.Gold, 110000);
        AddWalletReward(WalletType.RiftPoints, 11000);

        AddFixedItemReward(ItemId.UnappraisedCloudTrinketGeneral, 2);
        AddFixedItemReward(ItemId.ApUrtecaMountains, 50);
        AddFixedItemReward(ItemId.RoyalCrestMedalUrtecaDistrict, 5);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.BlackSwordElan, Stage.ElanWaterGrove, 15, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackKnightPhantomOpaque, 100, 25000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.DarkSkeletonBrute, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.DarkSkeleton, 100, 5000, 2),
            LibDdon.Enemy.Create(EnemyId.DarkSkeleton, 100, 5000, 3),
        });

        AddEnemies(EnemyGroupId.BlackSwordSacredDrops, Stage.BetweenSacredDrops, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Wight0, 100, 25000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.Grigori, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.Grigori, 100, 5000, 2),
            LibDdon.Enemy.Create(EnemyId.BeardedGrigori, 100, 5000, 3),
        });

        AddEnemies(EnemyGroupId.BlackSwordSpiritRoost, Stage.SpiritDragonsRoost0, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.CursedDragon, 100, 120000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.BeardedGrigori, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.Grigori, 100, 5000, 2),
            LibDdon.Enemy.Create(EnemyId.Grigori, 100, 5000, 3),
        });

        AddEnemies(EnemyGroupId.ShadoleanDarkness, Stage.DarknessShroudedShadoleanGreatTemple1, 7, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Abaddon0, 100, 120000, 0)
                .SetIsBoss(true),
        });

        AddEnemies(EnemyGroupId.VortexOfStagnation, Stage.DarknessShroudedShadoleanGreatTemple1, 7, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackSword1, 100, 120000, 0)
                .SetIsBoss(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 30273)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason33)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.LookoutCastle.TheCrewEndSeason33)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8320);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.AudienceChamber, NpcId.TheWhiteDragon, 30274)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 30275);
        process0.AddPlayEventBlock(QuestAnnounceType.Update, Stage.AudienceChamber, 240, 6);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.Joseph, 30275)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8675);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.FaranaPlains0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.FaranaPlains0, 0, 0, NpcId.Musel0, 31178)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8655);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.KingalCanyon);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.KingalCanyon, 2, 0, NpcId.Gearoid0, 30276)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8322)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8323);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.ElanWaterGrove);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.ElanWaterGrove, 0, 0, NpcId.Mordred0, 30277);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.ElanWaterGrove, -53706, 8101, 35278);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.BlackSwordElan);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.ElanWaterGrove, 1, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.ElanWaterGrove, 0, 0, NpcId.Mordred0, 30940)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8739)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8327);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.BetweenSacredDrops);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.BetweenSacredDrops, 1, 0, NpcId.Gavan, 31335);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.BlackSwordSacredDrops);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.BetweenSacredDrops, 0, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.BetweenSacredDrops, 1, 0, NpcId.Gavan, 31336)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8321)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8656);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.HollowofBeginnings1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.HollowofBeginnings1, 1, 0, NpcId.Gearoid0, 31208)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8331)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8338);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.SpiritDragonsRoost0);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.SpiritDragonsRoost0, -200, 25, -3134);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.BlackSwordSpiritRoost);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.SpiritDragonsRoost0, 0, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.SpiritDragonsRoost0, 1, 0, NpcId.AdairDonnchadh0, 30278)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8344);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.DarknessShroudedShadoleanGreatTemple1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.DarknessShroudedShadoleanGreatTemple1, 0, 0, NpcId.Gurdolin3, 30942);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.TheWhiteDragon, 31184)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 31047);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.Joseph, 31047);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheWhiteDragonTemple0, NpcId.Travers1, 31051);
        // Crystal War EXM is not implemented; auto-advance the clear objective.
        process0.AddDelayBlock(QuestAnnounceType.Update, 0, 5);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheWhiteDragonTemple0, NpcId.Travers1, 31053);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.TheWhiteDragon, 31185)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8347);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.DarknessShroudedShadoleanGreatTemple1);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.DarknessShroudedShadoleanGreatTemple1, 0, 999, 813);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.ShadoleanDarkness)
            .AddResultCmdSetDiePlayerReturnPos(Stage.DarknessShroudedShadoleanGreatTemple1, 2, 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.VortexOfStagnation)
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.HollowofBeginnings1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.HollowofBeginnings1, 1, 0, NpcId.Gearoid0, 30280);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.Joseph, 30281);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
