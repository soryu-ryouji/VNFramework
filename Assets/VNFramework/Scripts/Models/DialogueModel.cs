using System.Collections.Generic;

namespace VNFramework
{
    class DialogueModel : AbstractModel
    {
        public List<string> HistoricalDialogues { get; private set; }

        public bool IsAnimating = false;
        public bool NeedAnimation = true;

        private string _currentName;
        public string CurrentDialogue { get; set; }

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
            HistoricalDialogues.Add(dialogue);
        }

        public void InitModel()
        {
            CurrentDialogue = "";
            CurrentName = "";
            HistoricalDialogues.Clear();
        }

        protected override void OnInit()
        {
            HistoricalDialogues = new();
        }
    }
}