using System;
using UnityEngine;
using System.Collections;

namespace VNFramework
{
    public class AudioController : MonoBehaviour, IController
    {
        private string _currentAudioName;
        private float _currentVolume;
        protected AudioSource audioPlayer;

        public string CurrentAudioName
        {
            get => _currentAudioName;
        }

        public float CurrentVolume
        {
            get => _currentVolume;
        }
        
        public void OnAudioChanged(Hashtable hash)
        {
            var action = (AudioAction)hash["action"];

            if (action == AudioAction.Play) PlayAudio((string)hash["audio_name"]);
            else if (action == AudioAction.Stop) StopAudio();
            else if (action == AudioAction.Vol) SetVolume(Convert.ToSingle(hash["volume"]));
            else if (action == AudioAction.Loop) SetLoop((string)hash["is_loop"]);
        }

        public void SetVolume(string volume)
        {
            var vol = Convert.ToInt64(volume);
            _currentVolume = vol;
            audioPlayer.volume = _currentVolume;
        }

        public void SetVolume(float volume)
        {
            _currentVolume = volume;
            audioPlayer.volume = _currentVolume;
        }

        public void SetLoop(string value)
        {
            var isLoop = Convert.ToBoolean(value);
            audioPlayer.loop = isLoop;
        }


        public void PlayAudio(string audioName)
        {
            audioPlayer.clip = GetAudioClip(audioName);
            audioPlayer.Play();
        }

        public void StopAudio()
        {
            audioPlayer.Stop();
        }

        public void Continue()
        {
            audioPlayer.Play();
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
