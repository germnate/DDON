#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.KingalCanyon.AsStageLayoutId(44);
    public override QuestAreaId AreaId => QuestAreaId.KingalCanyon;
    public override uint RequiredAreaRank => 13;

    public class NamedParamId
    {
        public const uint Mutated = 1290;
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SeverelyInfectedDemon, 78, 0, isBoss: true)
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
        .AddDrop(ItemId.JetBlackPelt, 1, 2, DropRate.VERY_COMMON)
        .AddDrop(ItemId.JetBlackFur, 1, 3, DropRate.VERY_COMMON)
        .AddDrop(ItemId.Kingalite, 1, 2, DropRate.COMMON);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}


return new MonsterSpotInfo();
