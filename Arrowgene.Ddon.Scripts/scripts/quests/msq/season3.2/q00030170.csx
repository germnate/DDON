/**
 * @brief An Omen of Destruction
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.AnOmenOfDestruction;
    public override ushort RecommendedLevel => 92;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.ABriefDragonForce;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint StagnationDisturbing1 = 2308;
        public const uint ChaosofStagnation = 2309;
        public const uint StagnationDisturbing2 = 2310;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(92));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.AttackOnTheRoyalCapital));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.TempleofPurification, 5, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.MiseryGhost, 92, 4200, 0)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing1),
            LibDdon.Enemy.Create(EnemyId.MiseryGhost, 92, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing1),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.TempleofPurification, 13, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.MiseryGhost, 92, 4200, 0)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing1),
            LibDdon.Enemy.Create(EnemyId.MiseryGhost, 92, 4200, 1)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing1),
            LibDdon.Enemy.Create(EnemyId.MiseryGhost, 92, 4200, 2)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing1),
        });

        AddEnemies(EnemyGroupId.Encounter + 2, Stage.TempleofPurification, 12, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 92, 4200, 2)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing2),
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 92, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing2),
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 92, 4200, 4)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing2),
        });

        AddEnemies(EnemyGroupId.Encounter + 3, Stage.VortexofStagnation0, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.CursedDragon, 92, 105000, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.ChaosofStagnation),
        });

        AddEnemies(EnemyGroupId.Encounter + 4, Stage.VortexofStagnation0, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 92, 4200, 1)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing2),
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 92, 4200, 2)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing2),
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 92, 4200, 3)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing2),
            LibDdon.Enemy.Create(EnemyId.GrudgeGhost, 92, 4200, 4)
                .SetEnemyTargetTypesId(TargetTypesId.Normal)
                .SetNamedEnemyParams(NamedParamId.StagnationDisturbing2),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTouchAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 0)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheWhiteDragon7)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheWhiteDragon5);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.AudienceChamber, 215, 6);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.AudienceChamber, NpcId.Joseph, 22014)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7018)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 25753)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 25754)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 25755)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25756)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25757)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25758)
            .AddResultCmdQstTalkChg(NpcId.Gerd1, 25759)
            .AddResultCmdQstTalkChg(NpcId.Fabio0, 25760)
            .AddResultCmdQstTalkChg(NpcId.Mayleaf0, 25761)
            .AddResultCmdQstTalkChg(NpcId.Pamela, 25762);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TempleofPurification);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.TempleofPurification, 0, 0, OmQuestType.MyQuest, OmInteractType.Release)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7019)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7018);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7019);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.TempleofPurification, 2, 0, OmQuestType.MyQuest, OmInteractType.Release)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7024);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7024);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.TempleofPurification, 3, 0, OmQuestType.MyQuest, OmInteractType.Release)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7025);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 2)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7025);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.TempleofPurification, 4, 0, OmQuestType.MyQuest, OmInteractType.Release)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7293);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.VortexofStagnation0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7293)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7020);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 3)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7294);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 3, resetGroup: false);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.VortexofStagnation0, 80, -302, -15020);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.VortexofStagnation0, 10, 2)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 13);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MayleafsBedroom)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7090)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25763);
        process0.AddPartyGatherBlock(QuestAnnounceType.None, Stage.MayleafsBedroom, -2241, 18, 7)
            .AddResultCmdQstTalkChg(NpcId.Pamela, 25764)
            .AddResultCmdQstTalkChg(NpcId.Mayleaf0, 25765)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25766)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25767)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7089)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7091)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7090);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.MayleafsBedroom, 25, 1);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 22032)
            .AddResultCmdQstTalkChg(NpcId.Mayleaf0, 25768)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25769)
            .AddResultCmdQstTalkChg(NpcId.Pamela, 25770)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 25771)
            .AddResultCmdQstTalkChg(NpcId.Gerd1, 25772)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 25773)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7092)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7091);
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMyquestLayoutFlagOn(13);
        process1.AddSpawnGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 4)
            .AddCheckCmdIsMyquestLayoutFlagOff(13);
        process1.AddRemoveGroupBlock(QuestAnnounceType.None, EnemyGroupId.Encounter + 4);
    }
}

return new ScriptedQuest();
