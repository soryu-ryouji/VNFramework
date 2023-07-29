using UnityEngine;

namespace VNFramework
{
    class GmsAudioController : AudioController
    {
        private void Awake()
        {
            audioPlayer = GetComponent<AudioSource>();
            GameState.GmsChanged += OnAudioChanged;
        }

        private void OnDestroy()
        {
            GameState.GmsChanged -= OnAudioChanged;
        }
    }
}