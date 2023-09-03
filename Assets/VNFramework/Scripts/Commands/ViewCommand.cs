using UnityEngine;

namespace VNFramework
{
    class ShowTitleViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("<color=green>Command: Show Title View</color>");
            this.SendEvent<ShowTitleViewEvent>();
        }
    }
    class HideTitleViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<HideTitleViewEvent>();
        }
    }
    class ShowBacklogViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ShowBacklogViewEvent>();
        }
    }

    class HideBacklogViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<HideBacklogViewEvent>();
        }
    }

    class ShowChapterViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ShowChapterViewEvent>();
        }
    }

    class HideChapterViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<HideChapterViewEvent>();
        }
    }

    class ShowConfigViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
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

    class ChangeNameCommand : AbstractCommand
    {
        public string name;

        public ChangeNameCommand(string name)
        {
            this.name = name;
        }

        protected override void OnExecute()
        {
            this.GetModel<DialogueModel>().CurrentName = name;
        }
    }

    class AppendDialogueCommand : AbstractCommand
    {
        public string dialogue;

        public AppendDialogueCommand(string dialogue)
        {
            this.dialogue = dialogue;
        }

        protected override void OnExecute()
        {

            var dialogueModel = this.GetModel<DialogueModel>();
            dialogueModel.CurrentDialogue += dialogue;
            this.SendEvent<AppendDialogueEvent>();
        }
    }

    class ClearDialogueCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var dialogueModel = this.GetModel<DialogueModel>();
            dialogueModel.AddDialogueNode(dialogueModel.CurrentDialogue);
            dialogueModel.CurrentDialogue = "";
            this.SendEvent<ClearDialogueEvent>();
        }
    }

    class AppendNewlineToDialogueCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetModel<DialogueModel>().CurrentDialogue += "<br>";
            this.SendEvent<AppendNewLineToDialogueEvent>();
        }
    }

    class StopDialogueAnimCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("Trigger Stop Dialogue Anim Event");
            this.SendEvent<StopDialogueAnimEvent>();
        }
    }

    class OpenFullDialogueBoxCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<OpenFullDialogueBoxEvent>();
        }
    }

    class OpenNormDialogueBoxCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<OpenNormDialogueBoxEvent>();
        }
    }

    class ShowMenuViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ShowMenuViewEvent>();
        }
    }

    class HideMenuViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<HideMenuViewEvent>();
        }
    }

    class ShowDialoguePanelCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ShowDialoguePanelEvent>();
        }
    }

    class HideDialoguePanelCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<HideDialoguePanelEvent>();
        }
    }

    class ToggleDialoguePanelCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
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

    class ShowLoadGameSaveViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ShowLoadGameSaveViewEvent>();
        }
    }

    class ShowSaveGameSaveViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ShowSaveGameSaveViewEvent>();
        }
    }

    class HideGameSaveViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<HideGameSaveViewEvent>();
        }
    }

    class ShowChooseViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ShowChooseViewEvent>();
        }
    }
    
    class HideChooseViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetModel<PerformanceModel>().IsOpenChooseView = false;
            this.SendEvent<HideChooseViewEvent>();
        }
    }

    class InitViewControllerCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<InitViewControllerEvent>();
        }
    }
}