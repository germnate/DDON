using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_pawn_expedition */
    protected static readonly string[] PawnExpeditionFields = new[]
    {
        "character_id", "status", "area_id", "spot_id", "is_hot_spot", "is_golden_sally", "sally_count", "sally_start_time"
    };

    private readonly string SqlSelectPawnExpeditionRecord =
        $"SELECT {BuildQueryField(PawnExpeditionFields)} FROM \"ddon_pawn_expedition\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlSelectPawnExpeditionRecordsForClanMembers =
        $"SELECT {BuildQueryField(PawnExpeditionFields)} FROM \"ddon_pawn_expedition\" WHERE \"character_id\" IN ({{0}});";

    private readonly string SqlUpsertPawnExpeditionRecord =
        $"INSERT INTO \"ddon_pawn_expedition\" ({BuildQueryField(PawnExpeditionFields)}) VALUES ({BuildQueryInsert(PawnExpeditionFields)}) " +
        $"ON CONFLICT(\"character_id\") DO UPDATE SET {BuildQueryUpdate(new[] { "status", "area_id", "spot_id", "is_hot_spot", "is_golden_sally", "sally_count", "sally_start_time" })};";

    /* ddon_pawn_expedition_reward_box */
    protected static readonly string[] PawnExpeditionRewardBoxFields = new[]
    {
        "character_id", "mdl_type", "claimed"
    };

    private readonly string SqlInsertPawnExpeditionRewardBox =
        $"INSERT INTO \"ddon_pawn_expedition_reward_box\" ({BuildQueryField(PawnExpeditionRewardBoxFields)}) VALUES ({BuildQueryInsert(PawnExpeditionRewardBoxFields)});";

    private readonly string SqlSelectLastPawnExpeditionRewardBoxId =
        "SELECT MAX(\"box_id\") AS \"box_id\" FROM \"ddon_pawn_expedition_reward_box\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlSelectPawnExpeditionRewardBoxes =
        "SELECT \"box_id\", \"character_id\", \"mdl_type\", \"claimed\" FROM \"ddon_pawn_expedition_reward_box\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlSelectPawnExpeditionRewardBox =
        "SELECT \"box_id\", \"character_id\", \"mdl_type\", \"claimed\" FROM \"ddon_pawn_expedition_reward_box\" WHERE \"character_id\"=@character_id AND \"box_id\"=@box_id;";

    private readonly string SqlSetPawnExpeditionRewardBoxClaimed =
        "UPDATE \"ddon_pawn_expedition_reward_box\" SET \"claimed\"=@claimed WHERE \"box_id\"=@box_id;";

    private readonly string SqlDeleteAllPawnExpeditionRewardBoxes =
        "DELETE FROM \"ddon_pawn_expedition_reward_box\" WHERE \"character_id\"=@character_id;";

    /* ddon_pawn_expedition_reward_box_item */
    protected static readonly string[] PawnExpeditionRewardBoxItemFields = new[]
    {
        "box_id", "slot_no", "item_id", "item_num", "quality", "is_hidden", "claimed"
    };

    private readonly string SqlInsertPawnExpeditionRewardBoxItem =
        $"INSERT INTO \"ddon_pawn_expedition_reward_box_item\" ({BuildQueryField(PawnExpeditionRewardBoxItemFields)}) VALUES ({BuildQueryInsert(PawnExpeditionRewardBoxItemFields)});";

    private readonly string SqlSelectPawnExpeditionRewardBoxItems =
        $"SELECT {BuildQueryField(PawnExpeditionRewardBoxItemFields)} FROM \"ddon_pawn_expedition_reward_box_item\" WHERE \"box_id\"=@box_id;";

    private readonly string SqlClaimPawnExpeditionRewardBoxItem =
        "UPDATE \"ddon_pawn_expedition_reward_box_item\" SET \"claimed\"=1 WHERE \"box_id\"=@box_id AND \"slot_no\"=@slot_no;";

    public override PawnExpeditionRecord? GetPawnExpeditionRecord(uint characterId, DbConnection? connectionIn = null)
    {
        PawnExpeditionRecord? result = null;
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectPawnExpeditionRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
            }, reader =>
            {
                if (reader.Read())
                {
                    result = ReadPawnExpeditionRecord(reader);
                }
            });
        });
        return result;
    }

    public override List<PawnExpeditionRecord> GetPawnExpeditionRecordsForClanMembers(List<uint> characterIds, DbConnection? connectionIn = null)
    {
        List<PawnExpeditionRecord> results = new();
        if (characterIds.Count == 0)
        {
            return results;
        }

        ExecuteQuerySafe(connectionIn, connection =>
        {
            string parameterNames = string.Join(",", characterIds.Select((_, i) => $"@character_id_{i}"));
            string sql = string.Format(SqlSelectPawnExpeditionRecordsForClanMembers, parameterNames);
            ExecuteReader(connection, sql, command =>
            {
                for (int i = 0; i < characterIds.Count; i++)
                {
                    AddParameter(command, $"character_id_{i}", characterIds[i]);
                }
            }, reader =>
            {
                while (reader.Read())
                {
                    results.Add(ReadPawnExpeditionRecord(reader));
                }
            });
        });
        return results;
    }

    private PawnExpeditionRecord ReadPawnExpeditionRecord(DbDataReader reader)
    {
        return new PawnExpeditionRecord()
        {
            CharacterId = GetUInt32(reader, "character_id"),
            Status = (PawnExpeditionStatus)GetByte(reader, "status"),
            AreaId = GetUInt32(reader, "area_id"),
            SpotId = GetUInt32(reader, "spot_id"),
            IsHotSpot = GetBoolean(reader, "is_hot_spot"),
            IsGoldenSally = GetBoolean(reader, "is_golden_sally"),
            SallyCount = GetByte(reader, "sally_count"),
            SallyStartTime = GetDateTimeNullable(reader, "sally_start_time")
        };
    }

    public override bool UpsertPawnExpeditionRecord(PawnExpeditionRecord record, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpsertPawnExpeditionRecord, command =>
            {
                AddParameter(command, "character_id", record.CharacterId);
                AddParameter(command, "status", (byte)record.Status);
                AddParameter(command, "area_id", record.AreaId);
                AddParameter(command, "spot_id", record.SpotId);
                AddParameter(command, "is_hot_spot", record.IsHotSpot);
                AddParameter(command, "is_golden_sally", record.IsGoldenSally);
                AddParameter(command, "sally_count", record.SallyCount);
                AddParameter(command, "sally_start_time", record.SallyStartTime);
            }) > 0;
        });
    }

    public override uint InsertPawnExpeditionRewardBox(uint characterId, byte mdlType, DbConnection? connectionIn = null)
    {
        uint boxId = 0;
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteNonQuery(connection, SqlInsertPawnExpeditionRewardBox, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "mdl_type", mdlType);
                AddParameter(command, "claimed", false);
            });

            ExecuteReader(connection, SqlSelectLastPawnExpeditionRewardBoxId, command =>
            {
                AddParameter(command, "character_id", characterId);
            }, reader =>
            {
                if (reader.Read())
                {
                    boxId = GetUInt32(reader, "box_id");
                }
            });
        });
        return boxId;
    }

    public override bool InsertPawnExpeditionRewardBoxItem(PawnExpeditionRewardBoxItem item, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertPawnExpeditionRewardBoxItem, command =>
            {
                AddParameter(command, "box_id", item.BoxId);
                AddParameter(command, "slot_no", item.SlotNo);
                AddParameter(command, "item_id", item.ItemId);
                AddParameter(command, "item_num", item.ItemNum);
                AddParameter(command, "quality", item.Quality);
                AddParameter(command, "is_hidden", item.IsHidden);
                AddParameter(command, "claimed", item.Claimed);
            }) == 1;
        });
    }

    public override List<PawnExpeditionRewardBox> GetPawnExpeditionRewardBoxes(uint characterId, DbConnection? connectionIn = null)
    {
        List<PawnExpeditionRewardBox> boxes = new();
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectPawnExpeditionRewardBoxes, command =>
            {
                AddParameter(command, "character_id", characterId);
            }, reader =>
            {
                while (reader.Read())
                {
                    boxes.Add(new PawnExpeditionRewardBox()
                    {
                        BoxId = GetUInt32(reader, "box_id"),
                        CharacterId = GetUInt32(reader, "character_id"),
                        MdlType = GetByte(reader, "mdl_type"),
                        Claimed = GetBoolean(reader, "claimed")
                    });
                }
            });

            foreach (PawnExpeditionRewardBox box in boxes)
            {
                box.Items = SelectPawnExpeditionRewardBoxItems(connection, box.BoxId);
            }
        });
        return boxes;
    }

    public override PawnExpeditionRewardBox? GetPawnExpeditionRewardBox(uint characterId, uint boxId, DbConnection? connectionIn = null)
    {
        PawnExpeditionRewardBox? box = null;
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectPawnExpeditionRewardBox, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "box_id", boxId);
            }, reader =>
            {
                if (reader.Read())
                {
                    box = new PawnExpeditionRewardBox()
                    {
                        BoxId = GetUInt32(reader, "box_id"),
                        CharacterId = GetUInt32(reader, "character_id"),
                        MdlType = GetByte(reader, "mdl_type"),
                        Claimed = GetBoolean(reader, "claimed")
                    };
                }
            });

            if (box != null)
            {
                box.Items = SelectPawnExpeditionRewardBoxItems(connection, box.BoxId);
            }
        });
        return box;
    }

    private List<PawnExpeditionRewardBoxItem> SelectPawnExpeditionRewardBoxItems(DbConnection connection, uint boxId)
    {
        List<PawnExpeditionRewardBoxItem> items = new();
        ExecuteReader(connection, SqlSelectPawnExpeditionRewardBoxItems, command =>
        {
            AddParameter(command, "box_id", boxId);
        }, reader =>
        {
            while (reader.Read())
            {
                items.Add(new PawnExpeditionRewardBoxItem()
                {
                    BoxId = GetUInt32(reader, "box_id"),
                    SlotNo = GetUInt32(reader, "slot_no"),
                    ItemId = GetUInt32(reader, "item_id"),
                    ItemNum = GetUInt32(reader, "item_num"),
                    Quality = GetUInt32(reader, "quality"),
                    IsHidden = GetBoolean(reader, "is_hidden"),
                    Claimed = GetBoolean(reader, "claimed")
                });
            }
        });
        return items;
    }

    public override bool ClaimPawnExpeditionRewardBoxItem(uint boxId, uint slotNo, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlClaimPawnExpeditionRewardBoxItem, command =>
            {
                AddParameter(command, "box_id", boxId);
                AddParameter(command, "slot_no", slotNo);
            }) == 1;
        });
    }

    public override bool SetPawnExpeditionRewardBoxClaimed(uint boxId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlSetPawnExpeditionRewardBoxClaimed, command =>
            {
                AddParameter(command, "box_id", boxId);
                AddParameter(command, "claimed", true);
            }) == 1;
        });
    }

    public override void DeleteAllPawnExpeditionRewardBoxes(uint characterId, DbConnection? connectionIn = null)
    {
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteNonQuery(connection, SqlDeleteAllPawnExpeditionRewardBoxes, command =>
            {
                AddParameter(command, "character_id", characterId);
            });
        });
    }
}
