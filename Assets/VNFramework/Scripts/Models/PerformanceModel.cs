using System.Collections;
using System.Collections.Generic;

namespace VNFramework
{
    class PerformanceModel : AbstractModel, ICanGetModel
    {
        private string _performingMermaidName;
        private int _performingIndex;
        private string _dialogue;
        public bool IsOpenChooseView;
        public bool IsAutoExecuteCommand;

        private float _performingBgmVolume;
        private float _performingBgsVolume;
        private float _performingChsVolume;
        private float _performingGmsVolume;
        private string _bgpName;
        private string _chLeft;
        private string _chMid;
        private string _chRight;

        private string _bgmName;
        private string _bgsName;
        private string _chsName;
        private string _gmsName;
        private List<(string mermaidName, string optionText)> _chooseList;

        public string PerformingMermaidName
        {
            get { return _performingMermaidName; }
            set
            {
                _performingMermaidName = value;
            }
        }

        public List<(string mermaidName, string optionText)> ChooseList
        {
            get { return _chooseList; }
            set
            {
                _chooseList = value;
            }
        }

        public int PerformingIndex
        {
            get { return _performingIndex; }
            set
            {
                _performingIndex = value;
            }
        }

        public string PerformingDialogue
        {
            get { return _dialogue; }
            set { _dialogue = value; }
        }

        public float BgmVolume
        {
            get { return _performingBgmVolume; }
            set
            {
                _performingBgmVolume = value;
                this.SendEvent<PerformingModelChangedEvent>();
            }
        }
        public float BgsVolume
        {
            get { return _performingBgsVolume; }
            set
            {
                _performingBgsVolume = value;
                this.SendEvent<PerformingModelChangedEvent>();
            }
        }
        public float ChsVolume
        {
            get { return _performingChsVolume; }
            set
            {
                _performingChsVolume = value;
                this.SendEvent<PerformingModelChangedEvent>();
            }
        }
        public float GmsVolume
        {
            get { return _performingGmsVolume; }
            set
            {
                _performingGmsVolume = value;
                this.SendEvent<PerformingModelChangedEvent>();
            }
        }

        public string BgmName
        {
            get { return _bgmName; }
            set
            {
                _bgmName = value;
            }
        }

        public string BgsName
        {
            get { return _bgsName; }
            set
            {
                _bgsName = value;
            }
        }

        public string ChsName
        {
            get { return _chsName; }
            set
            {
                _chsName = value;
            }
        }

        public string GmsName
        {
            get { return _gmsName; }
            set
            {
                _gmsName = value;
            }
        }

        public string BgpName
        {
            get { return _bgpName; }
            set
            {
                _bgpName = value;
            }
        }

        public string ChLeft
        {
            get { return _chLeft; }
            set
            {
                _chLeft = value;
            }
        }

        public string ChMid
        {
            get { return _chMid; }
            set
            {
                _chMid = value;
            }
        }

        public string ChRight
        {
            get { return _chRight; }
            set
            {
                _chRight = value;
            }
        }

        public void Init()
        {
            PerformingIndex = 0;
            _dialogue = "";
            IsOpenChooseView = false;
            IsAutoExecuteCommand = false;
            _performingBgmVolume = 1;
            _performingBgsVolume = 1;
            _performingChsVolume = 1;
            _performingGmsVolume = 1;
            _bgpName = "";
            _chLeft = "";
            _chMid = "";
            _chRight = "";

            _bgmName = "";
            _bgsName = "";
            _chsName = "";
            _gmsName = "";
        }

        protected override void OnInit()
        {
            BgmVolume = 1;
            BgsVolume = 1;
            ChsVolume = 1;
            GmsVolume = 1;
        }
    }
}