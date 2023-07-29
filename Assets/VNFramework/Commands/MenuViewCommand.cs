namespace VNFramework
{
    class ShowMenuViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Show Menu View Command");
            this.SendEvent<ShowMenuViewEvent>();
        }
    }

    class HideMenuViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Hide Menu View Command");
            this.SendEvent<HideMenuViewEvent>();
        }
    }
}