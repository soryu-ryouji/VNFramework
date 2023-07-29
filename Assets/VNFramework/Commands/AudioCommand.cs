using UnityEngine;
namespace VNFramework
{
    class PlayAudioCommand : AbstractCommand
    {
        private string _audioName;
        private AudioPlayer _playerName;

        public PlayAudioCommand(string audioName, AudioPlayer playerName)
        {
            _audioName = audioName;
            _playerName = playerName;
        }

        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog($"{_playerName} Play Audio -> {_audioName}");
            if (_playerName == AudioPlayer.Bgm)
            {
                GameState.BgmChanged(VNutils.Hash(
                    "action", AudioAction.Play,
                    "audio_name", _audioName
                ));
            }

            else if (_playerName == AudioPlayer.Bgs)
            {
                GameState.BgsChanged(VNutils.Hash(
                    "action", AudioAction.Play,
                    "audio_name", _audioName
                ));
            }

            else if (_playerName == AudioPlayer.Chs)
            {
                GameState.ChsChanged(VNutils.Hash(
                    "action", AudioAction.Play,
                    "audio_name", _audioName
                ));
            }

            else if (_playerName == AudioPlayer.Gms)
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
        private AudioPlayer _playerName;

        public StopAudioCommand(AudioPlayer playerName)
        {
            _playerName = playerName;
        }

        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog($"{_playerName} Stop Audio");
            if (_playerName == AudioPlayer.Bgm)
            {
                GameState.BgmChanged(VNutils.Hash(
                    "action", AudioAction.Stop
                ));
            }

            else if (_playerName == AudioPlayer.Bgs)
            {
                GameState.BgsChanged(VNutils.Hash(
                    "action", AudioAction.Stop
                ));
            }

            else if (_playerName == AudioPlayer.Chs)
            {
                GameState.ChsChanged(VNutils.Hash(
                    "action", AudioAction.Stop
                ));
            }

            else if (_playerName == AudioPlayer.Gms)
            {
                GameState.GmsChanged(VNutils.Hash(
                    "action", AudioAction.Stop
                ));
            }
        }
    }
}