# Pawn Expedition

Full protocol/DB/manager implementation exists (all 10 packets, DB schema/migration v64,
`PawnExpeditionManager`, clan integration). Known gaps to revisit:

1. **Battle results not persisted** — `GetLastBattleResult` randomly synthesizes a result
   each call instead of storing the actual outcome; repeat views can differ.
2. **Sally spots hardcoded** — 4 placeholder area/spot entries; no real spot data exists
   in the repo's asset files.
3. **Reward item pools are guesswork** — small hand-picked item lists, not sourced from
   real game data.
4. **No live timer** — sally completion is lazy (evaluated on next request), so nothing
   proactively fires when a sally finishes.
5. **Migration path untested** — v64 migration script was never run against a real
   pre-v63 database; only fresh-schema creation and mock-based tests were verified.
6. **No new unit tests** for manager logic (sally lifecycle, reward generation, price
   validation).
7. **No live client verification** — packet structs were reverse-engineered from static
   research files (ELF/pcapng), not confirmed against a running client.
