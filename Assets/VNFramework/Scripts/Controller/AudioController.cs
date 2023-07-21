using System;
using System.Collections;
using UnityEngine;

namespace VNFramework
{
    public class AudioController : MonoBehaviour, IController
    {

        private static bool _audioControllerIsCreated = false;
        private AudioHandler bgm;
        private AudioHandler bgs;
        private AudioHandler gms;
        private AudioHandler chs;

        private ConfigModel _configModel;
        private PerformingModel _performingModel;

        private void Awake()
        {
            GameState.AudioChanged += OnAudioChanged;

            if (!_audioControllerIsCreated)
            {
                DontDestroyOnLoad(gameObject);
                _audioControllerIsCreated = true;
            }
            else
            {
                Destroy(gameObject);
            }

            bgm = transform.Find("BGM").GetComponent<AudioHandler>();
            bgs = transform.Find("BGS").GetComponent<AudioHandler>();
            gms = transform.Find("GMS").GetComponent<AudioHandler>();
            chs = transform.Find("CHS").GetComponent<AudioHandler>();
        }
        private void Start()
        {
            _configModel = this.GetModel<ConfigModel>();
            _performingModel = this.GetModel<PerformingModel>();

            this.RegisterEvent<PerformingModelChangedEvent>(_ => UpdateAudioVolume());
            this.RegisterEvent<ConfigChangedEvent>(_ => UpdateAudioVolume());
            UpdateAudioVolume();
        }

        private void OnDestroy()
        {
            GameState.AudioChanged -= OnAudioChanged;
        }

        private void UpdateAudioVolume()
        {
            bgm.SetVolume(_configModel.BgmVolume * _performingModel.BgmVolume);
            bgs.SetVolume(_configModel.BgsVolume * _performingModel.BgsVolume);
            chs.SetVolume(_configModel.ChsVolume * _performingModel.ChsVolume);
            gms.SetVolume(_configModel.GmsVolume * _performingModel.GmsVolume);
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
                case "vol": _performingModel.BgmVolume = Convert.ToSingle(hash["volume"]); break;
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
                case "vol": _performingModel.BgsVolume = Convert.ToSingle(hash["volume"]); break;
            }
        }

        private void OnChsChanged(Hashtable hash)
        {
            switch (hash["action"])
            {
                case "play": chs.PlayAudio((string)hash["audio_name"]); break;
                case "stop": chs.StopAudio(); break;
                case "continue": chs.Continue(); break;
                case "vol": _performingModel.ChsVolume = Convert.ToSingle(hash["volume"]); break;
            }
        }

        private void OnGmsChanged(Hashtable hash)
        {
            switch (hash["action"])
            {
                case "play": gms.PlayAudio((string)hash["audio_name"]); break;
                case "stop": gms.StopAudio(); break;
                case "continue": gms.Continue(); break;
                case "vol": _performingModel.GmsVolume = Convert.ToSingle(hash["volume"]); break;
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}