// Demon Army War Machine Garrison — stageid462 GroupId5 Pos5 (position 5 only)
// Rathnite Foothills Lakeside, Lv85, AR10
#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.RathniteFoothillsLakeside0.AsStageLayoutId(5);
    public override QuestAreaId AreaId => QuestAreaId.RathniteFoothills;
    public override uint RequiredAreaRank => 10;

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Catoblepas, 85, 5, isBoss: true),
        };


        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.DepthOpalRing, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
