using System;
using UnityEngine;

namespace VNFramework
{
    public class AudioHandler : MonoBehaviour, ICanGetUtility
    {
        private string _currentAudioName;
        private float _currentVolume;
        private AudioSource _audioPlayer;

        public string CurrentAudioName
        {
            get => _currentAudioName;
        }

        public float CurrentVolume
        {
            get => _currentVolume;
        }

        public void SetVolume(string volume)
        {
            var vol = Convert.ToInt64(volume);
            _currentVolume = vol;
            _audioPlayer.volume = _currentVolume;
        }

        public void SetVolume(float volume)
        {
            _currentVolume = volume;
            _audioPlayer.volume = _currentVolume;
        }

        public void SetLoop(string value)
        {
            var isLoop = Convert.ToBoolean(value);
            _audioPlayer.loop = isLoop;
        }

        private void Awake()
        {
            _audioPlayer = GetComponent<AudioSource>();
        }

        public void PlayAudio(string audioName)
        {
            _audioPlayer.clip = GetAudioClip(audioName);
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

        private AudioClip GetAudioClip(string audioName)
        {
            var audio = this.GetUtility<GameDataStorage>().LoadSound(audioName);
            return audio;
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
