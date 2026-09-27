/**
 * @brief A Brief Dragon Force
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.ABriefDragonForce;
    public override ushort RecommendedLevel => 92;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.ThePlightOfLookoutCastle;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    private class NamedParamId
    {
        public const uint Awoken = 2311;
        public const uint OldSpiritDragonGuardBeast1 = 2312;
        public const uint OldSpiritDragonGuardBeast2 = 2313;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(92));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.AnOmenOfDestruction));
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
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.TarieSmallTower, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SeverelyInfectedPixie, 92, 4200, 0)
                .SetNamedEnemyParams(NamedParamId.Awoken),
            LibDdon.Enemy.Create(EnemyId.SeverelyInfectedPixie, 92, 4200, 1)
                .SetNamedEnemyParams(NamedParamId.Awoken),
            LibDdon.Enemy.Create(EnemyId.SeverelyInfectedPixie, 92, 4200, 2)
                .SetNamedEnemyParams(NamedParamId.Awoken),
            LibDdon.Enemy.Create(EnemyId.SeverelyInfectedDemon, 92, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.OldSpiritDragonGuardBeast1),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.SageTowerRuins, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.LotusFin, 92, 105000, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.OldSpiritDragonGuardBeast2),
            LibDdon.Enemy.Create(EnemyId.SeverelyInfectedStymphalides, 92, 4200, 1)
                .SetNamedEnemyParams(NamedParamId.Awoken),
            LibDdon.Enemy.Create(EnemyId.SeverelyInfectedStymphalides, 92, 4200, 2)
                .SetNamedEnemyParams(NamedParamId.Awoken),
            LibDdon.Enemy.Create(EnemyId.SeverelyInfectedStymphalides, 92, 4200, 3)
                .SetNamedEnemyParams(NamedParamId.Awoken),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 22073)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheWhiteDragon7)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheWhiteDragon5);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.HollowofBeginnings1, 0, 0, NpcId.Gearoid0, 22074)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 25774)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 25775)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 25776)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25777)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25778)
            .AddResultCmdQstTalkChg(NpcId.Mayleaf0, 25779)
            .AddResultCmdQstTalkChg(NpcId.Pamela, 25780)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7093)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7094)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.HollowOfBeginnings.SpiritDragon)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.HollowOfBeginnings.Mordred);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.SpiritArtsHut, NpcId.AdairDonnchadh0, 27265)
            .AddResultCmdQstTalkChg(NpcId.Gearoid0, 25781)
            .AddResultCmdQstTalkChg(NpcId.Mordred0, 27264);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TarieSmallTower)
            .AddWorldManageUnlock(QuestFlags.FaranaPlains.TarieSmallTower)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7095)
            .AddResultCmdQstTalkChg(NpcId.Blair, 27266);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0, resetGroup: false);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.FaranaPlains0, 1, 0, OmQuestType.MyQuest, OmInteractType.Release)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7099);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.HollowofBeginnings1, 0, 0, NpcId.Gearoid0, 22075)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7350)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7093)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7099)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25784)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25785)
            .AddResultCmdQstTalkChg(NpcId.Blair, 27267)
            .AddResultCmdQstTalkChg(NpcId.AdairDonnchadh0, 27268)
            .AddResultCmdQstTalkChg(NpcId.Mordred0, 27270);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ExpeditionGarrison, NpcId.Bertrand, 27271)
            .AddResultCmdQstTalkChg(NpcId.Gearoid0, 27269);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.SageTowerRuins)
            .AddWorldManageUnlock(QuestFlags.BloodbaneIsle.SageTowerRuins)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7096)
            .AddResultCmdQstTalkChg(NpcId.ArisenCorpsRegimentalSoldier12, 27272);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1, resetGroup: false);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.BloodbaneIsle0, 1, 0, OmQuestType.MyQuest, OmInteractType.Release)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7100);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.HollowofBeginnings1, 0, 0, NpcId.Gearoid0, 25783)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7100)
            .AddResultCmdQstTalkChg(NpcId.ArisenCorpsRegimentalSoldier12, 27273)
            .AddResultCmdQstTalkChg(NpcId.Bertrand, 27274)
            .AddResultCmdQstTalkChg(NpcId.Mordred0, 25782);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.HollowofBeginnings1, 0, 1600, -22020)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 27275)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 27276);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.HollowofBeginnings1, 35, 7);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.HollowofBeginnings1, 0, 0, NpcId.Gearoid0, 22208)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7350)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7892)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25787)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25788)
            .AddResultCmdQstTalkChg(NpcId.Mordred0, 25789);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 22209)
            .AddResultCmdQstTalkChg(NpcId.Gearoid0, 25786)
            .AddResultCmdQstTalkChg(NpcId.Klaus0, 25790)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 25791);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
