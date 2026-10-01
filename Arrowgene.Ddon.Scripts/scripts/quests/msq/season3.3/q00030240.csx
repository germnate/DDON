/**
 * @brief The Dreadful Passage
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheDreadfulPassage;
    public override ushort RecommendedLevel => 95;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheRelicsOfTheFirstKing;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(95));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheRoyalFamilyMausoleum));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 860000);
        AddWalletReward(WalletType.Gold, 96000);
        AddWalletReward(WalletType.RiftPoints, 9600);

        AddFixedItemReward(ItemId.RoyalCrestMedalUrtecaDistrict, 5);
        AddFixedItemReward(ItemId.UnappraisedCloudTrinketGeneral, 2);
        AddFixedItemReward(ItemId.ApUrtecaMountains, 50);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.UrtecaMountains, 23, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Ifrit1stForm, 95, 110000, 0)
                .SetIsBoss(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 22447);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.LookoutCastle1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7848)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 26000)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 26001);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.LookoutCastle1, 0, 0, NpcId.Meirova0, 22448)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7850)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 26004)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 26003);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, 33733, 72263, -379117);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.UrtecaMountains, 10, 8);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddResultCmdSetDiePlayerReturnPos(Stage.UrtecaMountains, 8, 0);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.UrtecaMountains, 0, 0, NpcId.Gillian0, 26005)
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7851)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 26006);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FirefallMountainCampsite)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7850);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.FirefallMountainCampsite, 0, 0, NpcId.Gillian0, 26011)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 26007)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 26008)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 26009)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 26013);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddCheckCmdIsReleaseWarpPointAnyone(97);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.FirefallMountainCampsite, 0, 2, NpcId.Gurdolin3, 22471)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 26014)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 26012)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 26015)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 27842);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Joseph, 22472)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7848)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7851)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 26016)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 26017);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
