using System.Collections.Generic;

namespace VNFramework
{
    class DialogueModel : AbstractModel
    {
        private List<string> _historicalDialogues;

        private string _currentDialogue;
        private string _currentName;
        public bool isAnimating = false;
        public bool needAnimation = true;

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

        public void InitModel()
        {
            _currentDialogue = "";
            _currentName = "";
            _historicalDialogues.Clear();
            isAnimating = false;
            needAnimation = true;
        }

        public override string ToString()
        {
            return $@"Is Animating : {isAnimating}
needAnimation : {needAnimation}";
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