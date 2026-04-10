// The Howling Front — stageid462 GroupId1 MaxPos11 (positions 0-11)
// Rathnite Foothills Lakeside, Lv80, AR0
#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.RathniteFoothillsLakeside0.AsStageLayoutId(1);
    public override QuestAreaId AreaId => QuestAreaId.RathniteFoothills;
    public override uint RequiredAreaRank => 0;

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 80, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 80, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 80, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 80, 3),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 80, 4),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 80, 5),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 80, 6),
            LibDdon.Enemy.CreateAuto(EnemyId.Strix, 80, 7),
            LibDdon.Enemy.CreateAuto(EnemyId.Strix, 80, 8),
            LibDdon.Enemy.CreateAuto(EnemyId.Strix, 80, 9),
            LibDdon.Enemy.CreateAuto(EnemyId.Strix, 80, 10),
            LibDdon.Enemy.CreateAuto(EnemyId.Strix, 80, 11),
        };


        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[0].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[1]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[1].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[2]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[2].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[3]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[3].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[4]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[4].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[5]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[5].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[6]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[6].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[7]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[7].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[8]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[8].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[9]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[9].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[10]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[10].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[11]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BrittleSandstone, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.PyroclasticRock, 1, 1, DropRate.VERY_RARE);
        enemies[11].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
