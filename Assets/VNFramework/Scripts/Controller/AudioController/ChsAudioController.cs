using UnityEngine;

namespace VNFramework
{
    class ChsAudioController : AudioController
    {
        private void Awake()
        {
            audioPlayer = GetComponent<AudioSource>();
            GameState.ChsChanged += OnAudioChanged;
        }

        private void OnDestroy()
        {
            GameState.ChsChanged -= OnAudioChanged;
        }
    }
}