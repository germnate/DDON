using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private readonly string SqlInsertWorldQuestFirstClear =
        "INSERT INTO \"ddon_world_quest_period_first_clear\" (\"character_common_id\", \"quest_schedule_id\") VALUES (@character_common_id, @quest_schedule_id) ON CONFLICT DO NOTHING;";

    private readonly string SqlHasWorldQuestFirstClear =
        "SELECT COUNT(*) FROM \"ddon_world_quest_period_first_clear\" WHERE \"character_common_id\" = @character_common_id AND \"quest_schedule_id\" = @quest_schedule_id;";

    private readonly string SqlSelectWorldQuestFirstClears =
        "SELECT \"quest_schedule_id\" FROM \"ddon_world_quest_period_first_clear\" WHERE \"character_common_id\" = @character_common_id;";

    private readonly string SqlDeleteAllWorldQuestFirstClears =
        "DELETE FROM \"ddon_world_quest_period_first_clear\";";

    public override bool InsertWorldQuestFirstClear(uint commonId, uint questScheduleId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertWorldQuestFirstClear, command =>
            {
                AddParameter(command, "@character_common_id", commonId);
                AddParameter(command, "@quest_schedule_id", questScheduleId);
            }) >= 0;
        });
    }

    public override bool HasWorldQuestFirstClear(uint commonId, uint questScheduleId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            long count = 0;
            ExecuteReader(connection, SqlHasWorldQuestFirstClear,
                command =>
                {
                    AddParameter(command, "@character_common_id", commonId);
                    AddParameter(command, "@quest_schedule_id", questScheduleId);
                },
                reader =>
                {
                    if (reader.Read())
                        count = reader.GetInt64(0);
                });
            return count > 0;
        });
    }

    public override bool DeleteAllWorldQuestFirstClears(DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlDeleteAllWorldQuestFirstClears, _ => { }) >= 0;
        });
    }

    public override HashSet<uint> SelectWorldQuestFirstClears(uint commonId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            var result = new HashSet<uint>();
            ExecuteReader(connection, SqlSelectWorldQuestFirstClears,
                command => { AddParameter(command, "@character_common_id", commonId); },
                reader =>
                {
                    while (reader.Read())
                        result.Add(GetUInt32(reader, "quest_schedule_id"));
                });
            return result;
        });
    }
}
