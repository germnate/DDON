using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class WorldQuestResetTask : WeeklyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WorldQuestResetTask));

        public WorldQuestResetTask(DayOfWeek day, uint hour, uint minute) : base(TaskType.WorldQuestRotation, day, hour, minute)
        {
        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return server.GameSettings.GameServerSettings.WorldQuestSystem == WorldQuestSystemMode.ServerReset;
        }

        public override void RunTask(DdonGameServer server)
        {
            var settings = server.GameSettings.GameServerSettings;
            long seed = WorldQuestManager.ComputeCurrentPeriodSeed(settings.WorldQuestResetDay, settings.WorldQuestResetHour, settings.WorldQuestResetMinute);
            Logger.Info($"Triggering server-wide world quest reset with seed {seed}");
            server.RpcManager.AnnounceAll("internal/command", RpcInternalCommand.WorldQuestReset, seed);
        }
    }
}
