using UnityEngine;

namespace VNFramework
{
    class BgsAudioController : AudioController
    {
        private ConfigModel _configModel;
        private void Awake()
        {
            audioPlayer = GetComponent<AudioSource>();
            GameState.BgsChanged += OnAudioChanged;
        }
        private void Start()
        {
            _configModel = this.GetModel<ConfigModel>();
            SetVolume(_configModel.BgsVolume);

            this.RegisterEvent<ConfigChangedEvent>(_ => SetVolume(this.GetModel<ConfigModel>().BgsVolume));
        }
        private void OnDestroy()
        {
            GameState.BgsChanged -= OnAudioChanged;
        }
    }
}