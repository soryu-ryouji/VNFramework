namespace VNFramework
{
    class PerformingModel : AbstractModel
    {
        private string _performingMermaidName;
        private float _performingBgmVolume;
        private float _performingBgsVolume;
        private float _performingChsVolume;
        private float _performingGmsVolume;
        private string _bgpName;
        private string _bgmName;
        private string _chLeft;
        private string _chMid;
        private string _chRight;

        public string PerformingMermaidName
        {
            get { return _performingMermaidName; }
            set
            {
                _performingMermaidName = value;
            }
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

        protected override void OnInit()
        {
            BgmVolume = 1;
            BgsVolume = 1;
            ChsVolume = 1;
            GmsVolume = 1;
        }
    }
}