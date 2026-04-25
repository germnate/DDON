public class BoardQuestRotationTask : DailyTask
{
    public BoardQuestRotationTask(uint hour, uint minute)
        : base(TaskType.BoardQuestRotation, hour, minute) { }

    public override void RunTask(DdonGameServer server)
    {
        server.LightQuestManager.InsertRecordsFromAsset();
        server.RpcManager.AnnounceAll("internal/command", RpcInternalCommand.BoardQuestDailyRotation, null);
    }
}

return new BoardQuestRotationTask(5, 0);
