using System;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Ddon.Server;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class CraftingSchedulerTask : SchedulerTask
    {
        public CraftingSchedulerTask() : base(ScheduleInterval.Hourly, TaskType.Crafting)
        {
        }

        public override string TaskTypeName() => "Crafting";

        public override void RunTask(DdonGameServer server)
        {
            server.CraftManager.UpdateOnlineCraftingProgress();
        }

        public override long NextTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 10;
        }

        public override bool IsEnabled(DdonGameServer server) => true;
    }
}