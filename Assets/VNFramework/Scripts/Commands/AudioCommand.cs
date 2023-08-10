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
                GameState.BgmChanged(VNutils.Hash(
                    "action", AudioAction.Play,
                    "audio_name", _audioName
                ));
            }

            else if (_playerName == AsmObj.bgs)
            {
                GameState.BgsChanged(VNutils.Hash(
                    "action", AudioAction.Play,
                    "audio_name", _audioName
                ));
            }

            else if (_playerName == AsmObj.chs)
            {
                GameState.ChsChanged(VNutils.Hash(
                    "action", AudioAction.Play,
                    "audio_name", _audioName
                ));
            }

            else if (_playerName == AsmObj.gms)
            {
                GameState.GmsChanged(VNutils.Hash(
                    "action", AudioAction.Play,
                    "audio_name", _audioName
                ));
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
            if (_playerName == AsmObj.bgm)
            {
                GameState.BgmChanged(VNutils.Hash(
                    "action", AudioAction.Stop
                ));
            }

            else if (_playerName == AsmObj.bgs)
            {
                GameState.BgsChanged(VNutils.Hash(
                    "action", AudioAction.Stop
                ));
            }

            else if (_playerName == AsmObj.chs)
            {
                GameState.ChsChanged(VNutils.Hash(
                    "action", AudioAction.Stop
                ));
            }

            else if (_playerName == AsmObj.gms)
            {
                GameState.GmsChanged(VNutils.Hash(
                    "action", AudioAction.Stop
                ));
            }
        }
    }
}