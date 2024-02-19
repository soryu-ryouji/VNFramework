namespace VNFramework
{
    class ConfigModel : AbstractModel
    {
        private float _bgmVolume;
        private float _bgsVolume;
        private float _chsVolume;
        private float _gmsVolume;
        private float _textSpeed;
        private string _language;

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

        public string Language
        {
            get { return _language; }
            set
            {
                _language = value;
                this.SendEvent<LanguageChangedEvent>();
            }
        }

        public void InitModel()
        {
            this.GetUtility<GameDataStorage>().LoadSystemConfig();
        }

        protected override void OnInit()
        {
        }
    }
}