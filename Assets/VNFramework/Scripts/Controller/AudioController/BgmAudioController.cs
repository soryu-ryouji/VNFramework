using UnityEngine;

namespace VNFramework
{
    class BgmAudioController : AudioController
    {
        private ConfigModel _configModel;
        private void Awake()
        {
            audioPlayer = GetComponent<AudioSource>();
            GameState.BgmChanged += OnAudioChanged;
        }

        private void Start()
        {
            _configModel = this.GetModel<ConfigModel>();
            SetVolume(_configModel.BgmVolume);

            this.RegisterEvent<ConfigChangedEvent>(_ => SetVolume(_configModel.BgmVolume));
        }

        private void OnDestroy()
        {
            GameState.BgmChanged -= OnAudioChanged;
        }
    }
}