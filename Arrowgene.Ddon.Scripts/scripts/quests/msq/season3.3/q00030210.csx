/**
 * @brief The Missing Prince
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheMissingPrince;
    public override ushort RecommendedLevel => 95;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.NedosTrail;

    private class EnemyGroupId
    {
        public const uint Encounter = 10;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(95));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheFinalBattleOfTheRoyalCapital));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 820000);
        AddWalletReward(WalletType.Gold, 92000);
        AddWalletReward(WalletType.RiftPoints, 9200);

        AddFixedItemReward(ItemId.RoyalCrestMedalUrtecaDistrict, 5);
        AddFixedItemReward(ItemId.UnappraisedCloudTrinketGeneral, 2);
        AddFixedItemReward(ItemId.ApUrtecaMountains, 50);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Encounter + 0, Stage.MegadoDetachedPalace, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.WarReadyNightmareLightArmor, 95, 110000, 0)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 95, 4400, 1),
            LibDdon.Enemy.Create(EnemyId.WarReadyGrimwargLightArmor, 95, 4400, 2),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Joseph, 25943)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason32);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.LookoutCastle1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7821)
            .AddResultCmdQstTalkChg(NpcId.Joseph, 25944)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25945)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25950)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25949);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.LookoutCastle1, 1, 0, NpcId.Meirova0, 22330)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7824)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25956)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25951)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25952)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25953);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.FortressCityMegadoRoyalPalaceLevel, 0, 0, NpcId.Yuri, 29894)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7828)
            .AddResultCmdQstTalkChg(NpcId.Yuri, 25957);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MegadoDetachedPalace)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7821)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25958);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.MegadoDetachedPalace, -99, 100, -20824);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Encounter + 0)
            .AddResultCmdSetDiePlayerReturnPos(Stage.MegadoDetachedPalace, 4, 0);
        process0.AddPartyGatherBlock(QuestAnnounceType.Update, Stage.MegadoDetachedPalace, 103, 0, -20459)
            .AddResultCmdResetDiePlayerReturnPos(Stage.Invalid, 0);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.MegadoDetachedPalace, 0, 4)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7828)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7829)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25959)
            .AddResultCmdQstTalkChg(NpcId.Gurdolin3, 25960)
            .AddResultCmdQstTalkChg(NpcId.Lise0, 25961)
            .AddResultCmdQstTalkChg(NpcId.Elliot0, 25962);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.MegadoDetachedPalace, 1, 0, NpcId.Meirova0, 22406)
            .AddResultCmdQstTalkChg(NpcId.Meirova0, 25963);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsReleaseWarpPointAnyone(92)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7829)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 7822)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 8160);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.LookoutCastle1, 0, 0, NpcId.Bertha, 22407)
            .AddResultCmdQstTalkChg(NpcId.Bertha, 25964)
            .AddResultCmdQstTalkChg(NpcId.Gillian0, 25965);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Update, Stage.AudienceChamber, NpcId.Joseph, 22408)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 7822)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 8160)
            .AddResultCmdQstTalkChg(NpcId.TheWhiteDragon, 25971);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
