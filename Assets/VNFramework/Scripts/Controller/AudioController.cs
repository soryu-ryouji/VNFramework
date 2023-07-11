using System.Collections;
using UnityEngine;

namespace VNFramework
{
    public class AudioController : MonoBehaviour
    {
        private AudioHandler bgm;
        private AudioHandler bgs;
        private AudioHandler gms;
        private AudioHandler chs;

        private void Awake()
        {
            GameState.AudioChanged += OnAudioChanged;
            bgm = transform.Find("BGM").GetComponent<AudioHandler>();
            bgs = transform.Find("BGS").GetComponent<AudioHandler>();
            gms = transform.Find("GMS").GetComponent<AudioHandler>();
            chs = transform.Find("CHS").GetComponent<AudioHandler>();
        }

        private void OnDestroy()
        {
            GameState.AudioChanged -= OnAudioChanged;
        }

        private void OnAudioChanged(Hashtable hash)
        {
            var obj = (string)hash["object"];

            if (obj == "bgm") OnBgmChanged(hash);
            else if (obj == "bgs") OnBgsChanged(hash);
            else if (obj == "gms") OnGmsChanged(hash);
            else if (obj == "chs") OnChsChanged(hash);
        }

        private void OnBgmChanged(Hashtable hash)
        {
            switch (hash["action"])
            {
                case "play": bgm.PlayAudio((string)hash["audio_name"]); break;
                case "stop": bgm.StopAudio(); break;
                case "continue": bgm.Continue(); break;
                case "vol": bgm.SetVolume((float)hash["volume"]); break;
                case "loop": bgm.SetLoop((string)hash["is_loop"]); break;
            }
        }

        private void OnBgsChanged(Hashtable hash)
        {
            switch (hash["action"])
            {
                case "play": bgs.PlayAudio((string)hash["audio_name"]); break;
                case "stop": bgs.StopAudio(); break;
                case "continue": bgs.Continue(); break;
                case "vol": bgs.SetVolume((float)hash["volume"]); break;
            }
        }

        private void OnChsChanged(Hashtable hash)
        {
            switch (hash["action"])
            {
                case "play": chs.PlayAudio((string)hash["audio_name"]); break;
                case "stop": chs.StopAudio(); break;
                case "continue": chs.Continue(); break;
                case "vol": chs.SetVolume((float)hash["volume"]); break;
            }
        }

        private void OnGmsChanged(Hashtable hash)
        {
            switch (hash["action"])
            {
                case "play": gms.PlayAudio((string)hash["audio_name"]); break;
                case "stop": gms.StopAudio(); break;
                case "continue": gms.Continue(); break;
                case "vol": gms.SetVolume((float)hash["volume"]); break;
            }
        }
    }
}