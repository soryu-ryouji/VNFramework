namespace VNFramework
{
    class ShowConfigViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Show Config View Command");
            this.SendEvent<ShowConfigViewEvent>();
        }
    }

    class HideConfigViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Hide Config View Command");
            this.SendEvent<HideConfigViewEvent>();
        }
    }
}