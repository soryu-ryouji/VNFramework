namespace VNFramework
{
    class ShowChapterViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Show Chapter View Command");
            this.SendEvent<ShowChapterViewEvent>();
        }
    }

    class HideChapterViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Hide Chapter View Command");
            this.SendEvent<HideChapterViewEvent>();
        }
    }
}