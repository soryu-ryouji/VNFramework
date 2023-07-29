using UnityEngine;

namespace VNFramework
{
    class BgsAudioController : AudioController
    {
        private void Awake()
        {
            audioPlayer = GetComponent<AudioSource>();
            GameState.BgsChanged += OnAudioChanged;
        }

        private void OnDestroy()
        {
            GameState.BgsChanged -= OnAudioChanged;
        }
    }
}