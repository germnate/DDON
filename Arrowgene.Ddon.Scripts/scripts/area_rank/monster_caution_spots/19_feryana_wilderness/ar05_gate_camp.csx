// Gate Camp — stageid463 GroupId9 MaxPos4 (positions 0-4)
// Monster Caution Spot — Feryana Wilderness, Lv88, AR0
#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.FeryanaWilderness.AsStageLayoutId(9);
    public override QuestAreaId AreaId => QuestAreaId.FeryanaWilderness;
    public override uint RequiredAreaRank => 5;

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGoremanticoreLightArmor, 88, 0, isBoss: true),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 3),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 4),
        };

        // Available Items (4): BattleArmorFragment, UnrefinedAlloyLump, GiantKillerStoneShard, GiantKillerStone
        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.GiantKillerStoneShard, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.GiantKillerStone, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[1]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GiantKillerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GiantKillerStone, 1, 1, DropRate.VERY_RARE);
        enemies[1].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[2]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GiantKillerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GiantKillerStone, 1, 1, DropRate.VERY_RARE);
        enemies[2].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[3]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GiantKillerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GiantKillerStone, 1, 1, DropRate.VERY_RARE);
        enemies[3].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[4]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GiantKillerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GiantKillerStone, 1, 1, DropRate.VERY_RARE);
        enemies[4].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
