/**
 * @brief The Relics of the First King
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheRelicsOfTheFirstKing;
    public override ushort RecommendedLevel => 97;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.HopesBitterEnd;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(97));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheDreadfulPassage));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 880000);
        AddWalletReward(WalletType.Gold, 98000);
        AddWalletReward(WalletType.RiftPoints, 9800);

        AddFixedItemReward(ItemId.RoyalCrestMedalUrtecaDistrict, 5);
        AddFixedItemReward(ItemId.UnappraisedCloudTrinketGeneral, 2);
        AddFixedItemReward(ItemId.ApUrtecaMountains, 50);
    }

    protected override void InitializeEnemyGroups()
    {
        // Guardians of Samara's ring
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.CrumblingEntrancePath, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SkeletonCyclops, 97, 115000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.Wight0, 97, 4800, 1),
            LibDdon.Enemy.Create(EnemyId.Wight0, 97, 4800, 2),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 27822)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7890);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.FirefallMountainCampsite)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 27843)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 27844);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.None, Stage.FirefallMountainCampsite, 0, 0, NpcId.Cyril, 27845)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7852)
            .AddResultCmdQstTalkChg(NpcId.Cyril, 27846)
            .AddResultCmdQstTalkChg(NpcId.Bacias, 30104);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.FortressCityMegadoResidentialLevel1, 0, 0, NpcId.Gillian0, 27847)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7853)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 27848);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.FortressCityMegadoResidentialLevel1, 1, 0, OmQuestType.MyQuest, OmInteractType.EndText)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8039)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7855)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8040)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 27852);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.MegadosysPlateau, 1, 0, OmQuestType.MyQuest, OmInteractType.EndText)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8039)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8040)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7857)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8041);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CrumblingEntrancePath);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.CrumblingEntrancePath, -2106, 7, -35414);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0);
        process0.AddOmInteractEventBlock(QuestAnnounceType.Update, Stage.CrumblingEntrancePath, 0, 0, OmQuestType.MyQuest, OmInteractType.EndText)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 28301);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FirefallMountainCampsite, 0, 0, NpcId.Cyril, 27853)
            .AddResultCmdQstTalkChg(NpcId.Cyril, 28309)
            .AddResultCmdQstTalkChg(NpcId.Bacias, 30105);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.Joseph, 27854)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7852)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7853)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7855)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7857)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7890)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8041)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 27855);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
