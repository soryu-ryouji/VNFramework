namespace VNFramework
{
    class PlayAudioCommand : AbstractCommand
    {
        private string _audioName;
        private AsmObj _playerName;

        public PlayAudioCommand(string audioName, AsmObj playerName)
        {
            _audioName = audioName;
            _playerName = playerName;
        }

        protected override void OnExecute()
        {
            if (_playerName == AsmObj.bgm)
            {
                this.GetModel<PerformanceModel>().BgmName = _audioName;

                this.SendEvent<BgmPlayEvent>();
            }

            else if (_playerName == AsmObj.bgs)
            {
                this.GetModel<PerformanceModel>().BgsName = _audioName;

                this.SendEvent<BgsPlayEvent>();
            }

            else if (_playerName == AsmObj.chs)
            {
                this.GetModel<PerformanceModel>().ChsName = _audioName;

                this.SendEvent<ChsPlayEvent>();
            }

            else if (_playerName == AsmObj.gms)
            {
                this.GetModel<PerformanceModel>().GmsName = _audioName;

                this.SendEvent<GmsPlayEvent>();
            }
        }
    }

    class StopAudioCommand : AbstractCommand
    {
        private AsmObj _playerName;

        public StopAudioCommand(AsmObj playerName)
        {
            _playerName = playerName;
        }

        protected override void OnExecute()
        {
            if (_playerName == AsmObj.bgm) this.SendEvent<BgmStopEvent>();
            else if (_playerName == AsmObj.bgs) this.SendEvent<BgsStopEvent>();
            else if (_playerName == AsmObj.chs) this.SendEvent<ChsStopEvent>();
            else if (_playerName == AsmObj.gms) this.SendEvent<GmsStopEvent>();
        }
    }
}