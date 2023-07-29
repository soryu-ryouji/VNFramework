namespace VNFramework
{
    class ShowBacklogViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Show Backlog View Command");
            this.SendEvent<ShowBacklogViewEvent>();
        }
    }

    class HideBacklogViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Hide Backlog View Command");
            this.SendEvent<HideBacklogViewEvent>();
        }
    }
}