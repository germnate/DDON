/**
 * @brief The Fate of All
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheFateOfAll;
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.None;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(100));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheWhiteDragonsArisen));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 1200000);
        AddWalletReward(WalletType.Gold, 130000);
        AddWalletReward(WalletType.RiftPoints, 13000);

        AddFixedItemReward(ItemId.UnappraisedCloudTrinketGeneral, 2);
        AddFixedItemReward(ItemId.ApUrtecaMountains, 50);
        AddFixedItemReward(ItemId.RoyalCrestMedalUrtecaDistrict, 5);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.MergodaRuinsRoyalPalaceLevel0, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.ShadowGrigori, 100, 25000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.DarkSkeletonBrute, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.DarkSkeletonBrute, 100, 5000, 2),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.MergodaRuinsRoyalPalaceLevel0, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Eliminator, 100, 25000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.LivingArmor, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.LivingArmor, 100, 5000, 2),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.DarknessShroudedMergodaSecurityDistrict1, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.ArisenOfTheBlackDragon, 100, 25000, 0)
                .SetIsBoss(true),
        });

        AddEnemies(EnemyGroupId.Encounter + 3, Stage.AlternativePath, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackDragon1stForm1, 100, 120000, 0)
                .SetIsBoss(true),
        });

        AddEnemies(EnemyGroupId.Encounter + 4, Stage.AlternativePath, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackDragon2ndForm, 100, 120000, 0)
                .SetIsBoss(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.TheWhiteDragon, 30313)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8411)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 31021)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 31022)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 31020);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.AudienceChamber, NpcId.Joseph, 31021);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.Klaus0, 31022)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8412);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.Lestania, 0, 1, NpcId.Lise0, 31024)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 31025);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.MergodaRuinsRoyalPalaceLevel0, 2, 0, NpcId.Theodor, 30314)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8413)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8664);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.MergodaRuinsRoyalPalaceLevel0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8682);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.MergodaRuinsRoyalPalaceLevel0, 0, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.MergodaRuinsRoyalPalaceLevel0, 4, 0, NpcId.Beatrix, 31202)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8418)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8665)
            .AddResultCmdQstTalkChg(NpcId.Iosef, 31203);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.MergodaRuinsRoyalPalaceLevel0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1)
            .AddResultCmdPlayMessage(31204, 5);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.MergodaRuinsRoyalPalaceLevel0, 1, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.MergodaRuinsRoyalPalaceLevel0, 3, 0, NpcId.Theodor, 31195)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8427);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.DarknessShroudedMergodaSecurityDistrict1, 0, 1, NpcId.Lise0, 30315)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 31083);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheWhiteDragonTemple0, NpcId.Travers1, 31074);
        // Crystal War EXM is not scripted on this server; auto-advance this story purpose.
        process0.AddDelayBlock(QuestAnnounceType.Update, 0, 5);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheWhiteDragonTemple0, NpcId.Travers1, 31075);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.DarknessShroudedMergodaSecurityDistrict1, 0, 1, NpcId.Lise0, 31085)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8427)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8428);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.DarknessShroudedMergodaSecurityDistrict1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2)
            .AddResultCmdSetDiePlayerReturnPos(Stage.DarknessShroudedMergodaSecurityDistrict1, 6, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8727)
            .AddResultCmdPlayMessage(30431, 5)
            .AddResultCmdPlayMessage(30432, 5)
            .AddResultCmdPlayMessage(30450, 5);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.DarknessShroudedMergodaSecurityDistrict1, 3, 0, NpcId.Mysial0, 31028)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8757);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.AlternativePath)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8751);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheWhiteDragonTemple0, NpcId.Travers1, 31029);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.AlternativePath);
        process0.AddPlayEventBlock(QuestAnnounceType.Update, Stage.AlternativePath, 0, 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 3)
            .AddResultCmdSetDiePlayerReturnPos(Stage.AlternativePath, 0, 0)
            .AddResultCmdPlayMessage(30317, 5)
            .AddResultCmdPlayMessage(30390, 5);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 4)
            .AddResultCmdPlayMessage(30404, 5)
            .AddResultCmdPlayMessage(30413, 5)
            .AddResultCmdPlayMessage(30426, 5);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.AlternativePath, 5, 0)
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.TheWhiteDragon, 30319)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 31340)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 30416);
        process0.AddProcessEndBlock(true)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheCrewEndSeason34)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.LookoutCastle.TheCrewEndSeason34);
    }
}

return new ScriptedQuest();
