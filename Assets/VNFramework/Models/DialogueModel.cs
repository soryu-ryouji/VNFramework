using System.Collections.Generic;

namespace VNFramework
{
    class DialogueModel : AbstractModel
    {
        private List<string> _historicalDialogues;

        private string _currentDialogue;
        private string _currentName;
        public string CurrentDialogue
        {
            get { return _currentDialogue; }
            set { _currentDialogue = value; }
        }
        public string CurrentName
        {
            get { return _currentName; }
            set
            {
                _currentName = value;
                this.SendEvent<ChangeNameEvent>();
            }
        }

        public void AddDialogueNode(string dialogue)
        {
            _historicalDialogues.Add(dialogue);
        }

        public string[] GetHistoricalDialogues()
        {
            return _historicalDialogues.ToArray();
        }

        protected override void OnInit()
        {
            _historicalDialogues = new();
        }
    }
}