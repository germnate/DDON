BEGIN;

CREATE TEMP TABLE tmp_bloodbane_sched AS
SELECT quest_schedule_id
FROM ddon_light_quests
WHERE quest_id BETWEEN 40200001 AND 40200040;

DELETE FROM ddon_priority_quests
WHERE quest_schedule_id IN (SELECT quest_schedule_id FROM tmp_bloodbane_sched);

DELETE FROM ddon_quest_progress
WHERE quest_schedule_id IN (SELECT quest_schedule_id FROM tmp_bloodbane_sched);

DELETE FROM ddon_light_quests
WHERE quest_id BETWEEN 40200001 AND 40200040;

COMMIT;

UPDATE ddon_schedule_next SET timestamp = 0 WHERE type = 6;
INSERT INTO ddon_schedule_next(type, timestamp)
SELECT 6, 0
WHERE NOT EXISTS (SELECT 1 FROM ddon_schedule_next WHERE type = 6);
