// Deserted Village of Denisyr — stageid461 GroupId8 MaxPos9 (positions 0-9)
// Rathnite Foothills, Lv83, AR3
#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.RathniteFoothills.AsStageLayoutId(8);
    public override QuestAreaId AreaId => QuestAreaId.RathniteFoothills;
    public override uint RequiredAreaRank => 3;

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 83, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 83, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 83, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.BluntSoldierDwarfOrc, 83, 3),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 83, 4),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 83, 5),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 83, 6),
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 83, 7),
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 83, 8),
            LibDdon.Enemy.CreateAuto(EnemyId.SquadLeaderDwarfOrc, 83, 9),
        };


        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStone, 1, 1, DropRate.VERY_RARE);
        enemies[0].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[1]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStone, 1, 1, DropRate.VERY_RARE);
        enemies[1].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[2]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStone, 1, 1, DropRate.VERY_RARE);
        enemies[2].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[3]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStone, 1, 1, DropRate.VERY_RARE);
        enemies[3].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[4]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStone, 1, 1, DropRate.VERY_RARE);
        enemies[4].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[5]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStone, 1, 1, DropRate.VERY_RARE);
        enemies[5].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[6]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStone, 1, 1, DropRate.VERY_RARE);
        enemies[6].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[7]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStone, 1, 1, DropRate.VERY_RARE);
        enemies[7].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[8]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStone, 1, 1, DropRate.VERY_RARE);
        enemies[8].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[9]).Clone()
            .AddDrop(ItemId.BeastLuringMeat, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GustyWindsStone, 1, 1, DropRate.VERY_RARE);
        enemies[9].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
