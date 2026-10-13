using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration;

public class PawnCloneMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
{
    public uint From => 68;
    public uint To => 69;

    public bool Migrate(IDatabase db, DbConnection conn)
    {
        string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/migration_pawn_clone.sql");
        db.Execute(conn, adaptedSchema, true);
        return true;
    }
}
