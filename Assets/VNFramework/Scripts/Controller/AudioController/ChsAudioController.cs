using UnityEngine;

namespace VNFramework
{
    class ChsAudioController : AudioHandler
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