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

-- Without these two rows, loading the "Server" character (id 40) reads a NULL
-- sex/hp via the LEFT JOINs in the character-loading query, which throws
-- ("column sex is null") whenever the server support pawn is hired, and left
-- the pawn's own HP looking like 0 in the meantime.
INSERT INTO ddon_edit_info (
  character_common_id,
  sex, voice, voice_pitch, personality, speech_freq, body_type, hair, beard, makeup, scar,
  eye_preset_no, nose_preset_no, mouth_preset_no, eyebrow_tex_no, color_skin, color_hair,
  color_beard, color_eyebrow, color_r_eye, color_l_eye, color_makeup, sokutobu, hitai,
  mimi_jyouge, kannkaku, mabisasi_jyouge, hanakuchi_jyouge, ago_saki_haba, ago_zengo,
  ago_saki_jyouge, hitomi_ookisa, me_ookisa, me_kaiten, mayu_kaiten, mimi_ookisa, mimi_muki,
  elf_mimi, miken_takasa, miken_haba, hohobone_ryou, hohobone_jyouge, hohoniku, erahone_jyouge,
  erahone_haba, hana_jyouge, hana_haba, hana_takasa, hana_kakudo, kuchi_haba, kuchi_atsusa,
  eyebrow_uv_offset_x, eyebrow_uv_offset_y, wrinkle, wrinkle_albedo_blend_rate,
  wrinkle_detail_normal_power, muscle_albedo_blend_rate, muscle_detail_normal_power, height,
  head_size, neck_offset, neck_scale, upper_body_scale_x, belly_size, teat_scale, tekubi_size,
  koshi_offset, koshi_size, ankle_offset, fat, muscle, motion_filter
)
SELECT
  character_common_id,
  1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
  0, 0, 0, 0, 0, 0,
  0, 0, 0, 0, 0, 0, 0,
  0, 0, 0, 0, 0, 0,
  0, 0, 0, 0, 0, 0, 0,
  0, 0, 0, 0, 0, 0, 0,
  0, 0, 0, 0, 0, 0, 0,
  0, 0, 0, 0,
  0, 0, 0, 0,
  0, 0, 0, 0, 0, 0, 0,
  0, 0, 0, 0, 0, 0
FROM ddon_character
WHERE character_id = 40
ON CONFLICT (character_common_id) DO NOTHING;

INSERT INTO ddon_status_info (character_common_id, revive_point, hp, white_hp)
SELECT character_common_id, 3, 760, 760
FROM ddon_character
WHERE character_id = 40
ON CONFLICT (character_common_id) DO NOTHING;

COMMIT;

INSERT INTO ddon_server_support_pawn (pawn_id, required_quest_id, enabled)
SELECT pawn_id, 0, TRUE
FROM ddon_pawn
WHERE character_id = 40
  AND name = 'ServerPawn'
ON CONFLICT (pawn_id) DO UPDATE SET
  required_quest_id = EXCLUDED.required_quest_id,
  enabled = EXCLUDED.enabled;
