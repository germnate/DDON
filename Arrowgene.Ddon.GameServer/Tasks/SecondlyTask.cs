using Arrowgene.Ddon.Shared.Model.Scheduler;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public abstract class SecondlyTask : SchedulerTask
    {
        public uint Seconds { get; }

        public SecondlyTask(TaskType type, uint seconds) : base(ScheduleInterval.Secondly, type)
        {
            Seconds = seconds;
        }

        public override long NextTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Seconds;
        }

        public override string TaskTypeName()
        {
            return "Seconds Amount Task";
        }
    }
}