CREATE TABLE IF NOT EXISTS "ddon_character_pawn_clone"
(
    "character_id" INTEGER  NOT NULL,
    "slot_no"      SMALLINT NOT NULL CHECK ("slot_no" BETWEEN 1 AND 10),
    "pawn_id"      INTEGER  NOT NULL UNIQUE,
    "data"         BLOB     NOT NULL,
    "data_size"    INTEGER  NOT NULL,
    CONSTRAINT "pk_ddon_character_pawn_clone" PRIMARY KEY ("character_id", "slot_no"),
    CONSTRAINT "fk_ddon_character_pawn_clone_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_character_pawn_clone_pawn_id" FOREIGN KEY ("pawn_id") REFERENCES "ddon_pawn" ("pawn_id") ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS "idx_ddon_character_pawn_clone_pawn_id" ON "ddon_character_pawn_clone" ("pawn_id");
