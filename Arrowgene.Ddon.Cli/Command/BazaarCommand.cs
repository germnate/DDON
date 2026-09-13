using Arrowgene.Ddon.Cli;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;
using System;
using System.IO;

namespace Arrowgene.Ddon.Cli.Command
{
    public class BazaarCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(BazaarCommand));

        public string Key => "bazaar";

        public string Description => $"Bazaar stock maintenance. Ex.:{Environment.NewLine}" +
                                     $"bazaar rotate";

        public CommandResultType Run(CommandParameter parameter)
        {
            if (!parameter.Arguments.Contains("rotate"))
            {
                Logger.Error($"Command: '{Key}' expects the 'rotate' argument. Type 'help {Key}' for usage.");
                return CommandResultType.Continue;
            }

            string settingArgument = parameter.SwitchMap.TryGetValue("--config", out string configPath)
                ? configPath
                : "Files/Arrowgene.Ddon.config.json";
            string settingPath = Path.Combine(Util.ExecutingDirectory(), settingArgument);
            Setting settings = Setting.LoadFromFile(settingPath);
            if (settings == null)
            {
                Logger.Error($"Could not load settings from '{settingPath}'.");
                return CommandResultType.Exit;
            }

            settings.DatabaseSetting ??= new DatabaseSetting();

            IDatabase database = DdonDatabaseBuilder.Build(settings.DatabaseSetting);
            if (database == null)
            {
                Logger.Error("Could not open the configured database.");
                return CommandResultType.Exit;
            }

            try
            {
                uint currentDatabaseVersion = database.GetMeta().DatabaseVersion;
                if (currentDatabaseVersion != DdonDatabaseBuilder.Version)
                {
                    Logger.Error(
                        $"Database version is {currentDatabaseVersion}. Please update the database to version {DdonDatabaseBuilder.Version}.");
                    return CommandResultType.Exit;
                }

                AssetRepository assetRepository = new(settings.AssetPath);
                assetRepository.Initialize();

                ServerScriptManager serverScriptManager = new(settings.AssetPath);
                serverScriptManager.Initialize();

                DdonGameServer gameServer = new(settings.GameServerSetting,
                    serverScriptManager.GameServerSettingsModule.GameSettings, database, assetRepository);

                Logger.Info("Rotating bazaar stock.");
                gameServer.BazaarManager.RotateGeneratedStock();
                Logger.Info("Bazaar stock rotation completed.");
            }
            finally
            {
                database.Stop();
            }

            return CommandResultType.Exit;
        }

        public void Shutdown()
        {
        }
    }
}