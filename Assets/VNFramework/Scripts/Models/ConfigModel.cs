namespace VNFramework
{
    class ConfigModel : AbstractModel
    {
        private float _bgmVolume;
        private float _bgsVolume;
        private float _chsVolume;
        private float _gmsVolume;
        private float _textSpeed;

        public float BgmVolume
        {
            get { return _bgmVolume; }
            set
            {
                _bgmVolume = value;
                this.SendEvent<ConfigChangedEvent>();
            }
        }

        public float BgsVolume
        {
            get { return _bgsVolume; }
            set
            {
                _bgsVolume = value;
                this.SendEvent<ConfigChangedEvent>();
            }
        }

        public float ChsVolume
        {
            get { return _chsVolume; }
            set
            {
                _chsVolume = value;
                this.SendEvent<ConfigChangedEvent>();
            }
        }

        public float GmsVolume
        {
            get { return _gmsVolume; }
            set
            {
                _gmsVolume = value;
                this.SendEvent<ConfigChangedEvent>();
            }
        }

        public float TextSpeed
        {
            get { return _textSpeed; }
            set
            {
                _textSpeed = value;
                this.SendEvent<ConfigChangedEvent>();
            }
        }

        protected override void OnInit()
        {
            this.GetUtility<GameDataStorage>().LoadSystemConfig();
        }
    }
}