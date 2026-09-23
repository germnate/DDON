# Locked Chest Audit Report

Generated: 2026-09-23T14:16:00.620Z

## Spot Classification

- Total spots scanned: 6654
- Locked chest spots (GatheringType key lv1-lv4): 442
- Non-locked treasure/chest-like spots: 2131
- Unresolved metadata spots: 0

Top stages by locked chest count (StageNo -> count):
- 431: 26
- 100: 19
- 403: 17
- 408: 14
- 406: 12
- 517: 11
- 872: 10
- 504: 9
- 507: 9
- 519: 9
- 820: 8
- 870: 8
- 825: 7
- 505: 6
- 516: 6
- 590: 6
- 732: 6
- 841: 6
- 871: 6
- 402: 5

## Equipment Lane Policy Health

- Lane key: Level + SubCategory + Jobs
- Total equipment lanes: 1742
- Lanes with droppable pool after top-rank removal: 312
- Lanes that collapse (single-rank only, non-droppable): 1430

Detailed non-droppable lanes: docs/quests/locked_chest_non_droppable_lanes.csv

## Window Simulation (Equipment Only)

- Simulation method: evaluate candidate equipment in each QuestLootRange level window, then apply top-rank reservation per lane.
- Goal check: ensure resulting droppable pool never includes the top rank for any lane.

| Quest Level Bracket | Normal Window | Normal Candidates | Normal Droppable | Normal Removed | Boss Window | Boss Candidates | Boss Droppable | Boss Removed |
| --- | --- | ---: | ---: | ---: | --- | ---: | ---: | ---: |
| 0-20 | 1-20 | 4953 | 3342 | 32.53% | 15-30 | 1558 | 33 | 97.88% |
| 21-40 | 15-45 | 3471 | 271 | 92.19% | 35-55 | 2543 | 437 | 82.82% |
| 41-60 | 35-65 | 4045 | 927 | 77.08% | 55-75 | 3254 | 1526 | 53.1% |
| 61-80 | 55-85 | 4797 | 2397 | 50.03% | 75-100 | 4422 | 2591 | 41.41% |
| 81-999 | 75-110 | 4742 | 2591 | 45.36% | 95-115 | 2090 | 1084 | 48.13% |

Post-filter rank bounds by bracket:
- 0-20: normal rank 1-145, boss rank 4-8
- 21-40: normal rank 4-11, boss rank 8-20
- 41-60: normal rank 8-25, boss rank 1-60
- 61-80: normal rank 1-110, boss rank 1-130
- 81-999: normal rank 1-130, boss rank 95-130

## Notes

- This audit is data-only and does not execute runtime roll logic.
- Locked chest classification uses GatheringType key levels as the authoritative signal.
