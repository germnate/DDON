/**
 * @brief The Final Battle Of The Royal Capital
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheFinalBattleOfTheRoyalCapital;
    public override ushort RecommendedLevel => 94;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.None;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint Obstructing = 2316;
        public const uint BlackWarriorsTamed = 2317;
        public const uint CastleOccupying = 2318;
        public const uint WhiteDragonBlackKnight = 2319;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(94));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.ThePlightOfLookoutCastle));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.MegadoCorridor0, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyOgreLightArmor, 94, 21000, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.CastleOccupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 94, 4200, 1)
                .SetNamedEnemyParams(NamedParamId.CastleOccupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 94, 4200, 2)
                .SetNamedEnemyParams(NamedParamId.CastleOccupying),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 94, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.CastleOccupying),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.MegadoCorridor0, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyGorecyclopsLightArmor0, 94, 21000, 0)
                .SetIsBoss(true)
                .SetInfectionType(3)
                .SetNamedEnemyParams(NamedParamId.Obstructing),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.FortressCityMegadoRoyalPalaceLevel, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Goremanticore, 94, 105000, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.BlackWarriorsTamed),
            LibDdon.Enemy.Create(EnemyId.Grigori, 94, 4200, 1)
                .SetNamedEnemyParams(NamedParamId.CastleOccupying),
            LibDdon.Enemy.Create(EnemyId.Grigori, 94, 4200, 2)
                .SetNamedEnemyParams(NamedParamId.CastleOccupying),
            LibDdon.Enemy.Create(EnemyId.BeardedGrigori, 94, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.CastleOccupying),
            LibDdon.Enemy.Create(EnemyId.BeardedGrigori, 94, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.CastleOccupying),
        });

        AddEnemies(EnemyGroupId.Encounter + 3, Stage.BlackKingsRoom0, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlackKnightHolyIce0, 94, 105000, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.WhiteDragonBlackKnight),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 22230)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheWhiteDragon7)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheWhiteDragon5);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.LookoutCastle1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7114)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 25897)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 25898)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 25899)
            .AddResultCmdQstTalkChg(NpcId.Gerd1, 25900)
            .AddResultCmdQstTalkChg(NpcId.Fabio0, 25901)
            .AddResultCmdQstTalkChg(NpcId.Mayleaf0, 25902)
            .AddResultCmdQstTalkChg(NpcId.Pamela, 25903);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 0, 0, NpcId.Meirova0, 22231)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7115)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25905)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25906)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25907)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25908);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortressCityMegadoResidentialLevel0)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25904)
            .AddWorldManageUnlock(QuestFlags.MegadosysPlateau.FieldAreaMarkers3)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7439, QuestId.Q70032001) // Door to megado city 0
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7440, QuestId.Q70032001); // Door to megado city 1
        process0.AddPartyGatherBlock(QuestAnnounceType.None, Stage.FortressCityMegadoResidentialLevel0, 0, 2400, -25782)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7115)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7116)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25909)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25910)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier0, 25911)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier1, 25912)
            .AddResultCmdQstTalkChg(NpcId.LiberationArmySoldier2, 25913);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FortressCityMegadoResidentialLevel0, 5, 12);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortressCityMegadoResidentialLevel0, 1, 0, NpcId.Gillian0, 22260)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7116)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7117)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25916)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25917)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25918)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25919);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadoCorridor0)
            .AddQuestFlag(QuestFlagType.WorldManageQuest, QuestFlagAction.Set, 4149, QuestId.Q70032001) // Opens gates
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25915);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7545, QuestId.Q70032001) // Door to megado city 1
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7544, QuestId.Q70032001) // Door to megado city 0
            // Move these flags eventually to a world manage file as they're part of the default world state
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7549, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7551, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7553, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7555, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7557, QuestId.Q70032001)
            .AddCheckCmdOmEndAnimation(Stage.MegadoCorridor0, 24, 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 1);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.MegadoCorridor0, 0, 0, NpcId.Meirova0, 25920)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7119);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortressCityMegadoRoyalPalaceLevel)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7549, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7551, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7553, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7555, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7557, QuestId.Q70032001);
        process0.AddPartyGatherBlock(QuestAnnounceType.None, Stage.FortressCityMegadoRoyalPalaceLevel, 28, 4400, -295)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7562, QuestId.Q70032001); // Door to inner palace
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FortressCityMegadoRoyalPalaceLevel, 0, 5);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2)
            .AddResultCmdSetDiePlayerReturnPos(Stage.FortressCityMegadoRoyalPalaceLevel, 5, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7309)
            .AddResultCmdPlayMessage(22275, 5)
            .AddResultCmdPlayMessage(22281, 5)
            .AddResultCmdPlayMessage(22287, 5)
            .AddResultCmdPlayMessage(22293, 5)
            .AddResultCmdPlayMessage(22299, 5)
            .AddResultCmdPlayMessage(22276, 5)
            .AddResultCmdPlayMessage(22282, 5)
            .AddResultCmdPlayMessage(22301, 5)
            .AddResultCmdPlayMessage(22288, 5)
            .AddResultCmdPlayMessage(22295, 5)
            .AddResultCmdPlayMessage(22277, 5)
            .AddResultCmdPlayMessage(22283, 5)
            .AddResultCmdPlayMessage(22296, 5)
            .AddResultCmdPlayMessage(22290, 5)
            .AddResultCmdPlayMessage(22302, 5)
            .AddResultCmdPlayMessage(22279, 5)
            .AddResultCmdPlayMessage(22289, 5)
            .AddResultCmdPlayMessage(22286, 5)
            .AddResultCmdPlayMessage(22300, 5)
            .AddResultCmdPlayMessage(22294, 5)
            .AddResultCmdPlayMessage(22278, 5)
            .AddResultCmdPlayMessage(22284, 5)
            .AddResultCmdPlayMessage(22291, 5)
            .AddResultCmdPlayMessage(22303, 5)
            .AddResultCmdPlayMessage(22298, 5)
            .AddResultCmdPlayMessage(22280, 5)
            .AddResultCmdPlayMessage(22285, 5)
            .AddResultCmdPlayMessage(22292, 5)
            .AddResultCmdPlayMessage(22297, 5)
            .AddResultCmdPlayMessage(22304, 5)
            .AddResultCmdPlayMessage(22289, 5);
        process0.AddEventExecContBlock(QuestAnnounceType.None, Stage.FortressCityMegadoRoyalPalaceLevel, 5)
            .AddResultCmdStopMessage();
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.BlackKingsRoom0, 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 3)
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0)
            .AddResultCmdSetDiePlayerReturnPos(Stage.BlackKingsRoom0, 1, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7309)
            .AddResultCmdPlayMessage(22332, 5)
            .AddResultCmdPlayMessage(22333, 5)
            .AddResultCmdPlayMessage(22334, 5)
            .AddResultCmdPlayMessage(22335, 5)
            .AddResultCmdPlayMessage(22336, 5)
            .AddResultCmdPlayMessage(22337, 5)
            .AddResultCmdPlayMessage(22338, 5)
            .AddResultCmdPlayMessage(22339, 5)
            .AddResultCmdPlayMessage(22340, 5)
            .AddResultCmdPlayMessage(22341, 5)
            .AddResultCmdPlayMessage(22342, 5)
            .AddResultCmdPlayMessage(22343, 5)
            .AddResultCmdPlayMessage(22344, 5);
        process0.AddEventExecContBlock(QuestAnnounceType.None, Stage.BlackKingsRoom0, 0)
            .AddResultCmdStopMessage();
        process0.AddEventAfterJumpBlock(QuestAnnounceType.None, Stage.FortressCityMegadoRoyalPalaceLevel, 10, 6);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber)
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7562, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7122)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25921)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25922)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25923)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25924)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25925)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25926)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25927);
        process0.AddPartyGatherBlock(QuestAnnounceType.None, Stage.AudienceChamber, -222, 9164, -17)
            .AddResultCmdQstTalkChg(NpcId.Pamela, 25928)
            .AddResultCmdQstTalkChg(NpcId.Fabio0, 25929)
            .AddResultCmdQstTalkChg(NpcId.Mayleaf0, 25930)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 25931)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 25932)
            .AddResultCmdQstTalkChg(NpcId.Gerd1, 25933)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 25934);
        process0.AddEventExecContBlock(QuestAnnounceType.None, Stage.AudienceChamber, 220);
        process0.AddEventAfterJumpContinueBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 17, 10);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.AudienceChamber, 6);
        process0.AddProcessEndBlock(true)
            .AddWorldManageUnlock(QuestFlags.FortressCityMegadoResidentialLevel.MegadoCorridor)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheWhiteDragon5)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheWhiteDragon7)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheCrewEndSeason32)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7439, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Clear, 7544, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7440, QuestId.Q70032001)
            .AddQuestFlag(QuestFlagType.WorldManageLayout, QuestFlagAction.Set, 7545, QuestId.Q70032001);
    }
}

return new ScriptedQuest();
