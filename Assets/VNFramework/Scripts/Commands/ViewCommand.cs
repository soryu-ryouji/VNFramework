namespace VNFramework
{
    class ShowBacklogViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Show Backlog View Command");
            this.SendEvent<ShowBacklogViewEvent>();
        }
    }

    class HideBacklogViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Hide Backlog View Command");
            this.SendEvent<HideBacklogViewEvent>();
        }
    }

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

    class ShowSaveFileViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Show Save File View Command");
            this.SendEvent<ShowGameSaveViewEvent>();
        }
    }

    class HideSaveFileViewCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Hide Save File View Command");
            this.SendEvent<HideGameSaveViewEvent>();
        }
    }
}