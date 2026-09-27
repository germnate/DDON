/**
 * @brief The Road to the Royal Capital
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheRoadToTheRoyalCapital;
    public override ushort RecommendedLevel => 89;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.RallyTheTroops;

    private class GeneralAnnouncement
    {
        public const int AreaMasterUnlocked = 100576;
    }

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint TowerOccupying = 2302;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(89));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheBattleOfLookoutCastle));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.MegadosysPlateau, 22, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 89, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 89, 4200, 5)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 89, 4200, 6)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 89, 4200, 7)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 89, 4200, 8)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 89, 4200, 9)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.MegadosysPlateau, 22, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 89, 4200, 10)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 89, 4200, 11)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 89, 4200, 12)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 89, 4200, 13)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 89, 4200, 14)
                .SetNamedEnemyParams(NamedParamId.TowerOccupying),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.MegadosysPlateau, 1, QuestEnemyPlacementType.Manual, new()
        {
            /* prevent enemies from spawning */
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTouchAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 0)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.AudienceChamber, 205, 6);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.LookoutCastle1, 0, 1, NpcId.Meirova0, 21893)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7002)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7655)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 23964)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 23965)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 23966)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 23967);
        process0.AddSceHitInBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, 1)
            .AddWorldManageUnlock(QuestFlags.LookoutCastle.HarborDoor)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 23968)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 23969)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 23970);
        process0.AddPartyGatherBlock(QuestAnnounceType.None, Stage.LookoutCastle1, -9081, -84, -8873)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.LookoutCastle.MegadosysPlateauWarp)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7214)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 23971)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 23972)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 23973)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 23974)
            .AddResultCmdQstTalkChg(NpcId.Lotta, 27240);
        process0.AddEventExecContBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 0);
        process0.AddEventAfterJumpBlock(QuestAnnounceType.None, Stage.MegadosysPlateau, 0, 13);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadosysPlateau, 0, 0, NpcId.Gillian0, 21912)
            .AddWorldManageUnlock(QuestFlags.MegadosysPlateau.FieldAreaMarkers1)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.LookoutCastle.MegadosysPlateauWarp)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.MegadosysPlateau.EliGuardTowerWellOpen)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.MegadosysPlateau.EliGuardTowerWellClosed) // Part of the default world state
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7214)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7215)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 23975)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 23976)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 23977);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 23978)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 23979)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 23980)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 23981);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadosysPlateau, 1, 0, NpcId.Gillian0, 21913)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7244)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7215)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 23982)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 23983);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddCheckCmdIsReleaseWarpPointAnyone(82);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadosysPlateau, 1, 0, NpcId.Gillian0, 21914)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 23984)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 23985);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.MegadosysPlateau, 77652, 21505, -253208);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.MegadosysPlateau, 5, 12);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadosysPlateau, 2, 0, NpcId.Nedo0, 21932)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7243)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7244)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 23986)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 23987)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 23988)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 23989)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 23990)
            .AddResultCmdQstTalkChg(NpcId.Yuri, 27242)
            .AddResultCmdQstTalkChg(NpcId.Ross, 27243);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.EliGuardTower, NpcId.Doris, 27244)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.EliGuardTower.Doris)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.MegadosysPlateau.EliGuardTowerWellClosed)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.MegadosysPlateau.EliGuardTowerWellOpen)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 23991)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 23992);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.TheWhiteDragon, 21933)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncement.AreaMasterUnlocked)
            .AddWorldManageUnlock(QuestFlags.NpcFunctions.MegadosysPlateauAreaInfo)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 23993)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 23994)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 23995)
            .AddResultCmdQstTalkChg(NpcId.Doris, 27245)
            .AddResultCmdQstTalkChg(NpcId.Lucas, 27246)
            .AddResultCmdQstTalkChg(NpcId.Umit, 27247);
        process0.AddProcessEndBlock(true)
            .AddResultCmdReleaseAnnounce(ContentsRelease.MegadosysPlateauWorldQuests);
    }
}

return new ScriptedQuest();
