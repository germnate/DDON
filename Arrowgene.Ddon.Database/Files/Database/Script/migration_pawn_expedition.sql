CREATE TABLE "ddon_pawn_expedition"
(
    "character_id"      INTEGER  NOT NULL,
    "status"             TINYINT  NOT NULL,
    "area_id"            INTEGER  NOT NULL,
    "spot_id"            INTEGER  NOT NULL,
    "is_hot_spot"        BOOLEAN  NOT NULL,
    "is_golden_sally"    BOOLEAN  NOT NULL,
    "sally_count"        TINYINT  NOT NULL,
    "sally_start_time"   DATETIME NULL,
    CONSTRAINT "pk_ddon_pawn_expedition" PRIMARY KEY ("character_id"),
    CONSTRAINT "fk_ddon_pawn_expedition_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_pawn_expedition_reward_box"
(
    "box_id"       INTEGER PRIMARY KEY AUTOINCREMENT,
    "character_id" INTEGER NOT NULL,
    "mdl_type"     TINYINT NOT NULL,
    "claimed"      BOOLEAN NOT NULL,
    CONSTRAINT "fk_ddon_pawn_expedition_reward_box_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_pawn_expedition_reward_box_item"
(
    "box_id"    INTEGER NOT NULL,
    "slot_no"   INTEGER NOT NULL,
    "item_id"   INTEGER NOT NULL,
    "item_num"  INTEGER NOT NULL,
    "quality"   INTEGER NOT NULL,
    "is_hidden" BOOLEAN NOT NULL,
    "claimed"   BOOLEAN NOT NULL,
    CONSTRAINT "pk_ddon_pawn_expedition_reward_box_item" PRIMARY KEY ("box_id", "slot_no"),
    CONSTRAINT "fk_ddon_pawn_expedition_reward_box_item_box_id" FOREIGN KEY ("box_id") REFERENCES "ddon_pawn_expedition_reward_box" ("box_id") ON DELETE CASCADE
);
