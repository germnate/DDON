#!/usr/bin/env node
import fs from 'node:fs';
import path from 'node:path';

const repoRoot = process.cwd();
const gatheringSpotInfoPath = path.join(repoRoot, 'Arrowgene.Ddon.Shared/Files/Assets/GatheringSpotInfo.json');
const itemListPath = path.join(repoRoot, 'Arrowgene.Ddon.Shared/Files/Assets/itemlist.csv');
const questLootRangesPath = path.join(repoRoot, 'Arrowgene.Ddon.Shared/Files/Assets/QuestLootRanges.json');
const reportPath = path.join(repoRoot, 'docs/quests/locked_chest_audit_report.md');
const nonDroppableCsvPath = path.join(repoRoot, 'docs/quests/locked_chest_non_droppable_lanes.csv');

const lockedGatheringTypes = new Set([12, 13, 14, 35]);
const treasureGatheringTypes = new Set([15, 31, 32, 33, 34]);
const treasureChestUnitIds = new Set([
  513050, 513051, 513052, 513053, 513054, 513055, 513056, 513060, 513061, 523241, 523242,
  513130, 513133, 513134, 523907, 523908
]);

function parseCsv(csvText) {
  const lines = csvText.replace(/\r/g, '').split('\n').filter(Boolean);
  const header = parseCsvLine(lines[0]);
  return lines.slice(1).map((line) => {
    const cols = parseCsvLine(line);
    const row = {};
    for (let i = 0; i < header.length; i += 1) {
      row[header[i]] = cols[i] ?? '';
    }
    return row;
  });
}

function parseCsvLine(line) {
  const values = [];
  let current = '';
  let inQuotes = false;

  for (let i = 0; i < line.length; i += 1) {
    const ch = line[i];
    if (ch === '"') {
      if (inQuotes && line[i + 1] === '"') {
        current += '"';
        i += 1;
      } else {
        inQuotes = !inQuotes;
      }
      continue;
    }

    if (ch === ',' && !inQuotes) {
      values.push(current);
      current = '';
      continue;
    }

    current += ch;
  }

  values.push(current);
  return values;
}

function toByte(value) {
  if (value === undefined || value === null || value === '') {
    return null;
  }
  const n = Number(value);
  return Number.isFinite(n) ? n : null;
}

function laneKey(level, subCategory, jobs) {
  return `${level}|${subCategory}|${jobs ?? ''}`;
}

function summarizeEquipmentWindow(equipmentItems, topRankByLane, minLevel, maxLevel) {
  const candidates = equipmentItems.filter((item) => item.level >= minLevel && item.level <= maxLevel);
  const droppable = candidates.filter((item) => {
    const lane = laneKey(item.level, item.subCategory, item.jobs);
    const topRank = topRankByLane.get(lane);
    return topRank === undefined || item.rank < topRank;
  });

  const preRanks = [...new Set(candidates.map((item) => item.rank))].sort((a, b) => a - b);
  const postRanks = [...new Set(droppable.map((item) => item.rank))].sort((a, b) => a - b);
  const laneCount = new Set(candidates.map((item) => laneKey(item.level, item.subCategory, item.jobs))).size;
  const laneCountDroppable = new Set(droppable.map((item) => laneKey(item.level, item.subCategory, item.jobs))).size;

  return {
    candidateCount: candidates.length,
    droppableCount: droppable.length,
    laneCount,
    laneCountDroppable,
    filteredOutCount: candidates.length - droppable.length,
    preMinRank: preRanks[0] ?? null,
    preMaxRank: preRanks[preRanks.length - 1] ?? null,
    postMinRank: postRanks[0] ?? null,
    postMaxRank: postRanks[postRanks.length - 1] ?? null,
    percentRemoved: candidates.length === 0 ? 0 : Number((((candidates.length - droppable.length) / candidates.length) * 100).toFixed(2)),
  };
}

function main() {
  const gatheringSpotInfo = JSON.parse(fs.readFileSync(gatheringSpotInfoPath, 'utf8'));
  const itemRows = parseCsv(fs.readFileSync(itemListPath, 'utf8'));
  const questLootRanges = JSON.parse(fs.readFileSync(questLootRangesPath, 'utf8'));

  let totalSpots = 0;
  let lockedSpots = 0;
  let nonLockedTreasureSpots = 0;
  let unresolvedSpots = 0;

  const stageLockedCounts = new Map();

  for (const [stageNo, spots] of Object.entries(gatheringSpotInfo)) {
    for (const spot of spots) {
      totalSpots += 1;

      if (spot.GatheringType === undefined || spot.UnitId === undefined) {
        unresolvedSpots += 1;
        continue;
      }

      const isLocked = lockedGatheringTypes.has(spot.GatheringType);
      const isTreasureLike = treasureGatheringTypes.has(spot.GatheringType) || treasureChestUnitIds.has(spot.UnitId);

      if (isLocked) {
        lockedSpots += 1;
        const key = String(stageNo);
        stageLockedCounts.set(key, (stageLockedCounts.get(key) ?? 0) + 1);
      } else if (isTreasureLike) {
        nonLockedTreasureSpots += 1;
      }
    }
  }

  const equipmentItems = itemRows
    .map((row) => ({
      itemId: Number(row['#ItemId']),
      category: Number(row.Category),
      rank: toByte(row.Rank),
      name: row.Name,
      subCategory: row.Subcategory,
      level: toByte(row.Level),
      jobs: row.Jobs,
    }))
    .filter((row) => row.category === 3 && row.level !== null && row.rank !== null);

  const lanes = new Map();
  const topRankByLane = new Map();
  for (const item of equipmentItems) {
    const key = laneKey(item.level, item.subCategory, item.jobs);
    if (!lanes.has(key)) {
      lanes.set(key, []);
    }
    lanes.get(key).push(item);
  }

  for (const [key, items] of lanes.entries()) {
    const topRank = items.reduce((max, item) => Math.max(max, item.rank), 0);
    topRankByLane.set(key, topRank);
  }

  let totalLanes = 0;
  let lanesWithTopRankReserved = 0;
  let lanesNonDroppable = 0;
  const nonDroppableLanes = [];

  for (const [key, items] of lanes.entries()) {
    totalLanes += 1;
    const ranks = [...new Set(items.map((x) => x.rank))].sort((a, b) => a - b);
    const [level, subCategory, jobs] = key.split('|');

    if (ranks.length <= 1) {
      lanesNonDroppable += 1;
      nonDroppableLanes.push({
        level,
        subCategory,
        jobs,
        topRank: ranks[0] ?? '',
        distinctRanks: ranks.length,
        totalItems: items.length,
        sampleItem: items[0]?.name ?? '',
      });
      continue;
    }

    lanesWithTopRankReserved += 1;
  }

  nonDroppableLanes.sort((a, b) => Number(a.level) - Number(b.level) || Number(a.subCategory) - Number(b.subCategory));

  const topLockedStages = [...stageLockedCounts.entries()]
    .sort((a, b) => b[1] - a[1])
    .slice(0, 20);

  const simulationRows = [];
  for (const range of questLootRanges) {
    const normal = summarizeEquipmentWindow(
      equipmentItems,
      topRankByLane,
      Number(range.NormalItemLevelMin),
      Number(range.NormalItemLevelMax)
    );
    const boss = summarizeEquipmentWindow(
      equipmentItems,
      topRankByLane,
      Number(range.BossItemLevelMin),
      Number(range.BossItemLevelMax)
    );

    simulationRows.push({
      bracket: `${range.MinLevel}-${range.MaxLevel}`,
      normalWindow: `${range.NormalItemLevelMin}-${range.NormalItemLevelMax}`,
      bossWindow: `${range.BossItemLevelMin}-${range.BossItemLevelMax}`,
      normal,
      boss,
    });
  }

  const csvLines = [
    'Level,SubCategory,Jobs,TopRank,DistinctRanks,TotalItems,SampleItem',
    ...nonDroppableLanes.map((lane) => [
      lane.level,
      lane.subCategory,
      lane.jobs,
      lane.topRank,
      lane.distinctRanks,
      lane.totalItems,
      `"${String(lane.sampleItem).replace(/"/g, '""')}"`,
    ].join(',')),
  ];
  fs.writeFileSync(nonDroppableCsvPath, `${csvLines.join('\n')}\n`, 'utf8');

  const report = [
    '# Locked Chest Audit Report',
    '',
    `Generated: ${new Date().toISOString()}`,
    '',
    '## Spot Classification',
    '',
    `- Total spots scanned: ${totalSpots}`,
    `- Locked chest spots (GatheringType key lv1-lv4): ${lockedSpots}`,
    `- Non-locked treasure/chest-like spots: ${nonLockedTreasureSpots}`,
    `- Unresolved metadata spots: ${unresolvedSpots}`,
    '',
    'Top stages by locked chest count (StageNo -> count):',
    ...topLockedStages.map(([stageNo, count]) => `- ${stageNo}: ${count}`),
    '',
    '## Equipment Lane Policy Health',
    '',
    '- Lane key: Level + SubCategory + Jobs',
    `- Total equipment lanes: ${totalLanes}`,
    `- Lanes with droppable pool after top-rank removal: ${lanesWithTopRankReserved}`,
    `- Lanes that collapse (single-rank only, non-droppable): ${lanesNonDroppable}`,
    '',
    `Detailed non-droppable lanes: ${path.relative(repoRoot, nonDroppableCsvPath)}`,
    '',
    '## Window Simulation (Equipment Only)',
    '',
    '- Simulation method: evaluate candidate equipment in each QuestLootRange level window, then apply top-rank reservation per lane.',
    '- Goal check: ensure resulting droppable pool never includes the top rank for any lane.',
    '',
    '| Quest Level Bracket | Normal Window | Normal Candidates | Normal Droppable | Normal Removed | Boss Window | Boss Candidates | Boss Droppable | Boss Removed |',
    '| --- | --- | ---: | ---: | ---: | --- | ---: | ---: | ---: |',
    ...simulationRows.map((row) =>
      `| ${row.bracket} | ${row.normalWindow} | ${row.normal.candidateCount} | ${row.normal.droppableCount} | ${row.normal.percentRemoved}% | ${row.bossWindow} | ${row.boss.candidateCount} | ${row.boss.droppableCount} | ${row.boss.percentRemoved}% |`
    ),
    '',
    'Post-filter rank bounds by bracket:',
    ...simulationRows.map((row) =>
      `- ${row.bracket}: normal rank ${row.normal.postMinRank ?? 'n/a'}-${row.normal.postMaxRank ?? 'n/a'}, boss rank ${row.boss.postMinRank ?? 'n/a'}-${row.boss.postMaxRank ?? 'n/a'}`
    ),
    '',
    '## Notes',
    '',
    '- This audit is data-only and does not execute runtime roll logic.',
    '- Locked chest classification uses GatheringType key levels as the authoritative signal.',
  ].join('\n');

  fs.writeFileSync(reportPath, `${report}\n`, 'utf8');

  console.log(`Wrote report: ${path.relative(repoRoot, reportPath)}`);
  console.log(`Wrote CSV: ${path.relative(repoRoot, nonDroppableCsvPath)}`);
}

main();