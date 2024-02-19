using Unity.VisualScripting;
using UnityEngine;

namespace VNFramework
{
    [RequireComponent(typeof(AudioSource))]
    class AudioHandler: MonoBehaviour, ICanGetUtility
    {
        private AudioSource _audioPlayer;

        public float Volume => _audioPlayer.volume;

        private void Awake()
        {
            _audioPlayer = this.GetComponent<AudioSource>();
        }

        public void SetVolume(float volume)
        {
            _audioPlayer.volume = volume;
        }

        public void SetLoop(bool isLoop)
        {
            _audioPlayer.loop = isLoop;
        }

        public void PlayAudio(string audioName)
        {
            _audioPlayer.clip = this.GetUtility<GameDataStorage>().LoadSound(audioName);
            _audioPlayer.Play();
        }

        public void StopAudio()
        {
            _audioPlayer.Stop();
        }

        public void Continue()
        {
            _audioPlayer.Play();
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
