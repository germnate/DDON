// Demon Army Relay Station — stageid463 GroupId1 MaxPos8 (positions 0-8)
// Monster Caution Spot — Feryana Wilderness, Lv88, AR0
#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.FeryanaWilderness.AsStageLayoutId(1);
    public override QuestAreaId AreaId => QuestAreaId.FeryanaWilderness;
    public override uint RequiredAreaRank => 3;

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGorecyclopsLightArmor0, 88, 0, isBoss: true),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 3),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 4),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 5),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 6),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 7),
            LibDdon.Enemy.CreateAuto(EnemyId.RangedSoldierDwarfOrc, 88, 8),
        };

        // Available Items (4): BattleArmorFragment, UnrefinedAlloyLump, SkeletonSmasherStoneShard, SkeletonSmasherStone
        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.SkeletonSmasherStoneShard, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.SkeletonSmasherStone, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[1]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStone, 1, 1, DropRate.VERY_RARE);
        enemies[1].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[2]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStone, 1, 1, DropRate.VERY_RARE);
        enemies[2].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[3]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStone, 1, 1, DropRate.VERY_RARE);
        enemies[3].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[4]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStone, 1, 1, DropRate.VERY_RARE);
        enemies[4].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[5]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStone, 1, 1, DropRate.VERY_RARE);
        enemies[5].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[6]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStone, 1, 1, DropRate.VERY_RARE);
        enemies[6].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[7]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStone, 1, 1, DropRate.VERY_RARE);
        enemies[7].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[8]).Clone()
            .AddDrop(ItemId.BattleArmorFragment, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SkeletonSmasherStone, 1, 1, DropRate.VERY_RARE);
        enemies[8].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
