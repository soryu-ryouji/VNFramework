using UnityEngine;
namespace VNFramework
{
    class ShowConfigViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Command : Show Config View");
            this.SendEvent<ShowConfigViewEvent>();
        }
    }

    class HideConfigViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<HideConfigViewEvent>();
        }
    }
}