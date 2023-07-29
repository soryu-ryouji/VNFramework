using UnityEngine;

namespace VNFramework
{
    class BgmAudioController : AudioController
    {
        private void Awake()
        {
            audioPlayer = GetComponent<AudioSource>();
            GameState.BgmChanged += OnAudioChanged;
        }

        private void OnDestroy()
        {
            GameState.BgmChanged -= OnAudioChanged;
        }
    }
}