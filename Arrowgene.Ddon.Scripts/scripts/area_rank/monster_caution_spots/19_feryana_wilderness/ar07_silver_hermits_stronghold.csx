// Silver Hermit's Stronghold — stageid463 GroupId65 MaxPos3 (positions 0-3)
// Monster Caution Spot — Feryana Wilderness, Lv88, AR0
#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.FeryanaWilderness.AsStageLayoutId(65);
    public override QuestAreaId AreaId => QuestAreaId.FeryanaWilderness;
    public override uint RequiredAreaRank => 7;

    public class NamedParamId
    {
        public const uint BrutalHermit = 1735;
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WarReadyGorecyclopsLightArmor0, 88, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.BrutalHermit),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 88, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 88, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 88, 3),
        };

        // Available Items (4): WarmMud, UnrefinedAlloyLump, DemonExpellerStoneShard, DemonExpellerStone
        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.WarmMud, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[1]).Clone()
            .AddDrop(ItemId.WarmMud, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.VERY_RARE);
        enemies[1].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[2]).Clone()
            .AddDrop(ItemId.WarmMud, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.VERY_RARE);
        enemies[2].SetDropsTable(dropsTable);

        dropsTable = LibDdon.Enemy.GetDropsTable(enemies[3]).Clone()
            .AddDrop(ItemId.WarmMud, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.UnrefinedAlloyLump, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStoneShard, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonExpellerStone, 1, 1, DropRate.VERY_RARE);
        enemies[3].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
