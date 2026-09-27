/**
 * @brief Attack on the Royal Capital
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.AttackOnTheRoyalCapital;
    public override ushort RecommendedLevel => 90;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.AnOmenOfDestruction;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint Occupying = 2306;
        public const uint Obstructing = 2307;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(90));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.RallyTheTroops));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.FortressCityMegadoResidentialLevel0, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 90, 4200, 8)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 90, 4200, 9)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 90, 4200, 10)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 90, 4200, 11)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 90, 4200, 12)
                .SetNamedEnemyParams(NamedParamId.Occupying),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.FortressCityMegadoResidentialLevel0, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 90, 4200, 0)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 90, 4200, 1)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 90, 4200, 2)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 90, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 90, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 90, 4200, 5)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 90, 4200, 6)
                .SetNamedEnemyParams(NamedParamId.Occupying),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.FortressCityMegadoResidentialLevel0, 50, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 90, 4200, 5)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 90, 4200, 6)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 90, 4200, 7)
                .SetNamedEnemyParams(NamedParamId.Occupying),
        });

        AddEnemies(EnemyGroupId.Encounter + 3, Stage.FortressCityMegadoResidentialLevel0, 53, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 90, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 90, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 90, 4200, 5)
                .SetNamedEnemyParams(NamedParamId.Occupying),
        });

        AddEnemies(EnemyGroupId.Encounter + 4, Stage.FortressCityMegadoResidentialLevel0, 55, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 90, 4200, 6)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 90, 4200, 7)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 90, 4200, 8)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.RangedSoldierDwarfOrc, 90, 4200, 9)
                .SetNamedEnemyParams(NamedParamId.Occupying),
        });

        AddEnemies(EnemyGroupId.Encounter + 5, Stage.FortressCityMegadoResidentialLevel0, 57, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 90, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 90, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.Occupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 90, 4200, 5)
                .SetNamedEnemyParams(NamedParamId.Occupying),
        });

        AddEnemies(EnemyGroupId.Encounter + 6, Stage.FortressCityMegadoResidentialLevel0, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyGoremanticoreLightArmor, 90, 21000, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.Obstructing),
        });

        AddEnemies(EnemyGroupId.Encounter + 7, Stage.FortressCityMegadoResidentialLevel0, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 90, 4200, 8)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.Occupying)
                .SetRepopConditions(50, 10),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 90, 4200, 9)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.Occupying)
                .SetRepopConditions(50, 10),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 90, 4200, 10)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.Occupying)
                .SetRepopConditions(50, 10),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21950)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.MegadosysPlateau, 0, 5, NpcId.Meirova0, 21951)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7269)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7270)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 25640)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 25641)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25642)
            .AddResultCmdQstTalkChg(NpcId.Yuri, 25643)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25644)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25645)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25646)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25647)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25648);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadosysPlateau, 0, 3, NpcId.Nedo0, 21952)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25649)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25650)
            .AddResultCmdQstTalkChg(NpcId.Yuri, 25651)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25652)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25653)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25654)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25655);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.HiddenWaterwaytoMegado)
            .AddWorldManageUnlock(QuestFlags.MegadosysPlateau.FieldAreaMarkers2)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7277)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25656)
            .AddResultCmdQstTalkChg(NpcId.Sly, 25658);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.HiddenWaterwaytoMegado, 0, 0, NpcId.Bertha, 25657)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7746, QuestId.Q70032001) // Ladder to residental level 1
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7512, QuestId.Q70032001) // Part of the default world state: Invisible barrier
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7270)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7278)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7513)
            .AddResultCmdQstTalkChg(NpcId.Cyril, 25659);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortressCityMegadoResidentialLevel0)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7514, QuestId.Q70032001); // Warp to residential level 0
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7480)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7278)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7513)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7269)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7439, QuestId.Q70032001) // Door to megadosys plateau
            .AddCheckCmdMyQstFlagOn(0)
            .AddCheckCmdMyQstFlagOn(1)
            .AddCheckCmdMyQstFlagOn(2)
            .AddCheckCmdMyQstFlagOn(3)
            .AddCheckCmdMyQstFlagOn(4)
            .AddCheckCmdMyQstFlagOn(5);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.FortressCityMegadoResidentialLevel0, 15, 1400, -20387);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FortressCityMegadoResidentialLevel0, 0, 11);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 6)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 16)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7480);
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.HiddenWaterwaytoMegado)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 16);
        process0.AddIsStageNoBlock(QuestAnnounceType.Checkpoint, Stage.EliGuardTower);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.EliGuardTower, 0, 4, NpcId.Fabio0, 21971)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25666)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25667)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25668)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25669)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7016);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, -11938, 11898, -13)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25670)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25671)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25672)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25673);
        process0.AddEventAfterJumpBlock(QuestAnnounceType.None, Stage.AudienceChamber, 210, 6);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.TheWhiteDragon, 21994)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7291)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheWhiteDragon7)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheWhiteDragon5)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25674)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25675)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25676)
            .AddResultCmdQstTalkChg(NpcId.Fabio0, 25677)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 25678)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 25679);
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMyquestLayoutFlagOn(7480);
        process1.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 0);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdMyQstFlagOn(0);

        var process2 = AddNewProcess(2);
        process2.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMyquestLayoutFlagOn(7480);
        process2.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1);
        process2.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdMyQstFlagOn(1);

        var process3 = AddNewProcess(3);
        process3.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMyquestLayoutFlagOn(7480);
        process3.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 2);
        process3.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdMyQstFlagOn(2);

        var process4 = AddNewProcess(4);
        process4.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMyquestLayoutFlagOn(7480);
        process4.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 3);
        process4.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdMyQstFlagOn(3);

        var process5 = AddNewProcess(5);
        process5.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMyquestLayoutFlagOn(7480);
        process5.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 4);
        process5.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdMyQstFlagOn(4);

        var process6 = AddNewProcess(6);
        process6.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMyquestLayoutFlagOn(7480);
        process6.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 5);
        process6.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdMyQstFlagOn(5);

        var process7 = AddNewProcess(7);
        process7.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMyquestLayoutFlagOn(16);
        process7.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 7)
            .AddCheckCmdIsMyquestLayoutFlagOff(16);
        process7.AddRemoveGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 7);
    }
}

return new ScriptedQuest();
