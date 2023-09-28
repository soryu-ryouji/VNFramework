using UnityEngine;

namespace VNFramework
{
    public class AudioController : MonoBehaviour
    {
        private static AudioController instance;
        public static AudioController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject(typeof(AudioController).Name).AddComponent<AudioController>();
                    instance.InitController();
                }
                return instance;
            }
        }

        private AudioHandler _bgmController;
        private AudioHandler _bgsController;
        private AudioHandler _chsController;
        private AudioHandler _gmsController;

        private PerformanceModel _performModel;
        private ConfigModel _configModel;

        private void InitController()
        {
            Debug.Log("<color=green>AudioController: Init</color>");
            _bgmController = CreateAudioHandler("Bgm");
            _bgsController = CreateAudioHandler("Bgs");
            _chsController = CreateAudioHandler("Chs");
            _gmsController = CreateAudioHandler("Gms");
        }

        private AudioHandler CreateAudioHandler(string controllerName)
        {
            var controllerObj = new GameObject(controllerName);
            var audioHandler = controllerObj.AddComponent<AudioHandler>();
            controllerObj.transform.SetParent(transform);

            return audioHandler;
        }

        public void PlayAudio(string audioName, AsmObj audioType)
        {
            switch (audioType)
            {
                case AsmObj.bgm: _bgmController.PlayAudio(audioName); break;
                case AsmObj.bgs: _bgsController.PlayAudio(audioName); break;
                case AsmObj.chs: _chsController.PlayAudio(audioName); break;
                case AsmObj.gms: _gmsController.PlayAudio(audioName); break;
            }
        }

        public void StopAudio(AsmObj audioType)
        {
            switch (audioType)
            {
                case AsmObj.bgm: _bgmController.StopAudio(); break;
                case AsmObj.bgs: _bgsController.StopAudio(); break;
                case AsmObj.chs: _chsController.StopAudio(); break;
                case AsmObj.gms: _gmsController.StopAudio(); break;
            }
        }

        private void SetAudioVolume(float volume, AsmObj audioType)
        {
            switch (audioType)
            {
                case AsmObj.bgm: _bgmController.SetVolume(volume); break;
                case AsmObj.bgs: _bgsController.SetVolume(volume); break;
                case AsmObj.chs: _chsController.SetVolume(volume); break;
                case AsmObj.gms: _gmsController.SetVolume(volume); break;
            }
        }
    }
}
