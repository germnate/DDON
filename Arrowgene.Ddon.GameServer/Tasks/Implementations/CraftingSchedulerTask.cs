using System;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Ddon.Server;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class CraftingSchedulerTask : SecondlyTask
    {
        public CraftingSchedulerTask() : base(TaskType.Crafting, 1)
        {
        }

        public override void RunTask(DdonGameServer server)
        {
            server.CraftManager.UpdateOnlineCraftingProgress();
        }

        public override string TaskTypeName() => "Crafting";
    }
}