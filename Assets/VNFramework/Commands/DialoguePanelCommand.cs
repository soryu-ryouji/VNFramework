namespace VNFramework
{
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
}