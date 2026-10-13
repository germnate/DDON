using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text.Json;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlSelectPawnClonesByCharacterId = """
        SELECT "ddon_character_pawn_clone"."character_id",
               "ddon_character_pawn_clone"."slot_no",
               "ddon_character_pawn_clone"."pawn_id",
               "ddon_character_pawn_clone"."data",
               "ddon_character_pawn_clone"."data_size"
        FROM "ddon_character_pawn_clone"
        WHERE "ddon_character_pawn_clone"."character_id" = @character_id
        ORDER BY "ddon_character_pawn_clone"."slot_no";
        """;

    private const string SqlSelectPawnCloneByPawnId = """
        SELECT "ddon_character_pawn_clone"."character_id",
               "ddon_character_pawn_clone"."slot_no",
               "ddon_character_pawn_clone"."pawn_id",
               "ddon_character_pawn_clone"."data",
               "ddon_character_pawn_clone"."data_size"
        FROM "ddon_character_pawn_clone"
        WHERE "ddon_character_pawn_clone"."pawn_id" = @pawn_id;
        """;

    private const string SqlSelectPawnCloneSlot = """
        SELECT "ddon_character_pawn_clone"."pawn_id",
               "ddon_pawn"."character_common_id"
        FROM "ddon_character_pawn_clone"
        INNER JOIN "ddon_pawn"
            ON "ddon_pawn"."pawn_id" = "ddon_character_pawn_clone"."pawn_id"
        WHERE "ddon_character_pawn_clone"."character_id" = @character_id
          AND "ddon_character_pawn_clone"."slot_no" = @slot_no;
        """;

    private const string SqlInsertPawnClone = """
        INSERT INTO "ddon_character_pawn_clone"
            ("character_id", "slot_no", "pawn_id", "data", "data_size")
        VALUES
            (@character_id, @slot_no, @pawn_id, @data, @data_size)
        ON CONFLICT ("character_id", "slot_no") DO UPDATE SET
            "pawn_id" = excluded."pawn_id",
            "data" = excluded."data",
            "data_size" = excluded."data_size";
        """;

    public override List<PawnClone> SelectPawnClones(uint characterId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            List<PawnClone> clones = [];
            ExecuteReader(
                connection,
                SqlSelectPawnClonesByCharacterId,
                command => AddParameter(command, "@character_id", characterId),
                reader =>
                {
                    while (reader.Read())
                    {
                        clones.Add(ReadPawnClone(reader));
                    }
                });
            return clones;
        });
    }

    public override PawnClone? SelectPawnClone(uint pawnId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            PawnClone? clone = null;
            ExecuteReader(
                connection,
                SqlSelectPawnCloneByPawnId,
                command => AddParameter(command, "@pawn_id", pawnId),
                reader =>
                {
                    if (reader.Read())
                    {
                        clone = ReadPawnClone(reader);
                    }
                });
            return clone;
        });
    }

    public override PawnClone SavePawnClone(uint characterId, byte slot, Pawn pawn, RentalPawnRecord record)
    {
        if (slot is < 1 or > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(slot), slot, "Pawn clone slots must be between 1 and 10.");
        }

        PawnClone? savedClone = null;
        ExecuteInTransaction(connection =>
        {
            uint existingPawnId = 0;
            uint existingCommonId = 0;
            ExecuteReader(
                connection,
                SqlSelectPawnCloneSlot,
                command =>
                {
                    AddParameter(command, "@character_id", characterId);
                    AddParameter(command, "@slot_no", slot);
                },
                reader =>
                {
                    if (reader.Read())
                    {
                        existingPawnId = GetUInt32(reader, "pawn_id");
                        existingCommonId = GetUInt32(reader, "character_common_id");
                    }
                });

            pawn.CharacterId = characterId;
            if (existingPawnId != 0)
            {
                pawn.PawnId = existingPawnId;
                pawn.CommonId = existingCommonId;
                UpdatePawnCloneBacking(connection, pawn);
            }
            else
            {
                CreatePawnInTransaction(connection, pawn);
            }

            record.CharacterId = characterId;
            record.PawnId = pawn.PawnId;
            record.CommonId = pawn.CommonId;
            byte[] data = Util.CompressJSON(JsonSerializer.Serialize(record));

            int savedRows = ExecuteNonQuery(
                connection,
                SqlInsertPawnClone,
                command =>
                {
                    AddParameter(command, "@character_id", characterId);
                    AddParameter(command, "@slot_no", slot);
                    AddParameter(command, "@pawn_id", pawn.PawnId);
                    AddParameter(command, "@data", data);
                    AddParameter(command, "@data_size", data.Length);
                });
            if (savedRows != 1)
            {
                throw new InvalidOperationException($"Could not save pawn clone slot {slot} for character {characterId}.");
            }

            savedClone = new PawnClone
            {
                CharacterId = characterId,
                Slot = slot,
                PawnId = pawn.PawnId,
                Record = record
            };
        });

        return savedClone ?? throw new InvalidOperationException("The pawn clone was not saved.");
    }

    private void UpdatePawnCloneBacking(DbConnection connection, Pawn pawn)
    {
        int updatedPawnRows = ExecuteNonQuery(
            connection,
            SqlUpdatePawn,
            command => AddParameter(command, pawn));
        if (updatedPawnRows != 1)
        {
            throw new InvalidOperationException($"Could not update pawn clone {pawn.PawnId}.");
        }

        if (!UpdateCharacterCommonBaseInfo(pawn, connection)
            || !UpdateEditInfo(connection, pawn)
            || !UpdateStatusInfo(connection, pawn)
            || !UpdateCharacterProfile(pawn, connection))
        {
            throw new InvalidOperationException($"Could not update pawn clone character data for pawn {pawn.PawnId}.");
        }

        foreach (var jobData in pawn.CharacterJobDataList)
        {
            if (!ReplaceCharacterJobData(pawn.CommonId, jobData, connection))
            {
                throw new InvalidOperationException($"Could not update pawn clone vocation data for pawn {pawn.PawnId}.");
            }
        }
    }

    private PawnClone ReadPawnClone(DbDataReader reader)
    {
        uint characterId = GetUInt32(reader, "character_id");
        byte slot = GetByte(reader, "slot_no");
        uint pawnId = GetUInt32(reader, "pawn_id");
        int dataSize = GetInt32(reader, "data_size");
        byte[] data = GetBytes(reader, "data", dataSize);
        string json = Util.DecompressJSON(data);
        RentalPawnRecord record = JsonSerializer.Deserialize<RentalPawnRecord>(json)
            ?? throw new InvalidDataException($"Pawn clone {pawnId} contains invalid snapshot data.");

        return new PawnClone
        {
            CharacterId = characterId,
            Slot = slot,
            PawnId = pawnId,
            Record = record
        };
    }
}
