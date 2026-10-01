/**
 * @brief Nedo's Trail
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.NedosTrail;
    public override ushort RecommendedLevel => 95;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheRoyalFamilyMausoleum;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(95));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheMissingPrince));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 840000);
        AddWalletReward(WalletType.Gold, 94000);
        AddWalletReward(WalletType.RiftPoints, 9400);

        AddFixedItemReward(ItemId.RoyalCrestMedalUrtecaDistrict, 5);
        AddFixedItemReward(ItemId.UnappraisedCloudTrinketGeneral, 2);
        AddFixedItemReward(ItemId.ApUrtecaMountains, 50);
    }

    protected override void InitializeEnemyGroups()
    {
        // Ambush on the bandit Marten
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.UrtecaMountains, 13, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.CaptainOrc0, 95, 22000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 95, 4400, 1),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 95, 4400, 2),
            LibDdon.Enemy.Create(EnemyId.BluntSoldierDwarfOrc, 95, 4400, 3),
        });

        // Ashe's pursuers
        AddEnemies(EnemyGroupId.Encounter + 1, Stage.UrtecaMountains, 17, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Goremanticore, 95, 110000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 95, 4400, 1),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 95, 4400, 2),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 27421);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.LookoutCastle1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7832)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 27422);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 0, 0, NpcId.Meirova0, 27424)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7833)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 27423)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 27425);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, 5, 0, NpcId.Bertha, 27430)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 27431)
            .AddResultCmdQstTalkChg(NpcId.Raven, 27823);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, NpcId.Cyril, 27432)
            .AddResultCmdQstTalkChg(NpcId.Cyril, 27433)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 27434);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.NorthernBanditHideout, NpcId.Bacias, 27435)
            .AddResultCmdQstTalkChg(NpcId.Bacias, 27436);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsReleaseWarpPointAnyone(96)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7847);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, 0, 0, NpcId.Berce, 29895)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7834);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, 1, 0, NpcId.Sly, 27824)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7835)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7836)
            .AddResultCmdQstTalkChg(NpcId.Sly, 28421);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, 2, 0, NpcId.Pieri, 28422);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, 3, 0, NpcId.Marten, 28423);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7836)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8017);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, 6, 0, NpcId.Marten, 28424)
            .AddResultCmdQstTalkChg(NpcId.Marten, 28425);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.UrtecaMountains, -81102, 54409, -383077);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.UrtecaMountains, 5, 21);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7838);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, 4, 1, NpcId.Ashe, 27826)
            .AddResultCmdQstTalkChg(NpcId.Ashe, 27827);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, 4, 0, NpcId.Sly, 27828)
            .AddResultCmdQstTalkChg(NpcId.Sly, 27829);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, NpcId.Cyril, 27831)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7834)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7835)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7838)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7847)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8017)
            .AddResultCmdQstTalkChg(NpcId.Cyril, 27832)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 27830);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, 0, 0, NpcId.Meirova0, 27442)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 27437)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 27441);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.Joseph, 27443)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7832)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7833);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
