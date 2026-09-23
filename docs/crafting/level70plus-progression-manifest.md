# 70+ Crafting Progression Manifest

Generated: 2026-09-23

## Coverage Snapshot

| Tier | Level Range | Total Gear | With Recipe | Missing |
| --- | --- | ---: | ---: | ---: |
| A | 70-79 | 1314 | 255 | 1059 |
| B | 80-89 | 1542 | 227 | 1315 |
| C | 90-99 | 1155 | 49 | 1106 |
| D | 100-109 | 1190 | 7 | 1183 |
| E | 110+ | 320 | 2 | 318 |

## Existing 70+ EXM Material Anchors

| QuestId | BaseLevel | MinItemRank | Fixed Reward ItemId | Fixed Reward Name | Amount |
| ---: | ---: | ---: | ---: | --- | ---: |
| 50202000 | 70 | 37 | 11810 | Rare Vortex Crystal | 3 |
| 50202000 | 70 | 37 | 7795 | Blood Orb (1 BO) | 650 |
| 50203000 | 75 | 52 | 15940 | Rare Green Tree Crystal | 3 |
| 50203000 | 75 | 52 | 7795 | Blood Orb (1 BO) | 750 |
| 50204002 | 80 | 67 | 15997 | Ocean Crystal | 3 |
| 50204002 | 80 | 67 | 7795 | Blood Orb (1 BO) | 1000 |
| 50303000 | 95 | 110 | 18741 | Cyclops Lantern (Gold) | 1 |
| 50400004 | 100 | 0 | 24819 | Black and Silver Chaos Metal | 1 |
| 50400004 | 100 | 0 | 24737 | Otherworldly Drop | 1 |

## Proposed Tier Material Matrix

| Tier | Recipe Level Range | Core EXM Material Route | Secondary Route | Common Gathering Route (non-70-area policy) |
| --- | --- | --- | --- | --- |
| A | 70-79 | EM6 q50202000 fixed rewards plus EXM chest rare pool | QuestInstance chest materials filtered by QuestLootRanges plus curated quest reward drops | Corrupted Sealer Stone (21251), Pyroclastic Rock (17883), Blaze Grass (17885), Dried Test Sample (17907) |
| B | 80-89 | EM7 q50203000 fixed rewards plus EXM chest rare pool | QuestInstance chest materials filtered by QuestLootRanges plus curated quest reward drops | Waterweed (21213), Rose Megadosys (21214), Giant Caterpillar (21215), Phlogopite (21216) |
| C | 90-99 | EM8 q50204002 fixed rewards plus EXM chest rare pool | QuestInstance chest materials filtered by QuestLootRanges plus curated quest reward drops | Scroll of Tribute (21237), Urteca Hot Spring Water (21255), Belladonna (21256), Petrified Wood (21257) |
| D | 100-109 | EXM q50303000 fixed and random rewards plus high-rank QuestRareMaterials | QuestInstance chest materials filtered by QuestLootRanges plus curated quest reward drops | Royal Crest Medal (Megadosys District) (18820), Retainer's Spirit (18660), Dragon Temple's Charm (21275), Dragon Temple's Blessed Cloth (21276) |
| E | 110+ | EXM q50400004 fixed rewards plus top-rank QuestRareMaterials | QuestInstance chest materials filtered by QuestLootRanges plus curated quest reward drops | Royal Crest Medal (Urteca District) (18826), Relic Steel (Urteca District) (21392), Memento Fiber (Urteca District) (21393), Royal Crest Medal (Megadosys District) (18820) |

## Notes

- Quest boss chest mapping is currently empty and should be populated to improve high-tier chest reliability.
- Recipe unlock gating can use CraftingRecipe.UnlockID with existing server-side unlock checks.
- Missing recipe list is exported as CSV for implementation.
