using UnityEngine;

namespace VNFramework
{
    class ShowMenuViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Command : Show Menu View");
            this.SendEvent<ShowMenuViewEvent>();
        }
    }

    class HideMenuViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Command : Hide Menu View");
            this.SendEvent<HideMenuViewEvent>();
        }
    }
}