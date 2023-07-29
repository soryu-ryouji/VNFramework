using UnityEngine;

namespace VNFramework
{
    class ChsAudioController : AudioController
    {
        private ConfigModel _configModel;
        private void Awake()
        {
            audioPlayer = GetComponent<AudioSource>();
            GameState.ChsChanged += OnAudioChanged;
        }
        private void Start()
        {
            _configModel = this.GetModel<ConfigModel>();
            SetVolume(_configModel.ChsVolume);
            this.RegisterEvent<ConfigChangedEvent>(_ => SetVolume(this.GetModel<ConfigModel>().ChsVolume));
        }
        private void OnDestroy()
        {
            GameState.ChsChanged -= OnAudioChanged;
        }
    }
}