using UnityEngine;

namespace VNFramework
{
    public class AudioController : MonoBehaviour, IController
    {
        private static AudioController instance;
        public static AudioController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AudioController>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(AudioController).Name;
                        instance = obj.AddComponent<AudioController>();
                        instance.InitController();
                    }
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

            _performModel = this.GetModel<PerformanceModel>();
            _configModel = this.GetModel<ConfigModel>();
            UpdateAudioVolume();

            this.RegisterEvent<ConfigChangedEvent>(_ => UpdateAudioVolume());
        }

        private AudioHandler CreateAudioHandler(string controllerName)
        {
            // 创建一个空的 GameObject 来代表每个 AudioHandler
            GameObject controllerObj = new GameObject(controllerName);

            controllerObj.AddComponent<AudioSource>();
            // 添加 AudioHandler 组件到 GameObject
            AudioHandler audioHandler = controllerObj.AddComponent<AudioHandler>();

            // 设置 AudioHandler 的父对象为 AudioController
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

        private void UpdateAudioVolume()
        {
            _bgmController.SetVolume(_configModel.BgmVolume);
            _bgsController.SetVolume(_configModel.BgsVolume);
            _chsController.SetVolume(_configModel.ChsVolume);
            _gmsController.SetVolume(_configModel.GmsVolume);
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
