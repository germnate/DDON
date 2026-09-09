# Plan: Level-Appropriate Loot Generators for Extreme Mission & Campaign Treasure Chests

**Status:** Implemented (see "Implementation Status" below); solution builds clean
**Date:** 2026-09-08
**Related:** `docs/gathering_nodes.md`, `docs/quests/generic_quest_state_machine.md`

## 0. Implementation Status (2026-09-08)

Implemented and building cleanly. Two deviations from the original design below,
both discovered from real data during implementation:

1. **Item filtering uses `ClientItemInfo.Level`, not `Rank`.** Direct inspection of
   the full `itemlist.csv` (not just BBM's curated subsets) showed `Rank` does not
   correlate consistently with a piece of equipment's intended character level
   (e.g. a rank-15 item can require level 62 while a rank-45 item requires level 1).
   `Rank` is only a reliable tier signal *within* BBM's small hand-picked item
   lists, not across the whole catalog. `Level` (populated for ~16k/17k Equipment
   items, itemlist.csv Category 3) is the correct, direct signal for "is this
   item appropriate for a level-N mission" and is what `QuestLootRange` now uses
   (`NormalItemLevelMin/Max`, `BossItemLevelMin/Max` instead of `NormalRange`/`BossRange`).
2. **v2: materials/consumables added, weighted below equipment.** Rather than
   authoring a brand-new leveled material table, the generator reuses the
   existing hand-curated `DefaultGatheringDropsAsset` (`DefaultGatheringDrops.json`)
   pool - the same data that powers overworld gathering nodes/chests - which
   already carries a per-item `ItemLevel`. Since quest instances have no
   `QuestAreaId`, the pool is flattened across *all* areas (deduped by ItemId)
   rather than keyed by area; "area appropriate" is satisfied via level-window
   filtering the same way equipment is. Each chest rolls exactly one reward:
   30% chance Equipment (`EquipmentDropChance` in
   `QuestInstanceGatheringItemGenerator`), 70% chance Material/Consumable
   (any `DropCategory` except `Equipment`/`Jewelry`). The 30% ceiling on gear
   is intentional - keeping materials/consumables (and by extension, the gald
   earned from selling/using them) the common case so equipment drops don't
   make everything else in the economy obsolete. If a chosen path yields no
   candidates for the level window, it falls back to the other path so a
   chest is never silently empty.
3. **Signature EXM rare materials get a dedicated, boosted roll.** Investigation
   showed EXM's headline rewards (e.g. "Rare Netherworld Crystal", "Rare Vortex
   Crystal", "Otherworldly Drop") are hand-placed quest-completion rewards, not
   part of any gathering/chest pool - so without extra work, chests would never
   produce them even after the Equipment/Material split above. Added a curated
   allow-list of 21 such items (`QuestRareMaterials.json`, gathered by scanning
   `item_id` references across all 17 EXM quest JSONs and filtering to the
   clearly EXM-signature crystals/drops), bucketed for eligibility by
   `ClientItemInfo.Rank` per `QuestLootRange.RareMaterialRankMin/Max` (Rank
   scales with EXM base_level in the observed reward data: e.g. Rank 12 for
   BaseLevel ~58, Rank 130 for BaseLevel 100). For Extreme Mission quests only
   (`QuestUtils.IsExmQuest`), each chest first rolls against
   `RareMaterialChanceNormal`/`RareMaterialChanceBoss` (5-12% normal, 20-35%
   boss) before falling through to the Equipment/Material split - boss-room
   chests are meaningfully more likely to hand out one of these signature
   materials, matching the expectation that "the last chests" are the big
   payoff.

Implemented files:
- `Arrowgene.Ddon.Shared/Model/Quest/QuestLootRange.cs`, `QuestBossChestEntry.cs`
- `Arrowgene.Ddon.Shared/Files/Assets/QuestLootRanges.json` (5 level brackets, 0-999)
- `Arrowgene.Ddon.Shared/Files/Assets/QuestBossChests.json` (empty - see below)
- `Arrowgene.Ddon.Shared/Files/Assets/QuestRareMaterials.json` (21 curated EXM item IDs)
- `Arrowgene.Ddon.Shared/AssetRepository.cs` (new keys/properties/registrations)
- `Arrowgene.Ddon.GameServer/GatheringItems/Generators/QuestInstanceGatheringItemGenerator.cs`
- `Arrowgene.Ddon.GameServer/GatheringItems/InstanceGatheringItemManager.cs` (registered generator)
- `Arrowgene.Ddon.GameServer/Characters/QuestManager.cs` (stage-number index extended to
  World/ExtremeMission/Light quest types)
- `Arrowgene.Ddon.Server/Settings/GameServerSettings.cs` +
  `Arrowgene.Ddon.Scripts/scripts/settings/templates/GameServerSettings.csx`
  (new `EnableQuestInstanceChestDrops`, default `true`)

**Outstanding follow-up:** `QuestBossChests.json` ships empty - EM1's exact two
boss-room chest `(StageId, GroupId, PosId)` coordinates were never confirmed
in-game. Until entries are added there, *all* EXM chests (including the boss
room's) roll from the Normal item-level window rather than the Boss window -
they will no longer be empty, they just won't get the extra rarity bump yet.
The generator logs a Debug line per chest roll (`QuestInstanceGatheringItemGenerator`)
including quest id + `stageId.groupId.pos` - next time EM1 is run, check server
logs for the final chests' coordinates and add them to `QuestBossChests.json`, e.g.:
```json
[
  { "QuestId": 50101020, "StageId": 293, "GroupId": 0, "PosId": 5 }
]
```

## 1. Problem Statement

Treasure chest *objects* placed in a level always render — they come from the
original client's map data and are not something the server needs to spawn.
What the server is responsible for is the **loot inside each chest**, which is
resolved per-generator through `InstanceGatheringItemManager.Generate()` based
on `(StageLayoutId, PosId)`.

Confirmed by inspection:

- `GatheringItem.csv` (hand/rip-populated, keyed by exact stage+position) has
  **no rows at all** for instanced quest/mission stage IDs (e.g. EM1's combat
  stage `293`). Only ~130 open-world overworld stage IDs are populated.
- The generic area-fallback generator (`DefaultGatheringItemGenerator` /
  `default_gathering.csx`) only works for stage numbers present in
  `GatheringSpotInfoAsset` / `DefaultGatheringDropsAsset` — again, only
  overworld zones.
- Bitterblack Maze is the **only** dungeon type with a dedicated loot roller
  (`BitterblackGatheringItemGenerator` → `BitterblackMazeManager.RollChestLoot`),
  which resolves a per-stage `(NormalRange, SealedRange)` item-rank window
  (`BitterblackMazeAsset.LootRanges`) and rolls weapons/armor/materials
  filtered by `ClientItemInfos[itemId].Rank` falling inside that window.
- No equivalent generator exists for Extreme Mission or campaign
  (Main/World/Board/Clan) quest instances, so every chest in those instances
  resolves to an empty item list. **This is a missing data/generator gap, not
  a broken reference** — nothing is silently failing, the pipeline simply has
  no rows to return.

## 2. Goal

Add a new gathering generator (mirroring the existing `IGatheringGenerator`
pattern) that:

1. Produces **level-appropriate loot** for treasure chests inside Extreme
   Mission and campaign-quest instances, scaled off each quest's own
   `base_level` (already present in every quest JSON, e.g.
   `q50101020.json: "base_level": 58`) — not the player's own level/gear, so
   loot is consistent for every party regardless of who forms it.
2. Gives the **last two chests in an Extreme Mission's boss room** a
   separately-configured, much-higher chance of rare/rank-appropriate items
   (mirroring BBM's `SealedRange` vs `NormalRange` concept), instead of
   drawing from the same pool as every other chest in the mission.
3. Requires **no new per-item data entry** for the common case — item level
   already exists as `Rank` in `itemlist.csv` (loaded into
   `ClientItemInfos[itemId].Rank`) and is proven usable for rank-filtered
   rolling by the BBM generator already in production.

## 3. Design Overview

### 3.1 New model: `QuestLootRange` (per-mission-level loot config)

Introduce a small config keyed by a **level bracket**, not by raw stage ID
(campaign/EXM stage IDs are numerous and per-instance; level brackets are
finite and reusable):

```csharp
public class QuestLootRange
{
    public uint MinLevel { get; set; }      // inclusive base_level lower bound
    public uint MaxLevel { get; set; }       // inclusive base_level upper bound
    public (uint Min, uint Max) NormalRange; // item Rank window for regular chests
    public (uint Min, uint Max) BossRange;   // item Rank window for the final 1-2 chests
    public double BossRareChanceBonus;       // extra probability weight favoring the top of BossRange
}
```

Loaded from a new small JSON/CSV asset (`Files/Assets/QuestLootRanges.json`),
analogous to `BitterblackMazeAsset.LootRanges` but bucketed by level range
instead of per-stage, e.g.:

```json
[
  { "min_level": 1,  "max_level": 30, "normal_rank": [1, 4],  "boss_rank": [3, 6],  "boss_rare_bonus": 0.25 },
  { "min_level": 31, "max_level": 60, "normal_rank": [3, 7],  "boss_rank": [6, 10], "boss_rare_bonus": 0.30 },
  { "min_level": 61, "max_level": 999,"normal_rank": [6, 11], "boss_rank": [9, 14], "boss_rare_bonus": 0.35 }
]
```

This gives one small, human-editable table that scales with future
level/rank content instead of thousands of hand-authored CSV rows.

### 3.2 New model: `QuestBossChestMap` (identifying the "final chests")

Need a way to know, for a given `(quest_id, StageLayoutId, PosId)`, whether a
chest is one of the mission's final boss-room chests. Two options evaluated:

- **Option A (chosen): opt-in JSON list.** A small
  `Files/Assets/QuestBossChests.json` mapping
  `quest_id -> [ { stage_id, group_id, pos_id }, ... ]` for the 1-2 chests
  that should use `BossRange`. Every other chest in that quest instance
  automatically falls back to `NormalRange`. This avoids needing to infer
  "boss room" from stage layout heuristics, and matches the existing
  hand-authored style used for `gSealedChestDrops` in
  `BitterblackMazeManager.cs`.
- Option B (rejected for v1): infer automatically from the `enemy_groups`
  block that guards the final process (e.g. the group whose defeat sets the
  last required `MyQstFlags`). More elegant, but touches per-quest-format
  parsing edge cases (classic vs. categorized reward formats, multi-phase
  quests) that would expand scope significantly. Can be layered on later as a
  convenience default-population script.

### 3.3 New generator: `QuestInstanceGatheringItemGenerator`

Added to `InstanceGatheringItemManager`'s `Generators` list (after
`BitterblackGatheringItemGenerator`/`EpitaphRoadGatheringItemGenerator`, since
those two already early-return for their own stage ranges and this generator
must not run for them):

```csharp
public class QuestInstanceGatheringItemGenerator : IGatheringGenerator
{
    public override bool IsEnabled() => Server.GameSettings.GameServerSettings.EnableQuestInstanceChestDrops;

    public override List<InstancedGatheringItem> Generate(GameClient client, StageLayoutId stageId, uint index)
    {
        // 1. Resolve the active quest instance for this client/stage (see 3.4).
        // 2. Skip if not an Exm or campaign-family quest (World/Main/Board/Clan) — leaves
        //    Bitterblack/Epitaph/overworld untouched, and leaves other quest
        //    types (Tutorial/Light/Substory) alone for now.
        // 3. Look up base_level -> QuestLootRange bucket.
        // 4. Determine chest tier: BossRange if (quest_id, stageId, index) is in
        //    QuestBossChestMap, else NormalRange.
        // 5. Roll 1-4 items (mirroring DefaultGatheringItemGenerator's slot logic)
        //    from ClientItemInfos filtered by Rank within the resolved range,
        //    respecting DropCategory pools appropriate to a TreasureChest spot
        //    (reuse GatheringPointType.TreasureChest's DropCategories mapping already
        //    defined in default_gathering.csx, if the spot metadata is available;
        //    otherwise draw from a general equipment+material pool).
        // 6. For boss chests, apply BossRareChanceBonus to bias rolls toward the
        //    top of BossRange (highest-rank/rarest items available for that level).
    }
}
```

### 3.4 Resolving the active quest from stage context (precedent confirmed)

`InstanceGatheringItemManager.Generate()` only receives `(StageLayoutId, index)`
— it does not currently receive the active `quest_id`. However, **the
resolution path already exists in the codebase and is used in production for
Tutorial quests** — no new lookup mechanism needs to be invented:

1. `Quest.BaseLevel` (`Quests/Quest.cs:81`) and `Quest.StageId`
   (`Quests/Quest.cs:79`) are already populated on every loaded `Quest` object
   from each quest asset's `base_level` field — this is the value the new
   generator needs.
2. `QuestManager.GetQuestByStageNo(QuestType questType, uint stageNo)`
   (`Characters/QuestManager.cs:345`) is a static reverse index — **stage
   number → matching quest schedule IDs** — already built by
   `AddQuestToCollections()` and already consumed by
   `QuestGetTutorialQuestListHandler.cs` the same way this plan needs it.
   Today `AddQuestToCollections()` only populates this index for
   `QuestType.Tutorial` and `QuestType.Substory`; it needs to be **extended
   to also index `QuestType.ExtremeMission`, `QuestType.World`,
   `QuestType.Board`, and `QuestType.Clan`** the exact same way (small,
   additive change to an existing `if`/`else if` chain).
3. `SharedQuestStateManager`/`SoloQuestStateManager` (accessible via
   `client.Party.QuestState` per the existing reward-pipeline trace) already
   expose `GetActiveQuestScheduleIds()` / `HasActiveQuest(scheduleId)` —
   this tells us which of the candidate schedule IDs from step 2 is actually
   running for this specific client/party right now.

Putting it together, the generator resolves its quest context as:

```csharp
uint stageNo = StageManager.ConvertIdToStageNo(stageId);
var candidates = QuestManager.GetQuestByStageNo(QuestType.ExtremeMission, stageNo)
    .Concat(QuestManager.GetQuestByStageNo(QuestType.World, stageNo))
    // ...Board, Clan as needed
    .Where(client.Party.QuestState.HasActiveQuest);
var scheduleId = candidates.FirstOrDefault();
if (scheduleId == 0) return new(); // no matching active quest for this stage
Quest quest = QuestManager.GetQuestByScheduleId(scheduleId);
uint baseLevel = quest.BaseLevel;
```

This is a low-risk, additive change (new index entries + a lookup helper) —
no changes to `Generate()`'s signature or its call sites are required, and no
spike/investigation phase is needed before implementation.

## 4. Scope Boundaries

- **In scope:** Extreme Mission (`Exm`) quests and campaign-family quest
  types (Main/World story quests, Board Quests, Clan Quests) — i.e. anything
  currently getting a hardcoded empty list. Level scaling comes from each
  quest's own `base_level`.
- **Boss-chest special handling:** Extreme Missions only, per the user's
  request ("...except for the last two chests at the end of the mission").
  Campaign missions get uniform level-scaled loot with no special end-chest
  tier, since the user only called out this behavior for EXMs.
- **Out of scope / unchanged:** Bitterblack Maze, Epitaph Road, overworld
  gathering spots, `GatheringItem.csv`-driven spots — all already have working
  generators and must not regress.
- **Out of scope for v1:** auto-detecting boss chests from quest logic
  (Option B above); can be a fast-follow.

## 5. Implementation Phases

1. **Extend quest-by-stage indexing:** in `QuestManager.AddQuestToCollections`,
   add `QuestType.ExtremeMission`, `QuestType.World`, `QuestType.Board`, and
   `QuestType.Clan` to the branch that populates `QuestByStageNo` (currently
   Tutorial/Substory only) — mirrors existing code exactly, see section 3.4.
2. **Data model + asset loader:**
   - `Model/Quest/QuestLootRange.cs`, `AssetReader/QuestLootRangeDeserializer.cs`,
     wire into `AssetRepository` (mirrors `BitterblackMazeAsset.LootRanges`
     pattern already in the codebase).
   - `Files/Assets/QuestLootRanges.json` seeded with an initial 3-5 level
     brackets covering the full base_level range across all quest types
     (derive brackets from the spread of `base_level` values already present
     in `Files/Assets/quests/*.json`).
   - `Model/Quest/QuestBossChestMap.cs` + `Files/Assets/QuestBossChests.json`,
     seeded initially with just the known EM1 boss-room chest positions
     (need in-game/log confirmation of `stage_id`/`group_id`/`pos_id` for
     those two chests — likely obtainable from server debug logging when a
     player interacts with them, or from the `enemy_groups`/`stage_id` blocks
     already present in `q50101020.json` around the final process).
3. **Generator implementation:**
   - `GatheringItems/Generators/QuestInstanceGatheringItemGenerator.cs`
     implementing the logic in 3.3, reusing `Random.Shared.WeightedNext` (as
     `DefaultGatheringItemGenerator` does) for the rarity bias roll.
   - New settings flags in `GameServerSettings`
     (`EnableQuestInstanceChestDrops`, mirroring existing
     `EnableDefaultGatheringDrops`/`EnableToolGatheringDrops` toggles) so this
     can be disabled without a redeploy if something looks wrong.
   - Register in `InstanceGatheringItemManager.Generators`.
4. **Seed data for EM1 as the pilot case:**
   - Confirm the 2 boss-room chest coordinates for `q50101020` (EM1) and
     populate `QuestBossChests.json` with them.
   - Manually verify loot rolls in a local run: normal chests in EM1 should
     draw from the `base_level: 58` bracket's `NormalRange`; the two
     boss-room chests should draw from `BossRange` with the rarity bonus
     applied.
5. **Roll out to remaining EXM and campaign quests:**
   - No further per-quest data entry needed beyond `QuestBossChests.json`
     entries for each EXM's boss-room chests (campaign quests need zero
     additional entries since they have no special-tier chest requirement).
   - Cross-reference `docs/quests/endgame_content.md` to prioritize which
     EXMs to add boss-chest coordinates for first (likely EM1-EM8 core
     missions, matching the existing research doc).
6. **Validation:**
   - Unit/manual test: chests in a low-level campaign quest, a mid-level EXM,
     and a high-level EXM each resolve non-empty, rank-appropriate loot.
   - Regression check: BBM and Epitaph Road chest loot unaffected (their
     generators still early-return before this one runs, and this one must
     early-return for their stage IDs as well, as a defense-in-depth check).
   - Confirm `EnableQuestInstanceChestDrops = false` fully disables the new
     generator with no exceptions (empty-list fallback, matching existing
     `IsEnabled()` gate behavior elsewhere).

## 6. Risks / Considerations

- **Quest-instance lookup (3.4) is the main unknown.** If there's no cheap
  existing accessor from stage context back to the active quest + its
  `base_level`, we may need to thread additional context through
  `InstanceGatheringItemManager.Generate()`'s call sites (multiple handler
  call sites reference it — a signature change has moderate blast radius and
  should be scoped carefully during the spike).
- **Party quests (shared instances):** `Generate()` is called per-`GameClient`,
  but chest contents should probably be consistent for the whole party in a
  shared EXM instance (same as `InstancedItems` caching already does per
  stage/index) — need to confirm this doesn't accidentally roll different
  loot per party member for the same physical chest.
- **Level bracket granularity:** starting with 3-5 broad brackets is
  deliberately coarse to avoid over-engineering; can be refined once we see
  actual play data on what feels "off".
- **Boss chest data entry is manual** for each EXM (v1) — this is a
  reasonable trade-off for a private server, but is the main ongoing
  maintenance cost as new EXMs are added later.

## 7. Files Expected To Be Touched

- New: `Arrowgene.Ddon.Shared/Model/Quest/QuestLootRange.cs`
- New: `Arrowgene.Ddon.Shared/Model/Quest/QuestBossChestMap.cs`
- New: `Arrowgene.Ddon.Shared/AssetReader/QuestLootRangeDeserializer.cs`
- New: `Arrowgene.Ddon.Shared/AssetReader/QuestBossChestMapDeserializer.cs`
- New: `Arrowgene.Ddon.Shared/Files/Assets/QuestLootRanges.json`
- New: `Arrowgene.Ddon.Shared/Files/Assets/QuestBossChests.json`
- New: `Arrowgene.Ddon.GameServer/GatheringItems/Generators/QuestInstanceGatheringItemGenerator.cs`
- Modify: `Arrowgene.Ddon.Shared/AssetRepository.cs` (register new assets)
- Modify: `Arrowgene.Ddon.GameServer/GatheringItems/InstanceGatheringItemManager.cs` (register generator)
- Modify: `Arrowgene.Ddon.Server/Settings/GameServerSettings.cs` +
  `Arrowgene.Ddon.Scripts/scripts/settings/templates/GameServerSettings.csx`
  (new enable flag)
- Possibly modify: `GameClient.cs` / `QuestManager.cs` if the quest-lookup
  spike requires exposing a new accessor.

## 8. Explicitly Not Doing (unless asked later)

- Not touching `ddon_reward_box`/reward-box mission-completion rewards (JP,
  quest rewards, etc.) — that pipeline was independently confirmed working
  and out of scope here; this plan is scoped purely to gathering-point/chest
  loot.
- Not retrofitting Bitterblack Maze or Epitaph Road loot tables.
- Not building the automatic boss-chest-detection heuristic (Option B) in v1.
