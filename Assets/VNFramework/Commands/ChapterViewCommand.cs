using UnityEngine;
namespace VNFramework
{
    class ShowChapterViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Command : Show Chapter View");
            this.SendEvent<ShowChapterViewEvent>();
        }
    }

    class HideChapterViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Command : Hide Chapter View");
            this.SendEvent<HideChapterViewEvent>();
        }
    }
}