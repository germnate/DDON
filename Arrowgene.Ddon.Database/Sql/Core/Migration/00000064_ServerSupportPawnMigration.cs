using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration;

public sealed class ServerSupportPawnMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
{
    public uint From => 64;
    public uint To => 65;

    public bool Migrate(IDatabase db, DbConnection conn)
    {
        string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(
            databaseSetting,
            "Script/migration_server_support_pawns.sql");
        db.Execute(conn, adaptedSchema);
        return true;
    }
}
