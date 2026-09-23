# Plan: Level-Appropriate Equipment From Locked Quest Chests

**Date:** 2026-09-23  
**Scope:** Quest-instance locked treasure chests only  
**Related:** `docs/quests/loot_generator_plan_exm_and_campaign.md`, `docs/items/armor-weapons-by-rank.csv`

## 1. Goal

Restrict equipment chest drops so that:

1. Only **locked treasure chests** in quest instances use the new equipment-drop logic.
2. Dropped equipment is **level appropriate** for the quest.
3. Dropped equipment is **not the highest rank available** for that equipment lane.
4. The **highest rank is reserved for crafting**, not chest drops.

This is a refinement of the existing quest-instance chest generator rather than a new loot system.

## 2. Existing Constraints

### 2.1 Level appropriateness must use item level, not item rank

The existing quest chest design already made the correct call here: equipment should be filtered by `ClientItemInfo.Level`, not by `ClientItemInfo.Rank`.

Why:

- The exported item catalog in `docs/items/armor-weapons-by-rank.csv` shows that the same level can map to many different ranks.
- The repository's earlier quest loot work already documents this mismatch in `docs/quests/loot_generator_plan_exm_and_campaign.md`.
- The current implementation already filters equipment by level in `Arrowgene.Ddon.GameServer/GatheringItems/Generators/QuestInstanceGatheringItemGenerator.cs`.

Therefore:

- **Level** decides whether an item is appropriate for the quest.
- **Rank** is only used as a secondary policy for which items in that level-appropriate pool may drop.

### 2.2 Locked chests are already represented in code

The codebase already distinguishes locked chests from other treasure-like gathering nodes:

- `GatheringType.OM_GATHER_KEY_LV1`
- `GatheringType.OM_GATHER_KEY_LV2`
- `GatheringType.OM_GATHER_KEY_LV3`
- `GatheringType.OM_GATHER_KEY_LV4`

These are grouped by `GatherTypeExtension.IsLockedChest()` in `Arrowgene.Ddon.Shared/Model/GatheringType.cs`.

This should be the authoritative definition of a locked chest.

## 3. Current Implementation Surface

The current quest-instance chest logic already exists in:

- `Arrowgene.Ddon.GameServer/GatheringItems/Generators/QuestInstanceGatheringItemGenerator.cs`

Relevant behavior today:

1. Resolves the active quest for a stage/position.
2. Loads the correct `QuestLootRange` bucket from `Arrowgene.Ddon.Shared/Files/Assets/QuestLootRanges.json`.
3. Uses `NormalItemLevelMin/Max` or `BossItemLevelMin/Max`.
4. Rolls equipment or material rewards.
5. Applies higher-end weighting for boss chests.

Relevant gap today:

- It does **not** currently check whether the gathering spot is a locked chest before rolling this special quest loot.
- It does **not** currently reserve the highest-rank level-appropriate items for crafting.

## 4. Revised Design

### 4.1 Apply the special equipment logic only to locked chests

Add a lock-state gate to `QuestInstanceGatheringItemGenerator.Generate()`.

Desired behavior:

- If the interacted spot is **not** a locked chest, the quest-specific equipment-roll path should not run.
- If the interacted spot **is** a locked chest, proceed with quest-specific reward logic.

This changes the current scope from:

- all quest treasure chests

to:

- only quest **locked** treasure chests

### 4.2 Keep level-based filtering exactly as the outer filter

Do not replace the existing level windows.

Use the current quest-level buckets in `QuestLootRanges.json`:

- `NormalItemLevelMin`
- `NormalItemLevelMax`
- `BossItemLevelMin`
- `BossItemLevelMax`

These remain the first-pass filter for candidate equipment.

### 4.3 Reserve the top rank within each equipment lane

After building the level-appropriate candidate set, apply a second filter that removes the top rank from the chest-drop pool.

The grouping key should be:

- `Level`
- `Subcategory`
- `JobGroup`

This avoids comparing unrelated items such as helmets, swords, robes, and shields against each other.

For each group:

1. Gather all candidate items in that lane.
2. Determine the distinct ranks present.
3. Identify the highest distinct rank.
4. Exclude all items at that highest rank from chest drops.

Result:

- Chest drops remain level appropriate.
- The best-in-lane rank remains craft-only.

### 4.4 Boss chests still use the better droppable pool, not the craft-only pool

Boss-room locked chests should still feel better than ordinary locked chests.

Keep the current boss behavior:

- higher item-level window
- rarity bias toward the upper end of the allowed window
- rare material chance for EXM where applicable

But boss chests must still obey the same craft-reservation rule:

- boss chests may roll the **best droppable rank**
- boss chests may **not** roll the craft-reserved top rank

## 5. How To Determine Whether A Quest Chest Is Locked

### 5.1 Preferred source: `GatheringSpotInfo.json`

The cleanest source of truth is the spot metadata already modeled in:

- `Arrowgene.Ddon.Shared/Asset/GatheringInfoAsset.cs`
- `Arrowgene.Ddon.Shared/AssetReader/GatheringSpotInfoAssetDeserializer.cs`

That asset provides, per `(StageId, GroupNo, PosId)`:

- `GatheringType`
- `UnitId`
- `Position`

Plan:

1. Resolve the gathering spot for `(stageId.Id, stageId.GroupId, posId)`.
2. Read its `GatheringType`.
3. Call `IsLockedChest()`.
4. Only allow the locked-chest quest equipment logic if that returns `true`.

### 5.2 Fallback source: explicit quest locked-chest map

If instanced quest stages are missing or incomplete in `GatheringSpotInfo.json`, add a small explicit asset:

- `Arrowgene.Ddon.Shared/Files/Assets/QuestLockedChests.json`

Recommended shape:

```json
[
	{
		"QuestId": 50101020,
		"StageId": 293,
		"GroupId": 0,
		"PosId": 5,
		"LockLevel": 3
	}
]
```

Minimum required fields:

- `QuestId`
- `StageId`
- `GroupId`
- `PosId`

Optional field:

- `LockLevel` if later balancing should differentiate LV1-LV4 locked chests

This asset should only be used where metadata is absent or unreliable.

## 6. Drop-Policy Data

### 6.1 Do not hardcode craft-reserved ranks in C#

The chest-drop ceiling should be data-driven.

Recommended asset:

- `QuestEquipmentDropPolicy.json`

Recommended generated fields per lane:

- `Level`
- `Subcategory`
- `JobGroup`
- `MaxDroppableRank`
- optional `ExcludedItemIds`

This policy can be generated from:

- `Arrowgene.Ddon.Shared/Files/Assets/itemlist.csv`

and optionally cross-checked against:

- `Arrowgene.Ddon.Shared/Files/Assets/CraftingRecipes.json`

### 6.2 Generation rule for v1

For each `(Level, Subcategory, JobGroup)` lane:

1. Collect all equipment rows.
2. Find the highest distinct rank.
3. Set `MaxDroppableRank` to the next-highest distinct rank.
4. If no next-highest rank exists, mark the lane as non-droppable.

This gives a predictable rule:

- highest rank reserved for crafting
- everything below it remains eligible

### 6.3 Override support for anomalies

The item catalog contains cosmetic, event, and other outlier items that should not enter normal locked-chest pools even if they are technically below the top rank.

Add explicit override support:

- `ExcludedItemIds`
- optional `ReserveTopDistinctRanks`

Use this to remove promotional/collab/cosmetic outliers without rewriting the general rule.

## 7. Code Changes

### 7.1 `QuestInstanceGatheringItemGenerator`

Primary changes go in:

- `Arrowgene.Ddon.GameServer/GatheringItems/Generators/QuestInstanceGatheringItemGenerator.cs`

Planned changes:

1. Resolve whether the current spot is a locked chest.
2. Early-return from the quest-specific equipment path if it is not locked.
3. After the level-based candidate filter, apply the drop-policy ceiling.
4. Keep existing boss weighting and EXM rare-material behavior.

### 7.2 Asset registration

If a fallback map or policy asset is added, register it in:

- `Arrowgene.Ddon.Shared/AssetRepository.cs`

Likely new assets:

- `QuestLockedChests.json`
- `QuestEquipmentDropPolicy.json`

### 7.3 Optional helper model types

Add small model types under `Arrowgene.Ddon.Shared/Model/Quest/` for:

- locked chest entries
- equipment drop policy entries

Keep them narrow and data-only.

## 8. Validation Plan

### 8.1 Spot audit

Produce a report of quest-instance chest spots, classified into:

- locked chest
- non-locked treasure chest
- unresolved metadata

Only the first group should use the new equipment logic.

### 8.2 Candidate-pool audit

Using `docs/items/armor-weapons-by-rank.csv`:

1. Build lanes by `Level + Subcategory + JobGroup`.
2. Verify each lane has at least one non-top-rank candidate.
3. Flag lanes that collapse to zero after reserving the top rank.

These zero-candidate lanes should either:

- drop no equipment, or
- be handled by explicit policy overrides

### 8.3 Roll simulation

Simulate chest rolls across each `QuestLootRange` bracket:

- normal locked chest
- boss locked chest

Validate:

- item levels stay inside the configured quest window
- highest rank never drops
- boss chests still produce better droppable gear than normal locked chests

## 9. Non-Goals

This plan does **not** change:

- overworld chest behavior
- Bitterblack Maze chest logic
- Epitaph Road chest logic
- ordinary unlocked quest treasure chests
- EXM rare material handling, except that it remains attached to the locked-chest flow if desired

## 10. Final Rule Set

The intended final behavior is:

1. A player interacts with a quest-instance gathering spot.
2. If the spot is **not** a locked chest, this special equipment-drop plan does not apply.
3. If the spot **is** a locked chest, resolve the quest's level bracket.
4. Build the candidate equipment pool using the bracket's item-level window.
5. Remove the highest-rank candidates within each equipment lane.
6. Roll from the remaining pool.
7. Allow boss locked chests to use better windows and weighting, but never the craft-reserved top rank.

That preserves the intended progression model:

- locked chests give meaningful, level-appropriate gear
- chest drops do not invalidate crafting
- the very best rank remains something players must craft rather than simply loot
