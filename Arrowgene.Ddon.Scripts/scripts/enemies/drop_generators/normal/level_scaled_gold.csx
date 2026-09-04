/**
 * @brief Drops Coin Pouch items scaled to the killed enemy's level,
 * restricted to large monsters (Cyclops, Chimera, Griffin, Golem, Ogre,
 * Drake/Wyrm/Dragon, Behemoth, Grigori, Manticore, Medusa, Gorgon,
 * Cockatrice, Tarasque, Catoblepas, Colossus, Sphinx, Death, Ghost,
 * Evil Eye family, Orc family, etc.) instead of every enemy.
 *
 * Coin Pouches are real items with their own correct itemlist price
 * (e.g. "Coin Pouch (100 G)") and are automatically converted to Gold
 * when picked up (see ItemManager.ItemIdWalletTypeAndQuantity), so this
 * never causes a mismatch between what's displayed and what's paid out.
 */

#load "libs.csx"

public class Generator : IInstanceEnemyDropGenerator
{
    public GameMode GameMode => GameMode.Normal;

    // EnemyId values are allocated in ranges by the original developers.
    // Every "Large Monster" species (Cyclops, Chimera, Griffin, Golem,
    // Ogre, Drake/Wyrm/Dragon, Behemoth, Grigori, Manticore, Medusa,
    // Gorgon, Cockatrice, Tarasque, Catoblepas, Colossus, Sphinx, Death,
    // Ghost, the Eye family and the Orc family) falls in the 0x015000 -
    // 0x015FFF range, while regular field enemies (Goblin, Wolf,
    // Skeleton, Saurian, Harpy, etc.) fall in other ranges. This lets us
    // detect large monsters without hand-maintaining an ID allowlist.
    private static bool IsLargeMonster(EnemyId enemyId)
    {
        return ((uint)enemyId & 0xFFF000) == 0x015000;
    }

    // Roughly how much gold a level 50 enemy should drop = GoldPerLevel * 50.
    private const double GoldPerLevel = 100.0;

    // 0.0 - 1.0. 1.0 means every kill drops some gold.
    private const double DropChance = 1.0;

    // Adds some variety to the exact payout per kill.
    private const double VarianceMin = 0.8;
    private const double VarianceMax = 1.2;

    // Largest denomination first so we can greedily break down the total
    // into as few stacks as possible. Coin Pouch items stack up to 255.
    private static readonly (ItemId ItemId, uint Value)[] CoinPouchDenominations = new[]
    {
        (ItemId.CoinPouch10000G, 10000u),
        (ItemId.CoinPouch1000G, 1000u),
        (ItemId.CoinPouch100G, 100u),
        (ItemId.CoinPouch10G, 10u),
        (ItemId.CoinPouch1G, 1u),
    };

    private const uint MaxStackSize = 255;

    public List<InstancedGatheringItem> Generate(GameClient client, InstancedEnemy enemyKilled)
    {
        List<InstancedGatheringItem> results = new List<InstancedGatheringItem>();

        if (!IsLargeMonster(enemyKilled.EnemyId))
        {
            return results;
        }

        if (DropChance < 1.0 && Random.Shared.NextDouble() > DropChance)
        {
            return results;
        }

        int level = Math.Max(1, (int)enemyKilled.Lv);
        double variance = VarianceMin + (Random.Shared.NextDouble() * (VarianceMax - VarianceMin));
        uint goldAmount = (uint)Math.Round(level * GoldPerLevel * variance);

        if (goldAmount == 0)
        {
            return results;
        }

        foreach (var (itemId, value) in CoinPouchDenominations)
        {
            uint count = Math.Min(goldAmount / value, MaxStackSize);
            if (count == 0)
            {
                continue;
            }

            results.Add(new InstancedGatheringItem()
            {
                ItemId = itemId,
                ItemNum = count
            });

            goldAmount -= count * value;
        }

        return results;
    }
}

return new Generator();
