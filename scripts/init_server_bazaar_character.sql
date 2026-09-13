BEGIN;

INSERT INTO account (
  name,
  normal_name,
  hash,
  mail,
  mail_verified,
  state,
  created
)
SELECT
  'system_bazaar',
  'system_bazaar',
  'system_bazaar_hash',
  'system_bazaar@example.com',
  TRUE,
  0,
  NOW()
WHERE NOT EXISTS (
  SELECT 1
  FROM account
  WHERE name = 'system_bazaar'
);

INSERT INTO ddon_character_common (
  job,
  hide_equip_head,
  hide_equip_lantern
)
SELECT
  0,
  FALSE,
  FALSE
WHERE NOT EXISTS (
  SELECT 1
  FROM ddon_character_common
  WHERE job = 0
    AND hide_equip_head = FALSE
    AND hide_equip_lantern = FALSE
);

INSERT INTO ddon_character (
  character_id,
  character_common_id,
  account_id,
  version,
  first_name,
  last_name,
  created,
  my_pawn_slot_num,
  rental_pawn_slot_num,
  hide_equip_head_pawn,
  hide_equip_lantern_pawn,
  arisen_profile_share_range,
  fav_warp_slot_num,
  max_bazaar_exhibits,
  partner_pawn_id,
  game_mode
)
VALUES (
  40,
  (
    SELECT character_common_id
    FROM ddon_character_common
    WHERE job = 0
      AND hide_equip_head = FALSE
      AND hide_equip_lantern = FALSE
    ORDER BY character_common_id ASC
    LIMIT 1
  ),
  (
    SELECT id
    FROM account
    WHERE name = 'system_bazaar'
    ORDER BY id ASC
    LIMIT 1
  ),
  0,
  'Server',
  '',
  NOW(),
  0,
  0,
  FALSE,
  FALSE,
  0,
  0,
  100,
  0,
  0
)
ON CONFLICT ("character_id") DO UPDATE SET
  character_common_id = EXCLUDED.character_common_id,
  account_id = EXCLUDED.account_id,
  version = EXCLUDED.version,
  first_name = EXCLUDED.first_name,
  last_name = EXCLUDED.last_name,
  created = EXCLUDED.created,
  my_pawn_slot_num = EXCLUDED.my_pawn_slot_num,
  rental_pawn_slot_num = EXCLUDED.rental_pawn_slot_num,
  hide_equip_head_pawn = EXCLUDED.hide_equip_head_pawn,
  hide_equip_lantern_pawn = EXCLUDED.hide_equip_lantern_pawn,
  arisen_profile_share_range = EXCLUDED.arisen_profile_share_range,
  fav_warp_slot_num = EXCLUDED.fav_warp_slot_num,
  max_bazaar_exhibits = EXCLUDED.max_bazaar_exhibits,
  partner_pawn_id = EXCLUDED.partner_pawn_id,
  game_mode = EXCLUDED.game_mode;

COMMIT;
