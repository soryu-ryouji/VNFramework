using UnityEngine;

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
                Debug.Log("Play BGM: " + _audioName);
                AudioController.Instance.PlayAudio(_audioName, AsmObj.bgm);
                this.GetModel<PerformanceModel>().BgmName = _audioName;
            }

            else if (_playerName == AsmObj.bgs)
            {
                Debug.Log("Play BGS: " + _audioName);
                AudioController.Instance.PlayAudio(_audioName, AsmObj.bgs);
                this.GetModel<PerformanceModel>().BgsName = _audioName;
            }

            else if (_playerName == AsmObj.chs)
            {
                Debug.Log("Play CHS: " + _audioName);
                AudioController.Instance.PlayAudio(_audioName, AsmObj.chs);
                this.GetModel<PerformanceModel>().ChsName = _audioName;
            }

            else if (_playerName == AsmObj.gms)
            {
                Debug.Log("Play GMS: " + _audioName);
                AudioController.Instance.PlayAudio(_audioName, AsmObj.gms);
                this.GetModel<PerformanceModel>().GmsName = _audioName;
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
            if (_playerName == AsmObj.bgm) AudioController.Instance.StopAudio(AsmObj.bgm);
            else if (_playerName == AsmObj.bgs) AudioController.Instance.StopAudio(AsmObj.bgs);
            else if (_playerName == AsmObj.chs) AudioController.Instance.StopAudio(AsmObj.chs);
            else if (_playerName == AsmObj.gms) AudioController.Instance.StopAudio(AsmObj.gms);
        }
    }
}