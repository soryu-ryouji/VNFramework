using UnityEngine;

namespace VNFramework
{
    public class AudioController : MonoBehaviour, IController
    {
        private AudioHandler _bgmController;
        private AudioHandler _bgsController;
        private AudioHandler _chsController;
        private AudioHandler _gmsController;

        private PerformanceModel _performModel;
        private ConfigModel _configModel;

        private void Awake()
        {
            _bgmController = this.transform.Find("Bgm").GetComponent<AudioHandler>();
            _bgsController = this.transform.Find("Bgs").GetComponent<AudioHandler>();
            _chsController = this.transform.Find("Chs").GetComponent<AudioHandler>();
            _gmsController = this.transform.Find("Gms").GetComponent<AudioHandler>();
        }

        private void Start()
        {
            _performModel = this.GetModel<PerformanceModel>();
            _configModel = this.GetModel<ConfigModel>();
            UpdateAudioVolume();

            this.RegisterEvent<ConfigChangedEvent>(_ => UpdateAudioVolume());

            this.RegisterEvent<BgmPlayEvent>(_ => _bgmController.PlayAudio(_performModel.BgmName))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<BgsPlayEvent>(_ => _bgsController.PlayAudio(_performModel.BgsName))
                .UnRegisterWhenGameObjectDestroyed(this); ;
            this.RegisterEvent<ChsPlayEvent>(_ => _chsController.PlayAudio(_performModel.ChsName))
                .UnRegisterWhenGameObjectDestroyed(this); ;
            this.RegisterEvent<GmsPlayEvent>(_ => _gmsController.PlayAudio(_performModel.GmsName))
                .UnRegisterWhenGameObjectDestroyed(this); ;

            this.RegisterEvent<BgmStopEvent>(_ => _bgmController.StopAudio())
                .UnRegisterWhenGameObjectDestroyed(this); ;
            this.RegisterEvent<BgsStopEvent>(_ => _bgsController.StopAudio())
                .UnRegisterWhenGameObjectDestroyed(this); ;
            this.RegisterEvent<ChsStopEvent>(_ => _chsController.StopAudio())
                .UnRegisterWhenGameObjectDestroyed(this); ;
            this.RegisterEvent<GmsStopEvent>(_ => _gmsController.StopAudio())
                .UnRegisterWhenGameObjectDestroyed(this); ;
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
