/**
 * @brief Spun Together Hope
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.SpunTogetherHope;
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheWhiteDragonsArisen;

    private class EnemyGroupId
    {
        public const uint BlackSwordSecretSpring = 10;
        public const uint BlackSwordDacreim = 11;
        public const uint BlackSwordEvilDragon = 12;
        public const uint MegadoDarkness = 13;
        public const uint VortexOfStagnation = 14;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(100));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.BreakdownOfReason));
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
        AddEnemies(EnemyGroupId.BlackSwordSecretSpring, Stage.BeforetheSecretSpring, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackKnightPhantomOpaque, 100, 25000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.FlameSkeletonBrute, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.FlameSkeleton, 100, 5000, 2),
            LibDdon.Enemy.Create(EnemyId.FlameSkeleton, 100, 5000, 3),
        });

        AddEnemies(EnemyGroupId.BlackSwordDacreim, Stage.DacreimFortress4, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyGoremanticoreLightArmor, 100, 25000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 100, 5000, 2),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 100, 5000, 3),
        });

        AddEnemies(EnemyGroupId.BlackSwordEvilDragon, Stage.EvilDragonsRoost3, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlazeGrigori, 100, 25000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.FlameSkeletonBrute, 100, 5000, 1),
            LibDdon.Enemy.Create(EnemyId.FlameSkeleton, 100, 5000, 2),
            LibDdon.Enemy.Create(EnemyId.FlameSkeleton, 100, 5000, 3),
        });

        AddEnemies(EnemyGroupId.MegadoDarkness, Stage.DarknessShroudedMergodaRoyalPalaceLevel1, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Abaddon0, 100, 120000, 0)
                .SetIsBoss(true),
        });

        AddEnemies(EnemyGroupId.VortexOfStagnation, Stage.DarknessShroudedMergodaRoyalPalaceLevel1, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackSword1, 100, 120000, 0)
                .SetIsBoss(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.TheWhiteDragon, 30287)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8348);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.LookoutCastle1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8349);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.LookoutCastle1, 0, 0, NpcId.Nedo0, 30288)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8659);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.MegadosysPlateau);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.MegadosysPlateau, 0, 0, NpcId.Quintus, 31187)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8350)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8354);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.BeforetheSecretSpring);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.BeforetheSecretSpring, -3, 0, -1036);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.BlackSwordSecretSpring);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.BeforetheSecretSpring, 0, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.BeforetheSecretSpring, 1, 0, NpcId.Bertha, 30289)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8356)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8360);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.DacreimFortress4);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.DacreimFortress4, 1, 0, NpcId.Meirova0, 30290);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.BlackSwordDacreim);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.DacreimFortress4, 0, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8361);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.DacreimFortress4, 2, 0, NpcId.Meirova0, 30291)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8368)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8364);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.EvilDragonsRoost3);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.EvilDragonsRoost3, 0, 0, NpcId.Gillian0, 30914);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.BlackSwordEvilDragon);
        process0.AddIsBrokenLayoutBlock(QuestAnnounceType.Update, Stage.EvilDragonsRoost3, 2, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8369);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.EvilDragonsRoost3, 1, 0, NpcId.Gillian0, 30291);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.FortressCityMegadoResidentialLevel1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8487);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheWhiteDragonTemple0, NpcId.Travers1, 31060);
        // Crystal War EXM is not implemented; auto-advance the clear objective.
        process0.AddDelayBlock(QuestAnnounceType.Update, 0, 5);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheWhiteDragonTemple0, NpcId.Travers1, 31062)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8377);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.SouthLandofDarkness);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.SouthLandofDarkness, 0, 0, NpcId.Gurdolin3, 31062)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8378);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.DarknessShroudedMergodaRoyalPalaceLevel1);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.DarknessShroudedMergodaRoyalPalaceLevel1, 1, 4850, -10340);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MegadoDarkness)
            .AddResultCmdSetDiePlayerReturnPos(Stage.DarknessShroudedMergodaRoyalPalaceLevel1, 6, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.DarknessShroudedMergodaRoyalPalaceLevel1, 0, 0, NpcId.Gurdolin3, 30292);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.DarknessShroudedMergodaRoyalPalaceLevel1, 3, 0, OmQuestType.MyQuest, OmInteractType.Release);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.VortexOfStagnation)
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8379);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.DarknessShroudedMergodaRoyalPalaceLevel1, 1, 0, NpcId.Nedo0, 30294);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.Joseph, 30295);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
