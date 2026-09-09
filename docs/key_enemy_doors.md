# Key Enemy Doors

Some dungeon doors open only after a specific "key enemy" group is wiped. Normally the **client**
owns that mapping: the server sends `S2CInstanceEnemyGroupDestroyNtc` (which carries only a
`LayoutId` and `IsAreaBoss`), and the client's stage data decides whether that group opens a door.

When the key enemy group is missing from `EnemySpawn.json`, that path can never fire. `KeyDoors.json`
lets the server drive the door's object-manager (OM) state directly instead.

## The asset

`Arrowgene.Ddon.Shared/Files/Assets/KeyDoors.json`:

```json
[
  {
    "Comment": "Crypt of Murmurs - key enemy door",
    "StageId": 331,
    "LayerNo": 0,
    "GroupId": 4,
    "Opens": [
      { "StageId": 331, "GroupId": 23, "PosId": 3, "State": 8 }
    ]
  }
]
```

Two different id namespaces, easy to mix up:

| Field | Meaning |
| --- | --- |
| top-level `GroupId` | the **enemy** group that must be wiped |
| `Opens[].GroupId` / `PosId` | the **OM** (door object) layout id |
| `Opens[].State` | raw `SeasonDungeonOmState`; `8` = `DoorUnlocked` |

`Opens[].StageId` defaults to the entry's `StageId` when set to `0`.

The asset hot-reloads via the existing file watcher, so edits do not need a server restart.

## Confirming the OM values

Use the admin chat command to test a door before writing it into the asset:

```
/updateom <omGroupId> <posId> <state>
```

For example `/updateom 23 3 8` opens the Crypt of Murmurs door. Whatever arguments work here are
exactly the `Opens[]` values.

## Finding a missing enemy group

`/spawntest` spawns 20 dummy crystals in every group/position slot the client's stage data defines,
encoding the slot as **`Lv = groupId * 100 + index`**.

1. `/spawntest`, then leave and re-enter the stage.
2. Walk to the door and read the level off the crystals in front of it — `Lv 700` means group `7`,
   index `0`.
3. That gives you both the group id and how many position slots it has.
4. `/spawntest` again to toggle off.

Then add the real enemies to `EnemySpawn.json` at that `StageId` / `LayerNo` / `GroupId` /
`PositionIndex`, and set the same group as the trigger `GroupId` in `KeyDoors.json`.

## Logging

Two log prefixes help verify the wiring:

- `[EnemyRoster]` — dumped on every enemy set request: the stage layout split into
  `StageId`/`LayerNo`/`GroupId`, plus each enemy's index, id, level and `IsRequired`.
- `[EnemyKill]` — per kill: `X/Y required killed -> GroupDestroyed=`, the live group state, and an
  explicit line when `S2CInstanceEnemyGroupDestroyNtc` and any `[KeyDoor]` state are sent.

If `GroupDestroyed=True` prints but the door stays shut, the trigger group in `KeyDoors.json` does
not match the group you actually cleared.

## Persistence

Opened doors are tracked per party and re-sent on area change, so a door stays open when the stage
reloads. State is cleared when the whole party returns to a safe area.
