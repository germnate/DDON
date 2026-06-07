// Dewdrop Crater — stageid462 GroupId6 MaxPos8 (positions 0-8)
// Rathnite Foothills Lakeside, Lv83, AR3
#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.RathniteFoothillsLakeside0.AsStageLayoutId(6);
    public override QuestAreaId AreaId => QuestAreaId.RathniteFoothills;
    public override uint RequiredAreaRank => 3;

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 83, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 83, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 83, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 83, 3),
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGrimwargLightArmor, 83, 4),
            LibDdon.Enemy.CreateAuto(EnemyId.Chimera0, 83, 5, isBoss: true),
            LibDdon.Enemy.CreateAuto(EnemyId.Chimera0, 83, 6, isBoss: true),
        };


        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemihumanCutterStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemihumanCutterStone , 1, 1, DropRate.VERY_RARE);
        enemies[0].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[1]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemihumanCutterStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemihumanCutterStone , 1, 1, DropRate.VERY_RARE);
        enemies[1].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[2]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemihumanCutterStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemihumanCutterStone , 1, 1, DropRate.VERY_RARE);
        enemies[2].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[3]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemihumanCutterStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemihumanCutterStone , 1, 1, DropRate.VERY_RARE);
        enemies[3].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[4]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemihumanCutterStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemihumanCutterStone , 1, 1, DropRate.VERY_RARE);
        enemies[4].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[5]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.DemihumanCutterStoneShard, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.DemihumanCutterStone , 1, 1, DropRate.RARE);
        enemies[5].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[6]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.DemihumanCutterStoneShard, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.DemihumanCutterStone , 1, 1, DropRate.RARE);
        enemies[6].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
