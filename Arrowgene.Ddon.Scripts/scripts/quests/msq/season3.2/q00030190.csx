/**
 * @brief The Plight of Lookout Castle
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.ThePlightOfLookoutCastle;
    public override ushort RecommendedLevel => 92;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheFinalBattleOfTheRoyalCapital;

    private class EnemyGroupId
    {
        public const uint Empty = 0;
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint Raid = 2314;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(92));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.ABriefDragonForce));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 800000);
        AddWalletReward(WalletType.Gold, 90000);
        AddWalletReward(WalletType.RiftPoints, 9000);

        AddFixedItemReward(ItemId.RoyalCrestMedalMegadosysDistrict, 5);
        AddFixedItemReward(ItemId.UnappraisedWaterTrinketGeneral, 2);
        AddFixedItemReward(ItemId.ApMegadosysPlateau, 500);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.LookoutCastle2, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 92, 4200, 0)
                .SetNamedEnemyParams(NamedParamId.Raid),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 92, 4200, 1)
                .SetNamedEnemyParams(NamedParamId.Raid),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 92, 4200, 2)
                .SetNamedEnemyParams(NamedParamId.Raid),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 92, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.Raid),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 92, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.Raid),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.LookoutCastle2, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 92, 4200, 0)
                .SetNamedEnemyParams(NamedParamId.Raid),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 92, 4200, 1)
                .SetNamedEnemyParams(NamedParamId.Raid),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 92, 4200, 2)
                .SetNamedEnemyParams(NamedParamId.Raid),
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 92, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.Raid),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 92, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.Raid),
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 92, 4200, 5)
                .SetNamedEnemyParams(NamedParamId.Raid),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.LookoutCastle2, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarMaster0, 92, 105000, 0)
                .SetIsBoss(true),
        });

        AddEnemies(EnemyGroupId.Encounter + 10, Stage.LookoutCastle2, 10, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 92, 4200, 0)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 92, 4200, 1)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 92, 4200, 2)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 92, 4200, 3)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 92, 4200, 4)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.Encounter + 11, Stage.LookoutCastle2, 10, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyOgreLightArmor, 92, 21000, 5)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.WarReadySaurianLightArmor, 92, 4200, 6)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.WarReadySaurianLightArmor, 92, 4200, 7)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.WarReadySaurianLightArmor, 92, 4200, 8)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.WarReadySaurianLightArmor, 92, 4200, 9)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.WarReadySaurianLightArmor, 92, 4200, 10)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.WarReadySaurianLightArmor, 92, 4200, 11)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.Encounter + 12, Stage.LookoutCastle2, 10, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 92, 4200, 12)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 92, 4200, 13)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 92, 4200, 14)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 92, 4200, 15)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 92, 4200, 16)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.Encounter + 13, Stage.LookoutCastle2, 10, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 92, 4200, 17)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 92, 4200, 20)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.Encounter + 14, Stage.LookoutCastle2, 11, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyNightmareLightArmor, 92, 21000, 0)
                .SetIsManualSet(true)
                .SetStartThinkTblNo(9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        });

        AddEnemies(EnemyGroupId.Empty, Stage.MegadosysPlateau, 15, QuestEnemyPlacementType.Manual, new()
        {

        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 22210)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheWhiteDragon7)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheWhiteDragon5);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.MegadosysPlateau, 0, 0, NpcId.LiberationArmySoldier3, 27337)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7102)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7104)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 25833)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 25834)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 25835)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25836)
            .AddResultCmdQstTalkChg(NpcId.Mayleaf0, 25837)
            .AddResultCmdQstTalkChg(NpcId.Pamela, 25838)
            .AddResultCmdQstTalkChg(NpcId.Sago, 28164)
            .AddResultCmdQstTalkChg(NpcId.Fred, 28165);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, 22, 18280, -14478)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7102)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7103)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier3, 27338)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25839)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25840)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25841)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25842)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25843)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25844)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25845);
        process0.AddEventExecContBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 10);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.LookoutCastle2, 13);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.LookoutCastle2, 0, 0, NpcId.Nedo0, 25866)
            .AddResultCmdSetDiePlayerReturnPos(Stage.LookoutCastle2, 13, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7103)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7105)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25863)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25864)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25865)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25867)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25868)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25869);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7945)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7106);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.LookoutCastle2, 1, 0, NpcId.Sara, 27340)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, 4020)
            .AddResultCmdQstTalkChg(NpcId.Adem, 27342)
            .AddResultCmdQstTalkChg(NpcId.Dennis, 27343);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.LookoutCastle2, 4, 0, NpcId.LiberationArmySoldier4, 27344)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier5, 27346)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier6, 27347)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7104)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7308);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.LookoutCastle2, -10, 150, 8570)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier4, 27345);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.LookoutCastle2, 0, 3);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2)
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0)
            .AddResultCmdSetDiePlayerReturnPos(Stage.LookoutCastle2, 3, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7109);
        process0.AddEventExecContBlock(QuestAnnounceType.None, Stage.LookoutCastle2, 5)
            .AddResultCmdBgmStop();
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, 1, 1, NpcId.Meirova0, 22239)
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7308)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7113)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25870)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25871)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25872)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25873)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25874)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25875);
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdEmHpLess(Stage.LookoutCastle2, 0, 0, 100);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 10)
            .AddCheckCmdEmHpLess(Stage.LookoutCastle2, 0, 0, 75);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 11)
            .AddCheckCmdEmHpLess(Stage.LookoutCastle2, 0, 0, 50);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 12)
            .AddCheckCmdEmHpLess(Stage.LookoutCastle2, 0, 0, 25);
        process1.AddSpawnGroupsBlock(QuestAnnounceType.None, [EnemyGroupId.Encounter + 13, EnemyGroupId.Encounter + 14]);
        process1.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
