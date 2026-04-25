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
            var settings = server.GameSettings.GameServerSettings;
            // Run if server-side pool rotation is needed OR if first-clear reward tracking needs periodic resets.
            return settings.WorldQuestSystem == WorldQuestSystemMode.ServerReset
                || settings.WorldQuestFirstClearRewards;
        }

        public override void RunTask(DdonGameServer server)
        {
            var settings = server.GameSettings.GameServerSettings;
            long seed = WorldQuestManager.ComputeCurrentPeriodSeed(settings.WorldQuestResetDay, settings.WorldQuestResetHour, settings.WorldQuestResetMinute, settings.GetEffectiveUtcOffset());
            Logger.Info($"Triggering server-wide world quest reset with seed {seed}");
            server.RpcManager.AnnounceAll("internal/command", RpcInternalCommand.WorldQuestReset, seed);
        }
    }
}
