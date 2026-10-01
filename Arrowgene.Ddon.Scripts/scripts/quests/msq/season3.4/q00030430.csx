/**
 * @brief The White Dragons' Arisen
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheWhiteDragonsArisen;
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheFateOfAll;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(100));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.SpunTogetherHope));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.ErteDeenan, 18, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.LivingArmor, 100, 25000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.DarkSkeletonBrute, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.DarkSkeletonBrute, 100, 5000, 2),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.GardnoxFortress0, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyGorecyclopsLightArmor0, 100, 25000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.WarReadySaurianLightArmor, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.WarReadySaurianLightArmor, 100, 5000, 2),
            LibDdon.Enemy.Create(EnemyId.WarReadySaurianLightArmor, 100, 5000, 3),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.VortexofStagnation0, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackKnightPhantomOpaque, 100, 25000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.MiseryGhost, 100, 5000, 2),
        });

        AddEnemies(EnemyGroupId.Encounter + 3, Stage.DarknessShroudedDreedCastle1, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackKnightPhantomClear, 100, 25000, 0)
                .SetIsBoss(true),
        });

        AddEnemies(EnemyGroupId.Encounter + 4, Stage.EastLandofDarkness, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackKnight, 100, 120000, 0)
                .SetIsBoss(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.TheWhiteDragon, 30298)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8381)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8481)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 31018)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 30992)
            .AddResultCmdQstTalkChg(NpcId.Fabio0, 30299)
            .AddResultCmdQstTalkChg(NpcId.Mayleaf0, 30991);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.Lestania, 0, 0, NpcId.Fabio0, 30299)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8382)
            .AddResultCmdQstTalkChg(NpcId.Fabio0, 30993);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.ErteDeenan);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8662);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.ErteDeenan, 0, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.ErteDeenan, 1, 0, NpcId.Ringdeel0, 31190)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8482)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8395)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8563)
            .AddResultCmdQstTalkChg(NpcId.Ringdeel0, 31191);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.GardnoxFortress0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.GardnoxFortress0, 4, 0, NpcId.Elliot0, 30300)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8399);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.GardnoxFortress0, 1, 0, NpcId.Heinz2, 30997)
            .AddResultCmdQstTalkChg(NpcId.Heinz2, 30998);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.GardnoxFortress0, 0, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8399)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8401);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.GardnoxFortress0, 2, 0, NpcId.Heinz2, 30301)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8483)
            .AddResultCmdQstTalkChg(NpcId.Heinz2, 30999);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.TempleofPurification);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.VortexofStagnation0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8389)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8393)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8663);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2)
            .AddResultCmdSetDiePlayerReturnPos(Stage.VortexofStagnation0, 2, 0)
            .AddResultCmdPlayMessage(31003, 5);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.VortexofStagnation0, 1, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.VortexofStagnation0, 2, 0, NpcId.Gerd1, 30302)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8393)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8394)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8484);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.DarknessShroudedDreedCastle1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8406);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheWhiteDragonTemple0, NpcId.Travers1, 31192);
        // Crystal War EXM is not scripted on this server; auto-advance this story purpose.
        process0.AddDelayBlock(QuestAnnounceType.Update, 0, 5);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheWhiteDragonTemple0, NpcId.Travers1, 31193);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.Lestania, 2, 0, NpcId.Scherzo, 31194)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8681);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.DarknessShroudedDreedCastle1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 3)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8406)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8410)
            .AddResultCmdPlayMessage(31070, 5)
            .AddResultCmdPlayMessage(31071, 5);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.DarknessShroudedDreedCastle1, 2, 0, NpcId.Vanessa0, 31068);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.EastLandofDarkness)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8409);
        process0.AddDestroyGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 4)
            .AddResultCmdSetDiePlayerReturnPos(Stage.EastLandofDarkness, 0, 0)
            .AddResultCmdPlayMessage(30361, 5)
            .AddResultCmdPlayMessage(30371, 5)
            .AddResultCmdPlayMessage(30386, 5);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.DarknessShroudedDreedCastle1, 7)
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.DarknessShroudedDreedCastle1, 2, 0, NpcId.Vanessa0, 31011)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8726);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.Joseph, 30306)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 31018)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 31019);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
