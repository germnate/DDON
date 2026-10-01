/**
 * @brief Those Who Follow the Dragon
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScriptedQuest));

    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.ThoseWhoFollowTheDragon;
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.BreakdownOfReason;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(100));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.HopesBitterEnd));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 900000);
        AddWalletReward(WalletType.Gold, 100000);
        AddWalletReward(WalletType.RiftPoints, 10000);

        AddFixedItemReward(ItemId.RoyalCrestMedalUrtecaDistrict, 5);
        AddFixedItemReward(ItemId.UnappraisedCloudTrinketGeneral, 2);
        AddFixedItemReward(ItemId.ApUrtecaMountains, 50);
        AddFixedItemReward(ItemId.BraceletOfTheFirstKing, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.TheWhiteDragon, 22450);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.AudienceChamber, NpcId.TheWhiteDragon, 22450);
        process0.AddProcessEndBlock(true)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheCrewEndSeason33)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.LookoutCastle.TheCrewEndSeason33);
    }
}

return new ScriptedQuest();
