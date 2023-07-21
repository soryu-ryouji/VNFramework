using UnityEngine;
namespace VNFramework
{
    class PlayAudioCommand : AbstractCommand
    {
        private string _audioName;
        private string _playerName;

        public PlayAudioCommand(string audioName, string playerName)
        {
            _audioName = audioName;
            _playerName = playerName;
        }

        protected override void OnExecute()
        {
            GameState.AudioChanged(VNutils.Hash(
                "object", _playerName,
                "action", "play",
                "audio_name", _audioName
            ));
        }
    }
}