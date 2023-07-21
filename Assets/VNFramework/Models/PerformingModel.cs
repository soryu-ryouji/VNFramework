namespace VNFramework
{
    class PerformingModel : AbstractModel
    {
        private float _performingBgmVolume;
        private float _performingBgsVolume;
        private float _performingChsVolume;
        private float _performingGmsVolume;

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

        protected override void OnInit()
        {
            BgmVolume = 1;
            BgsVolume = 1;
            ChsVolume = 1;
            GmsVolume = 1;
        }
    }
}