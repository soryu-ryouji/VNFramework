using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace VNFramework
{
    public class ConfigController:MonoBehaviour
    {
        private void Awake()
        {
            LoadConfigFromFile();
            _chapterRecord = AssetsManager.LoadChapterRecord();
        }

        private static float _textSpeed;
        private static float _chsVolume;
        private static float _gmsVolume;
        private static float _bgmVolume;
        private static float _bgsVolume;

        public static string _currentChapterName;
        public static string CurrentChapterName
        {
            get { return _currentChapterName; }
            set
            {
                _currentChapterName = value;
            }
        }

        private static List<string> _chapterRecord = new();

        public static void AddChapterRecord(string chapterName)
        {
            _chapterRecord.Add(chapterName);
        }

        public static List<string> ChapterRecord
        {
            get { return _chapterRecord; }
        }

        public static float TextSpeed
        {
            get { return _textSpeed; }
        }

        public static float ChsVolume
        {
            get { return _chsVolume; }
        }

        public static float GmsVolume
        {
            get { return _gmsVolume; }
        }

        public static float BgmVolume
        {
            get { return _bgmVolume; }
        }

        public static float BgsVolume
        {
            get { return _bgsVolume; }
        }

        public static void SetBgmVolume(float bgmVolume)
        {
            _bgmVolume = bgmVolume;
            GameState.AudioChanged(VNutils.Hash(
                "object", "bgm",
                "action", "vol",
                "volume", _bgmVolume
            ));
        }
        public static void SetTextSpeed(float bgsVolume)
        {
            _bgsVolume = bgsVolume;
            GameState.DialogueChanged(VNutils.Hash(
                "object", "dialogue",
                "action", "text_speed",
                "volume", _textSpeed / 10
            ));
        }

        public static void SetBgsVolume(float bgsVolume)
        {
            _bgsVolume = bgsVolume;
            GameState.AudioChanged(VNutils.Hash(
                "object", "bgs",
                "action", "vol",
                "volume", _bgsVolume
            ));
        }

        public static void SetChsVolume(float chsVolume)
        {
            _chsVolume = chsVolume;
            GameState.AudioChanged(VNutils.Hash(
                "object", "chs",
                "action", "vol",
                "volume", _chsVolume
            ));
        }
        public static void SetGmsVolume(float gmsVolume)
        {
            _gmsVolume = gmsVolume;
            GameState.AudioChanged(VNutils.Hash(
                "object", "gms",
                "action", "vol",
                "volume", _gmsVolume
            ));
        }

        public static void SaveConfigToFile()
        {
            string configFile = $"text_speed:{_textSpeed}\n" +
                $"bgm_volume:{_bgmVolume}\n"+
                $"bgs_volume:{_bgsVolume}\n"+
                $"chs_volume:{_chsVolume}\n"+
                $"gms_volume:{_gmsVolume}";
            AssetsManager.SaveGameConfig(configFile);
        }

        public static void LoadConfigFromFile()
        {
            var configs = AssetsManager.LoadGameConfig();

            _bgmVolume = (float)configs["bgm_volume"];
            _bgsVolume = (float)configs["bgs_volume"];
            _chsVolume = (float)configs["chs_volume"];
            _gmsVolume = (float)configs["gms_volume"];
            _textSpeed = (float)configs["text_speed"];
        }
    }
}