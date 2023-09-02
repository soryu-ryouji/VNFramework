using UnityEngine;

namespace VNFramework
{
    class AudioHandler: MonoBehaviour, ICanGetUtility
    {
        private AudioSource audioPlayer;
        private void Awake()
        {
            audioPlayer = this.GetComponent<AudioSource>();
        }

        public void SetVolume(float volume)
        {
            audioPlayer.volume = volume;
        }

        public void SetLoop(bool isLoop)
        {
            audioPlayer.loop = isLoop;
        }

        public void PlayAudio(string audioName)
        {
            audioPlayer.clip = this.GetUtility<GameDataStorage>().LoadSound(audioName);
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

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
