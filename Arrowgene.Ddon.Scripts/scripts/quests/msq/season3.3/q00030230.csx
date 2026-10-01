/**
 * @brief The Royal Family Mausoleum
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheRoyalFamilyMausoleum;
    public override ushort RecommendedLevel => 95;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheDreadfulPassage;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(95));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.NedosTrail));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 850000);
        AddWalletReward(WalletType.Gold, 95000);
        AddWalletReward(WalletType.RiftPoints, 9500);

        AddFixedItemReward(ItemId.RoyalCrestMedalUrtecaDistrict, 5);
        AddFixedItemReward(ItemId.UnappraisedCloudTrinketGeneral, 2);
        AddFixedItemReward(ItemId.ApUrtecaMountains, 50);
    }

    protected override void InitializeEnemyGroups()
    {
        // Orc commander seeking revenge for Lookout Castle
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.UrtecaMountains, 14, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.CaptainOrc0, 95, 22000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 95, 4400, 1),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 95, 4400, 2),
            LibDdon.Enemy.Create(EnemyId.SwordSoldierDwarfOrc, 95, 4400, 3),
        });

        AddEnemies(EnemyGroupId.Encounter + 1, Stage.TheRoyalFamilyMausoleum, 2, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarMaster1, 95, 22000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.BeastMaster1, 95, 22000, 1)
                .SetIsBoss(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 22409);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.LookoutCastle1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7839)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7881)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7882)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 25972)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25975)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25977)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25976)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25979)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25978);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 1, 0, NpcId.Bertha, 22410)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7841)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25980)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25981)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25982)
            .AddResultCmdQstTalkChg(NpcId.Ashe, 27840)
            .AddResultCmdQstTalkChg(NpcId.Sly, 27841);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, -92526, 61017, -365473)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25983);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.UrtecaMountains, 1, 9);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddResultCmdPlayMessage(30190, 5);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.UrtecaMountains, 2, 9)
            .AddResultCmdStopMessage();
        process0.AddIsStageNoBlock(QuestAnnounceType.Update, Stage.TheRoyalFamilyMausoleum)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7841);
        process0.AddPartyGatherBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheRoyalFamilyMausoleum, 801, 1298, -10146);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 1);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.TheRoyalFamilyMausoleum, -20, 2336, -16800);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.TheRoyalFamilyMausoleum, 0, 1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7842)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8027)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25986)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25984)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25985);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.TheRoyalFamilyMausoleum, 0, 0, NpcId.Meirova0, 22451)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25987)
            .AddResultCmdQstTalkChg(NpcId.Nedo0, 25988)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25994)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25995)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25996)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25989);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, 0, 0, NpcId.Meirova0, 22452)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7842)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8027)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25997)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25999);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.TheWhiteDragon, 22453)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7839)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7881)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7882);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
