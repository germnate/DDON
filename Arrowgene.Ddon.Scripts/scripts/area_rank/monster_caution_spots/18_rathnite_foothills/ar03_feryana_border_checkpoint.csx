// Feryana Border Checkpoint — stageid462 GroupId32 MaxPos6 (positions 0-6)
// Rathnite Foothills Lakeside, Lv83, AR3
#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.RathniteFoothillsLakeside0.AsStageLayoutId(32);
    public override QuestAreaId AreaId => QuestAreaId.RathniteFoothills;
    public override uint RequiredAreaRank => 3;

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.BlackGriffin0, 83, 0, isBoss: true),
            LibDdon.Enemy.CreateAuto(EnemyId.HeavySoldierDwarfOrc, 83, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.HeavySoldierDwarfOrc, 83, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.HeavySoldierDwarfOrc, 83, 3),
            LibDdon.Enemy.CreateAuto(EnemyId.HeavySoldierDwarfOrc, 83, 4),
            LibDdon.Enemy.CreateAuto(EnemyId.HeavySoldierDwarfOrc, 83, 5),
            LibDdon.Enemy.CreateAuto(EnemyId.HeavySoldierDwarfOrc, 83, 6),
        };


        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[1]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.VERY_RARE);
        enemies[1].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[2]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.VERY_RARE);
        enemies[2].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[3]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.VERY_RARE);
        enemies[3].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[4]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.VERY_RARE);
        enemies[4].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[5]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.VERY_RARE);
        enemies[5].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[6]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.VERY_RARE);
        enemies[6].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
