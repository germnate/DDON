CREATE TABLE IF NOT EXISTS "ddon_server_support_pawn"
(
    "pawn_id"          INTEGER PRIMARY KEY NOT NULL,
    "required_quest_id" INTEGER NOT NULL DEFAULT 0,
    "enabled"          BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT "fk_ddon_server_support_pawn_pawn_id"
        FOREIGN KEY ("pawn_id") REFERENCES "ddon_pawn" ("pawn_id") ON DELETE CASCADE
);
