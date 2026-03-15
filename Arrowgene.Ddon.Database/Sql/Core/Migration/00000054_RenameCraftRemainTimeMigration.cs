using System;
using System.Data.Common;
using System.IO;
using System.Text;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class RenameCraftRemainTimeMigration : IMigrationStrategy
    {
        public uint From => 53;
        public uint To => 54;

        private readonly DatabaseSetting DatabaseSetting;

        public RenameCraftRemainTimeMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string scriptPath = Path.Combine(DatabaseSetting.DatabaseFolder, "Script/migration_rename_craft_remain_time_sqlite.sql");
            string script = File.ReadAllText(scriptPath, Encoding.UTF8);
            string adaptedScript = DdonDatabaseBuilder.AdaptSQLiteSchemaTo(DatabaseSetting.Type, script);
            db.Execute(conn, adaptedScript);
            return true;
        }
    }
}