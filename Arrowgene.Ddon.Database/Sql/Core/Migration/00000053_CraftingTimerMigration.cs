using System;
using System.Data.Common;
using System.IO;
using System.Text;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class CraftingTimerMigration : IMigrationStrategy
    {
        public uint From => 52;
        public uint To => 53;

        private readonly DatabaseSetting DatabaseSetting;

        public CraftingTimerMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string scriptPath = Path.Combine(DatabaseSetting.DatabaseFolder, "Script/migration_crafting_timer_sqlite.sql");
            string script = File.ReadAllText(scriptPath, Encoding.UTF8);
            string adaptedScript = DdonDatabaseBuilder.AdaptSQLiteSchemaTo(DatabaseSetting.Type, script);
            db.Execute(conn, adaptedScript);
            return true;
        }
    }
}