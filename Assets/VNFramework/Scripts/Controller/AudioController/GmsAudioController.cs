using UnityEngine;

namespace VNFramework
{
    class GmsAudioController : AudioController
    {
        private ConfigModel _configModel;
        private void Awake()
        {
            audioPlayer = GetComponent<AudioSource>();
            GameState.GmsChanged += OnAudioChanged;
        }
        private void Start()
        {
            _configModel = this.GetModel<ConfigModel>();
            SetVolume(_configModel.GmsVolume);
            this.RegisterEvent<ConfigChangedEvent>(_ => SetVolume(this.GetModel<ConfigModel>().GmsVolume));
        }
        private void OnDestroy()
        {
            GameState.GmsChanged -= OnAudioChanged;
        }
    }
}