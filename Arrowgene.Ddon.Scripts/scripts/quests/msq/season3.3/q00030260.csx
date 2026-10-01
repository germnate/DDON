/**
 * @brief Hope's Bitter End
 *
 * Ported from the legacy Mq030260_HopesBitterEnd class (built from a packet capture).
 * The Evil Dragon linkage-flag phase handling and the companion side processes are simplified.
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.HopesBitterEnd;
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.ThoseWhoFollowTheDragon;

    private class EnemyGroupId
    {
        public const uint Harpies = 10;
        public const uint Goremanticore = 11;
        public const uint DwarfOrcs = 12;
        public const uint EvilDragon = 13;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(100));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheRelicsOfTheFirstKing));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 900000);
        AddWalletReward(WalletType.Gold, 100000);
        AddWalletReward(WalletType.RiftPoints, 10000);

        AddFixedItemReward(ItemId.RoyalCrestMedalUrtecaDistrict, 5);
        AddFixedItemReward(ItemId.UnappraisedCloudTrinketGeneral, 2);
        AddFixedItemReward(ItemId.ApUrtecaMountains, 50);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Harpies, Stage.SacredFlamePath0, 17, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlazeHarpy, 100, 5000, 0).SetNamedEnemyParams(2307),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 100, 5000, 1).SetNamedEnemyParams(2307),
            LibDdon.Enemy.Create(EnemyId.BlazeGrigori, 100, 5000, 2).SetNamedEnemyParams(2307),
            LibDdon.Enemy.Create(EnemyId.BlazeGrigori, 100, 5000, 3).SetNamedEnemyParams(2307),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 100, 5000, 4).SetNamedEnemyParams(2307),
        });

        AddEnemies(EnemyGroupId.Goremanticore, Stage.SacredFlamePath0, 18, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyGoremanticoreLightArmor, 100, 25000, 0)
                .SetNamedEnemyParams(2307)
                .SetIsBoss(true),
        });

        AddEnemies(EnemyGroupId.DwarfOrcs, Stage.SacredFlamePath0, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 100, 5000, 0).SetNamedEnemyParams(1757),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 100, 5000, 1).SetNamedEnemyParams(1757),
            LibDdon.Enemy.Create(EnemyId.WarReadySaurianLightArmor, 100, 5000, 2).SetNamedEnemyParams(1757),
        });

        AddEnemies(EnemyGroupId.EvilDragon, Stage.EvilDragonsRoost1, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.TheEvilDragon0, 100, 1000000, 0)
                .SetIsBoss(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.TheWhiteDragon, 22449)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7860) // Lise, Gurdolin and Elliot in the audience chamber
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8013)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7954, QuestId.Q70033001)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 30164)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier3, 30116)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier4, 30117)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 30729)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 30730)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 30731);
        process0.AddPartyGatherBlock(QuestAnnounceType.Accept, Stage.LookoutCastle1, 15, 18280, -14593)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7861)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7891)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8013)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 8036, QuestId.Q70033001)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 26018)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 26019)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 26020)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 26021)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 26022)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 26023)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 26024)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 26025)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 26026);
        process0.AddEventExecContBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 20, Stage.LookoutCastle1, 32);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, 0, 1, NpcId.Meirova0, 0x57f2)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 0x1eb6)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 0x1eb4)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 0x65ac)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 0x65ad)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 0x65ae)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 0x65af)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 0x65b0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.FirefallMountainCampsite, 0, 0, NpcId.Meirova0, 26034)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7901)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7967, QuestId.Q70033001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7968, QuestId.Q70033001)
            .AddResultCmdQstTalkChg(NpcId.Cyril, 26027);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.SacredFlamePath0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8059)
            .AddResultCmdQstTalkChg(NpcId.Bacias, 26033);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Harpies)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7862)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8150);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Goremanticore)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8061)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 30732);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.DwarfOrcs)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 30733);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.SacredFlamePathUpperLevel)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7955, QuestId.Q70033001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 8202, QuestId.Q70033001)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8060)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8065)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 30734);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.SacredFlamePathUpperLevel, 21030, 2934, -24797)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7904)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8059)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8060)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8061)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8065)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8150)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7861);
        process0.AddEventAfterJumpContinueBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.EvilDragonsRoost1, 0, 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Accept, EnemyGroupId.EvilDragon)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7868)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7904)
            .AddResultCmdSetDiePlayerReturnPos(Stage.EvilDragonsRoost1, 1, 0)
            .AddResultCmdBgmRequestFix(BgmType.Unknown1, 264);
        process0.AddEventExecBlock(QuestAnnounceType.None, Stage.EvilDragonsRoost1, 5)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7868)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7903)
            .AddResultCmdBgmStop()
            .AddResultCmdBgmRequestFix(BgmType.Unknown1, 272);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.EvilDragonsRoost1, 1, 0, NpcId.Nedo0, 26035)
            .AddResultCmdBgmStop()
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7872)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 26037)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 26039)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 26040)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 26041)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 26042);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.TheWhiteDragon, 22599)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8200)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 26036)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 26038)
            .AddResultCmdQstTalkChg(NpcId.Gerhard, 26043)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 26044)
            .AddResultCmdQstTalkChg(NpcId.Quintus, 26045)
            .AddResultCmdQstTalkChg(NpcId.Bacias, 30106)
            .AddResultCmdQstTalkChg(NpcId.Cyril, 26046)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 26047)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 26048);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
