using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using VNFramework.Core;

namespace VNFramework
{
    class GameDataStorage : IUtility, ICanGetModel, ICanGetUtility, ICanSendEvent
    {
        private readonly string _configDirPath = Path.Combine(Application.dataPath, "Config");

        private Dictionary<string, AssetBundle> _abDic = new();
        public AudioClip LoadSound(string audioName)
        {
            if (audioName == "")
            {
                Debug.Log("<color=red>Audio Name is Null</color>");
                return null;
            }

            var ret = _abDic["sound"].LoadAsset<AudioClip>(audioName);
            if (ret == null) Debug.Log($"<color=red>AudioClip {audioName} Not Found</color>");

            return ret;
        }

        public Sprite LoadSprite(string spriteName)
        {
            if (spriteName == "")
            {
                Debug.Log(string.Format("<color=red>Sprite Name is Null</color>"));
                return null;
            }

            var ret = _abDic["sprite"].LoadAsset<Sprite>(spriteName);
            if (ret == null) Debug.Log($"<color=red>Sprite {spriteName} not found</color>");

            return ret;
        }

        public string[] LoadVNScript(string scriptName)
        {
            string[] fileLines = _abDic["vnscript"].LoadAsset<TextAsset>(scriptName).text.Split('\n');
            string[] vnScriptLines = fileLines.Select(str => str.TrimEnd('\r', '\n')).ToArray();

            return vnScriptLines;
        }

        public string LoadVNMermaid(string name)
        {
            var file = _abDic["vnscript"].LoadAsset<TextAsset>(name).text;

            return file;
        }

        public GameSave[] LoadGameSave()
        {
            string path = Path.Combine(_configDirPath, "save_file.txt");
            if (!File.Exists(path)) return new GameSave[60];

            string gameSaveText = File.ReadAllText(path);

            var gameSaves = VNGameSave.ParseGameSaveText(gameSaveText);

            return gameSaves;
        }

        public void SaveGameSave()
        {
            var gameSaves = this.GetModel<GameSaveModel>().GameSaves;

            var gameSaveText = VNGameSave.GameSavesToText(gameSaves);
            var path = Path.Combine(_configDirPath, "save_file.txt");
            File.WriteAllText(path, gameSaveText);
        }

        public List<string> LoadUnlockedChapterList()
        {
            string path = Path.Combine(_configDirPath, "unlocked_chapter.txt");
            if (!File.Exists(path)) return new List<string>();

            var fileText = File.ReadAllText(path);

            return VNChapter.ExtractUnlockedChapterList(fileText);
        }

        public void SaveUnlockedChapterList()
        {
            string path = Path.Combine(_configDirPath, "unlocked_chapter.txt");

            var unlockedChapterList = this.GetModel<ChapterModel>().UnlockedChapterList;
            var text = VNChapter.UnlockedChapterListToText(unlockedChapterList);

            File.WriteAllText(path, text);
        }

        public void LoadSystemConfig()
        {
            string path = Path.Combine(_configDirPath, "game_config.txt");
            var systemConfigModel = this.GetModel<ConfigModel>();
            var defaultConfig = new Dictionary<string, object>
            {
                { "bgm_volume", 0.8f },
                { "bgs_volume", 0.5f },
                { "chs_volume", 1.0f },
                { "gms_volume", 0.4f },
                { "text_speed", 0.08f },
                { "language", "Chinese"}
            };

            if (File.Exists(path))
            {
                string[] configList = File.ReadAllLines(path);

                foreach (var config in configList.Select(ch => ch.Split(":").Select(str => str.Trim())))
                {
                    string key = config.ElementAtOrDefault(0);
                    string value = config.ElementAtOrDefault(1);

                    if (defaultConfig.ContainsKey(key) && float.TryParse(value, out float floatValue))
                    {
                        defaultConfig[key] = floatValue;
                    }
                }
            }

            // 将配置值设置到 systemConfigModel 中
            systemConfigModel.BgmVolume = (float)defaultConfig["bgm_volume"];
            systemConfigModel.BgsVolume = (float)defaultConfig["bgs_volume"];
            systemConfigModel.ChsVolume = (float)defaultConfig["chs_volume"];
            systemConfigModel.GmsVolume = (float)defaultConfig["gms_volume"];
            systemConfigModel.TextSpeed = (float)defaultConfig["text_speed"];
            systemConfigModel.Language = (string)defaultConfig["language"];

            SaveSystemConfig();
        }

        public void SaveSystemConfig()
        {
            string path = Path.Combine(_configDirPath, "game_config.txt");

            var systemConfigModel = this.GetModel<ConfigModel>();
            var configStr = @$"bgm_volume : {systemConfigModel.BgmVolume}
bgs_volume : {systemConfigModel.BgsVolume}
chs_volume : {systemConfigModel.ChsVolume}
gms_volume : {systemConfigModel.GmsVolume}
text_speed : {systemConfigModel.TextSpeed}
language : {systemConfigModel.Language}"
;

            if (!Directory.Exists(_configDirPath)) Directory.CreateDirectory(_configDirPath);

            File.WriteAllText(path, configStr);
        }

        public List<ChapterInfo> LoadChapterInfoList()
        {
            string content = _abDic["vnscript"].LoadAsset<TextAsset>("chapter_info").text;
            var chapterInfoList = VNChapter.ExtractChapterInfo(content);

            return chapterInfoList;
        }

        public void LoadProjectConfig()
        {
            var configFile = _abDic["projectdata"].LoadAsset<TextAsset>("game_info").text;

            var gameInfoModel = this.GetModel<ProjectModel>();

            var gameInfo = VNProjectConfig.ExtractProjectConfig(configFile);
            foreach (var (blockType, property) in gameInfo)
            {
                if (blockType == VNProjectConfig.ProjectConfigType.TitleView)
                {
                    gameInfoModel.TitleViewLogo = property["logo"];
                    gameInfoModel.TitleViewBgm = property["bgm"];
                    gameInfoModel.TitleViewBgp = property["bgp"];
                }
                else if (blockType == VNProjectConfig.ProjectConfigType.GameSaveView)
                {
                    gameInfoModel.GameSaveViewBgm = property["bgm"];
                    gameInfoModel.GameSaveViewBgp = property["bgp"];
                    gameInfoModel.GameSaveViewGalleryItemPic = property["gallery_item_pic"];
                    gameInfoModel.GameSaveViewGalleryListPic = property["gallery_list_pic"];
                }
                else if (blockType == VNProjectConfig.ProjectConfigType.PerformanceView)
                {
                    gameInfoModel.NormDialogueBoxPic = property["norm_dialogue_box_pic"];
                    gameInfoModel.FullDialogueBoxPic = property["full_dialogue_box_pic"];
                    gameInfoModel.NormNameBoxPic = property["norm_name_box_pic"];
                    gameInfoModel.PerformanceViewMenuViewButtonPic = property["menu_view_button_pic"];
                    gameInfoModel.PerformanceViewBacklogViewButtonPic = property["backlog_view_button_pic"];
                    gameInfoModel.PerformanceViewConfigViewButtonPic = property["config_view_button_pic"];
                    gameInfoModel.PerformanceViewSaveGameSaveViewButtonPic = property["save_view_button_pic"];
                }
                else if (blockType == VNProjectConfig.ProjectConfigType.BacklogView)
                {
                    gameInfoModel.BacklogViewBgp = property["bgp"];
                    gameInfoModel.BacklogViewTextColor = property["text_color"];
                    gameInfoModel.BacklogViewBackButtonTextColor = property["back_button_text_color"];
                }
            }
        }

        public string LoadI18nRes(string I18nName)
        {
            if (I18nName == "")
            {
                Debug.Log("<color=red>I18n Name is Null</color>");
                return null;
            }

            var ret = _abDic["settings"].LoadAsset<TextAsset>(I18nName).text;
            if (ret == null) Debug.Log($"<color=red>I18n {I18nName} Not Found</color>");

            return ret;
        }

        public TextAsset LoadDefaultKeyboard()
        {
            var ret = _abDic["settings"].LoadAsset<TextAsset>("Keyboard");
            if (ret == null) Debug.Log($"<color=red>Default Keyboard Not Found</color>");

            return ret;
        }

        public GameObject LoadPrefab(string prefabName)
        {
            GameObject obj = _abDic["prefab"].LoadAsset<GameObject>(prefabName);

            if (obj == null) Debug.Log($"<color=red>Prefab 「{prefabName}」 Not Found</color>");

            return obj;
        }

        public void LoadAllRes()
        {
            string resPath = Application.streamingAssetsPath + "/";

            LoadRes("vnscript", resPath + "vnscript");
            LoadRes("sound", resPath + "sound");
            LoadRes("sprite", resPath + "sprite");
            LoadRes("projectdata", resPath + "projectdata");
            LoadRes("prefab", resPath + "prefab");
            LoadRes("settings", resPath + "settings");
        }

        private void LoadRes(string assetBundleName, string path)
        {
            if (_abDic.ContainsKey(assetBundleName)) return;
            {
                // 进行加载
                AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
                if (assetBundle != null)
                {
                    // 将AssetBundle添加到管理器中
                    _abDic.Add(assetBundleName, assetBundle);
                }
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}