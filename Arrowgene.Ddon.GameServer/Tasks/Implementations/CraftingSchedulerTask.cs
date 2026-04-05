using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Model.Scheduler;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class CraftingSchedulerTask : SecondlyTask
    {
        public CraftingSchedulerTask() : base(TaskType.Crafting, 1)
        {
        }

        public override void RunTask(DdonGameServer server)
        {
            server.RpcManager.AnnounceAll("internal/command", RpcInternalCommand.UpdateCrafting, null);
        }
    }
}
