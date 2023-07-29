using UnityEngine;

namespace VNFramework
{
    class ShowDialoguePanelCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Show Dialogue View Command");
            this.SendEvent<ShowDialoguePanelEvent>();
        }
    }

    class HideDialoguePanelCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Hide Dialogue View Command");
            this.SendEvent<HideDialoguePanelEvent>();
        }
    }

    class ToggleDialoguePanelCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Toggle Dialogue View Command");
            this.SendEvent<ToggleDialoguePanelEvent>();
        }
    }

    class ShowPerformanceViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Show Performance View Command");
            this.SendEvent<ShowPerformanceViewEvent>();
        }
    }
    class HidePerformanceViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Hide Performance View Command");
            this.SendEvent<HidePerformanceViewEvent>();
        }
    }
}