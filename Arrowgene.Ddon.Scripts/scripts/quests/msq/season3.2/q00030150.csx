/**
 * @brief Rally the Troops
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.RallyTheTroops;
    public override ushort RecommendedLevel => 90;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.AttackOnTheRoyalCapital;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint Pursuit2303 = 2303;
        public const uint Pursuit2304 = 2304;
        public const uint Pursuit2305 = 2305;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(90));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheRoadToTheRoyalCapital));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.MegadosysPlateau, 23, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.AncestorOrc, 90, 4200, 5)
                .SetNamedEnemyParams(NamedParamId.Pursuit2303),
            LibDdon.Enemy.Create(EnemyId.AncestorOrc, 90, 4200, 6)
                .SetNamedEnemyParams(NamedParamId.Pursuit2303),
            LibDdon.Enemy.Create(EnemyId.AncestorOrc, 90, 4200, 7)
                .SetNamedEnemyParams(NamedParamId.Pursuit2303),
            LibDdon.Enemy.Create(EnemyId.AncestorOrc, 90, 4200, 8)
                .SetNamedEnemyParams(NamedParamId.Pursuit2303),
            LibDdon.Enemy.Create(EnemyId.CaptainAncestorOrc, 90, 4200, 9)
                .SetNamedEnemyParams(NamedParamId.Pursuit2303),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.MegadosysPlateau, 40, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GhostMail, 90, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.Pursuit2304),
            LibDdon.Enemy.Create(EnemyId.DarkCorpseTorturer, 90, 4200, 5)
                .SetNamedEnemyParams(NamedParamId.Pursuit2304),
            LibDdon.Enemy.Create(EnemyId.GhostMail, 90, 4200, 6)
                .SetNamedEnemyParams(NamedParamId.Pursuit2304),
            LibDdon.Enemy.Create(EnemyId.DarkCorpseTorturer, 90, 4200, 7)
                .SetNamedEnemyParams(NamedParamId.Pursuit2304),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.MegadosysPlateau, 51, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 90, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.Pursuit2305),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 90, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.Pursuit2305),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 90, 4200, 5)
                .SetNamedEnemyParams(NamedParamId.Pursuit2305),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 90, 4200, 6)
                .SetNamedEnemyParams(NamedParamId.Pursuit2305),
            LibDdon.Enemy.Create(EnemyId.SquadLeaderDwarfOrc, 90, 4200, 7)
                .SetNamedEnemyParams(NamedParamId.Pursuit2305),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 21934)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddPartyGatherBlock(QuestAnnounceType.Accept, Stage.LookoutCastle1, 22, 18280, -14478)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7245)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7257)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 25314)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 25315)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25316)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25317)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25318)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25319)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25320)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25321)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25322);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 5, 25);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, 1, 0, NpcId.Gillian0, 21947)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25323)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25324)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25325)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25326)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25327)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25328);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadosysPlateau, 6, 0, NpcId.Yuri, 28162)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7468)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25329);
        process0.AddSpawnGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 0)
            .AddResultCmdQstTalkChg(NpcId.Yuri, 28163)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7252)
            .AddCheckCmdIsEnemyFoundForOrderRadius(Stage.MegadosysPlateau, 23, -1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadosysPlateau, 1, 0, NpcId.Kirsty0, 27252)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.MegadosysPlateau.Kirsty)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7247)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7252);
        process0.AddSpawnGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 1)
            .AddResultCmdQstTalkChg(NpcId.Eddys, 25336)
            .AddResultCmdQstTalkChg(NpcId.Sago, 25337)
            .AddResultCmdQstTalkChg(NpcId.Kirsty0, 27253)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7253)
            .AddCheckCmdIsEnemyFoundForOrderRadius(Stage.MegadosysPlateau, 40, -1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadosysPlateau, 3, 0, NpcId.Percy, 25343)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7249)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7253);
        process0.AddSpawnGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Encounter + 2)
            .AddResultCmdQstTalkChg(NpcId.Percy, 27254)
            .AddResultCmdQstTalkChg(NpcId.Barry, 25344)
            .AddResultCmdQstTalkChg(NpcId.Ivey, 25335)
            .AddResultCmdQstTalkChg(NpcId.Fred, 25338)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7254)
            .AddCheckCmdIsEnemyFoundForOrderRadius(Stage.MegadosysPlateau, 51, -1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2, resetGroup: false);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadosysPlateau, 5, 0, NpcId.Cyril, 25342)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7251)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7254)
            .AddResultCmdQstTalkChg(NpcId.Karen, 25339)
            .AddResultCmdQstTalkChg(NpcId.Linus, 25340)
            .AddResultCmdQstTalkChg(NpcId.Toyugaru, 25341)
            .AddResultCmdQstTalkChg(NpcId.Esen, 25345);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.EliGuardTower)
            .AddResultCmdQstTalkChg(NpcId.Cyril, 27255);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.EliGuardTower, 0, 0, NpcId.Gillian0, 25348)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7256)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7251)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7257)
            .AddResultCmdQstTalkChg(NpcId.Karen, 27248)
            .AddResultCmdQstTalkChg(NpcId.Linus, 27249)
            .AddResultCmdQstTalkChg(NpcId.Toyugaru, 27250)
            .AddResultCmdQstTalkChg(NpcId.Esen, 27251);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, 0, 1, NpcId.Meirova0, 21948)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25346)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25347)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25349)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25350)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25351);
        process0.AddProcessEndBlock(true)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.MegadosysPlateau.Kirsty);
    }
}

return new ScriptedQuest();
