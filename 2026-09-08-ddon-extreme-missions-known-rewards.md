# Dragon's Dogma Online Extreme Missions: Known Missions and Rewards

**Research date:** 2026-09-08 (updated)
**Scope:** Dragon's Dogma Online (DDON) Extreme Missions (EM)
**Primary source:** `C:\Project\DDON` — the Arrowgene DDON private-server reimplementation repository

## How this update differs from the first version

The original version of this document was built from web searches of Japanese fan wikis and blogs, so mission/item names were approximate translations and reward tables were incomplete/unverifiable.

This update instead reads the actual quest data shipped in the `C:\Project\DDON` repository:
- Quest definitions: `Arrowgene.Ddon.Shared\Files\Assets\quests\*.json` (reward `item_id`s and amounts)
- Scripted quest rewards: `Arrowgene.Ddon.Scripts\scripts\quests\exm\*.csx` (uses named `ItemId` enum values directly)
- Item names: `Arrowgene.Ddon.Shared\Files\Assets\itemlist.csv` (`#ItemId` → `Name`)
- Mission list/English names/quest IDs: `docs\quests\endgame_content.md` (maintained by the repo's own contributors)

This is developer-authored reference data extracted from the game's own quest files, so it is far more reliable than the earlier web-search-based summary. However, it only covers what the repository has documented or implemented — see "What is still not confirmed" below.

## Four-player Extreme Missions (EM1–EM8 line), confirmed rewards

Quest IDs and reward contents below were read directly from each quest's JSON definition.

| Quest ID | Mission (EN) | Japanese name | EM# | Rec. Level | Min. Item Rank | Confirmed rewards |
|---|---|---|:---:|---:|---:|---|
| 50101020 | The Call of the Catacombs | 地下墓場の誘い | EM1 | 58 | 0 | Rare Netherworld Crystal ×3; Blood Orb ×200; 800 JP |
| 50102020 | Drawn to Ancient Power | 古き力に魅かれし者 | EM2 | 60 | 0 | Rare Ancient Crystal ×3; Blood Orb ×200; 1,500 JP; 25,000 EXP |
| 50103020 | The Ancient City's Legacy | 古都の賜物 | EM3 | 60 | 0 | Rare Greenleaf Crystal ×3; Blood Orb ×200; 1,500 JP; 25,000 EXP |
| 50104000 | The Shining Gate | 輝く扉 | EM4 | 60 | 0 | Rare Golden Crystal ×3; Blood Orb ×1,000; 1,500 JP; 25,000 EXP |
| 50201000 | Agent of Corruption | 歪みの執行人 | EM5 | 65 | 22 | Rare Meteorite Crystal ×3; Blood Orb ×500; 1,550 JP; 35,000 EXP |
| 50202000 | Phantasmic Great Dragon | 淀みし大竜力 | EM6 | 70 | 37 | Rare Vortex Crystal ×3; Blood Orb ×650; 1,600 JP; 50,000 EXP |
| 50203000 | Earth's Fury | 大地の怒り | EM7 | 75 | 52 | Rare Green Tree Crystal ×3; Blood Orb ×750; 1,650 JP; 65,000 EXP |
| 50204002 | Onset of Darkness | 降臨せし闇 | EM8 | 80 | 67 | Ocean Crystal ×3; Blood Orb ×1,000; 2,000 JP; 80,000 EXP |

The "Rare ... Crystal" items are unique per mission and are the closest equivalent to the previously reported 希晶 ("crystal") rewards. Names above are the item's actual in-game English name from `itemlist.csv`, not a re-translation of the Japanese name.

## Related four-player EMs (non-EM# numbered, still in the same quest line)

| Quest ID | Mission (EN) | Japanese name | Rec. Level | Min. Item Rank | Confirmed rewards | Notes |
|---|---|---|---:|---:|---|---|
| 50202003 | With the Myrmidons | 戦徒と共に | 70 | 0 | Book of Acquisition (Companion Healing) ×1; 1,000 JP; 50,000 EXP | Solo/Pawn-only mission (no other players) |
| 50204001 | Onset of Darkness: Restricted Stage | 降臨せし闇・限界域 | 80 | 72 | Panacea ×1; Blood Orb ×2,000; choice of **one**: Greedy Mask, Greedy Breastplate, Greedy Hands, Greedy Boots, or Greedy Mantle; 4,000 JP; 250,000 EXP | Limited-time variant of EM8, separate quest ID from 50204002 |
| 50206000 | Phindym War Chronicles | フィンダム追懐戦記 | 80 | 70 | Central Tree Drop, random quantity (0/5/10/20/30/60, weighted); 1,600 JP; 100,000 EXP | Limited-time event mission |

## Eight-player / Grand Mission-line Extreme Missions, confirmed rewards

| Quest ID | Mission (EN) | Japanese name | Rec. Level | Confirmed rewards |
|---|---|---|---:|---|
| 50300001 | Ancient Warrior (also documented as "Ancient Place of Rituals") | 太古の強者 | 45 | Green Dye ×1; Blood Orb ×200; random Lestania Glass; random Lestania Amber; random Unappraised Moon Trinket (Soldier); random Unappraised Moon Trinket (General); 200 JP; 50,000 EXP |
| 50300003 | The Lost Order | 失われた秩序 | 55 | Pink Dye ×1; Blood Orb ×500; random Disordering Drop; random Water of Chaos; random Unappraised Moon Trinket (Soldier/General); 300 JP; 85,000 EXP |
| 50300004 | Battle for Gritten Fort: Recapture Battle | グリッテン砦攻防戦 戦況：奪回戦 | 60 and 90 | High Orb ×100; Blood Orb ×1,000; random Unidentified Dragon Trinket (one per job: Alchemist, Elemental Archer, Fighter, High Scepter, Hunter, Priest, Seeker, Shield Sage, Sorcerer, Spirit Lancer, Warrior); random Bonus Dungeon Ticket (Gold/Rare/Bronze tiers); random Unappraised Flight/Snow Trinket (King) or Superior Gala Extract/Healing Potion; random Merit Medal (Lion), 10–50; random Unappraised Moon Trinket (Soldier/General); random Silver Ticket, 60/90/120; random Red Dye, Quality Defense Upgrade Rock, White Dragon Defense Upgrade Rock, White Wings Radiant Crystal, or Blessed Lestalite |
| 50300005 | The Crucible of Demons | 魔物のるつぼ | 60 | Random material tiers including: Healing Elixir, Blue Dye, Superior Quality Gala Extract, Dragon Bone, Large Dragon Bone, Scale Dust, Shining Dragon Bone, Large-Grained Sand, Ancient Scale Dust, Lestalite, High Lestalite, Thick Dragonscale, Sacred Tree Ring (exact tier awarded depends on performance) |
| 50300006 | The Dazzling Gold | 眩き黄金 | 60 | Yellow Dye ×1; Blood Orb ×1,000; random Golden Wedge; random Gold Ritual Instrument; random Unappraised Moon Trinket (Soldier/General); 1,500 JP; 100,000 EXP |
| 50300010 | The Dragon Awakened | 呼び覚まされし竜 | 80 | Blood Orb ×1,500; random Blue Opal; random Blue Skies Blue Opal; random Sky Dragon's Blue Opal; 2,500 JP; 200,000 EXP |

## Later/high-level Extreme Missions, confirmed rewards

| Quest ID | Mission (EN) | Japanese name | Rec. Level | Min. Item Rank | Confirmed rewards |
|---|---|---|---:|---:|---|
| 50303000 | Flames of Darkness: Restricted Stage | 暗晦の炎 限界域 | 95 | 110 | Cyclops Lantern (Gold) ×1; random Burning Magma Lump; random Crest of Herculean Power (Burning) or Crest of Herculean Power (Burning)+; random Crest of Superior Magick (Burning) or Crest of Superior Magick (Burning)+ |
| 50400004 | Arisen of the Black Darkness | 黒き闇の覚者 | 100 | 0 | Black and Silver Chaos Metal ×1; Otherworldly Drop ×1 |
| 50400008 | The Great Dragon Crystal War: The Resisting Land | 大竜晶破壊戦：抗う大地 | — | — | Keystone To Ruin ×2 |

This confirms the previously-reported "滅びへ向かう鍵石" reward for the Great Dragon Crystal War line — its actual in-game English name is **Keystone To Ruin**.

## Missions listed in the project's own quest catalog but without confirmed reward data here

`docs\quests\endgame_content.md` in the repository lists many additional EM quest IDs that either aren't marked "Implemented" (✓) in that document, or don't yet have a quest JSON/CSX file with reward data in this codebase snapshot. Their names and quest IDs are confirmed; their exact reward tables are not:

**4-player, always available:**
- 50301001 — Breakneck Destruction: Dacreim Fortress (速壊の極み ダクレイム砦), Lv85
- 50400000 — Gatekeeper of the Dark World (黒界の門番), Lv100
- 50400001 — High Difficulty: Gatekeeper of the Dark World (【高難度】黒界の門番), Lv100
- 50400002 — Blue Shadow Dancing in Revelry (狂宴に舞う蒼影), Lv100
- 50400003 — High Difficulty: Blue Shadow Dancing in Revelry (【高難度】狂宴に舞う蒼影), Lv100
- 50400005 — High Difficulty: Arisen of the Black Darkness (【高難度】黒き闇の覚者), Lv100
- 50400006 — Power that Destroys Reason (理を破壊する力), Lv100
- 50400007 — High Difficulty: Power that Destroys Reason (【高難度】理を破壊する力), Lv100
- 50400013 — Demon Forces Thundering Through the Fortress (砦に轟く戦鬼連合), Lv105
- 50400014 — The Black Martyr (黒の殉教者), Lv110
- 50400015 — Legend of Lestania (レジェンドオブレスタニア), Lv149
- 50302001 — Breakneck Speed: Jifule Fortress (速移の極み ジフール砦)
- 50400009 — The Great Dragon Crystal War: Captured Palace (大竜晶破壊戦：奪われた王宮), chain quest
- 50400010 — The Great Dragon Crystal War: Castle with No Lord (大竜晶破壊戦：主無き古城), chain quest
- 50400011 — The Great Dragon Crystal War: The Sacrificed Capital (大竜晶破壊戦：捧げられし廃都), chain quest

**4-player, limited-time:**
- 50101021 — Ghosts 'n Goblins Collaboration: Special Mission, Lv60 (2017.09.01–2017.10.05)
- 50105000 — Lestania War Chronicles (レスタニア追懐戦記), Lv60 (2016.06.09–2016.06.30)
- 50201001 — Agent of Corruption: Restricted Stage (歪みの執行人・限界域), Lv65 (2016.09.08–2016.09.21)
- 50209000 — 1st Anniversary: White Dragon Cup, Lv65/IR20 (2016.08.04–2016.08.18)
- 50202002 — Battle Competition: Dragon Battle (戦技闘会・竜伐戦), Lv70 (2016.11.17–2016.12.01)
- 50202001 — Phantasmic Great Dragon: Restricted Stage (淀みし大竜力・限界域), Lv70 (2016.11.24–2016.12.15)
- 50203001 — Earth's Fury: Restricted Stage (大地の怒り・限界域), Lv75 (2017.02.23–2017.03.16)
- 50204000 — Recurrence of Darkness (闇の再動), Lv80 (2017.06.15–2017.08.17)
- 50301000 — Enfilade of Despair and Tragedy: Restricted Stage, Lv85/IR90 (2017.11.30–2017.12.14)
- 50302000 — The Inviting Eye: Restricted Stage, Lv90/IR100 (2018.03.29–2018.04.12)
- 50209001 — 2nd Anniversary: White Dragon Cup
- 50309000 — 3rd Anniversary: White Dragon Cup, Lv95 (2018.08.30–2018.09.06)
- 50309001 — 3rd Anniversary: Breakneck Speed

**8-player, always available:**
- 50300000 — Battle for Gritten Fort: Fierce Battle, Lv40
- 50300002 — Battle for Gritten Fort: Mystery Battle, Lv50
- 50300007 — The Demon of Darkness Awakens (目覚めし闇の魔物), Lv65
- 50300008 — Bloodbane Isle's Feast of Madness (魔赤島の狂宴), Lv70
- 50300009 — The Deathly Battle of the Ancient Temple (古代神殿の死闘), Lv75
- 50400012 — Restricted Stage: Power that Destroys Reason (【限界域】理を破壊する力), Lv100

## Classification notes

- EM began as a four-player version of the older Grand Mission-style content.
- Grand Mission content (the 50300xxx quest IDs above) was later integrated into the EM system, which is why both four-player and eight-player IDs share the same numbering block.
- "High Difficulty" (高難度) and "Restricted Stage" (限界域) entries are harder variants of an existing mission rather than separate base missions.
- The "Great Dragon Crystal War" (大竜晶破壊戦) missions are a linked chain rather than independent one-off encounters.

## What is still not confirmed

1. Reward tables for the missions listed in the "without confirmed reward data" section above — their quest JSON/CSX files either don't exist yet in this repository snapshot or don't define rewards there.
2. Whether reward amounts shown here represent the full table or only the first-clear/daily-reset variant — the quest JSON format found here does not distinguish first-clear vs. repeat-clear rewards explicitly, except where "random" reward pools are shown.
3. Drop rates/weights for a few reward slots were not shown above where the JSON only listed a flat list without chance values (these are noted as "random" without percentages).
4. Any missions added to the live game after this repository's current quest data snapshot.

## Sources (all within `C:\Project\DDON`)

- `Arrowgene.Ddon.Shared\Files\Assets\quests\q50101020.json` through `q50400004.json` (see tables above for exact filenames used)
- `Arrowgene.Ddon.Scripts\scripts\quests\exm\q50300004.csx`
- `Arrowgene.Ddon.Scripts\scripts\quests\exm\q50300005.csx`
- `Arrowgene.Ddon.Scripts\scripts\quests\exm\q50400008.csx`
- `Arrowgene.Ddon.Shared\Files\Assets\itemlist.csv`
- `docs\quests\endgame_content.md`
