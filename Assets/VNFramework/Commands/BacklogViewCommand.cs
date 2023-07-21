using UnityEngine;
namespace VNFramework
{
    class ShowBacklogViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Command : Show Backlog View");
            this.SendEvent<ShowBacklogViewEvent>();
        }
    }

    class HideBacklogViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Command : Hide Backlog View");
            this.SendEvent<HideBacklogViewEvent>();
        }
    }
}