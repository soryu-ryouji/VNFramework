using UnityEngine;

namespace VNFramework
{
    class ShowDialoguePanelCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Command : Show Dialogue Panel");
            this.SendEvent<ShowDialoguePanelEvent>();
        }
    }
    
    class HideDialoguePanelCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Command : Hide Dialogue Panel");
            this.SendEvent<HideDialoguePanelEvent>();
        }
    }

    class ToggleDialoguePanelCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Command : Toggle Dialogue Panel");
            this.SendEvent<ToggleDialoguePanelEvent>();
        }
    }

    class ShowPerformanceViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ShowPerformanceViewEvent>();
        }
    }
    class HidePerformanceViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<HidePerformanceViewEvent>();
        }
    }
}