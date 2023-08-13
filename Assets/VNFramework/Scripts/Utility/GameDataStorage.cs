using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using VNFramework.Core;

namespace VNFramework
{
    class GameDataStorage : IUtility, ICanGetModel, ICanGetUtility
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
            if (ret) Debug.Log($"<color=red>AudioClip {audioName} Not Found</color>");

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
            if (ret) Debug.Log($"<color=red>Sprite {spriteName} not found</color>");

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
            File.WriteAllText(path,gameSaveText);
        }

        public List<string> LoadUnlockedChapterList()
        {
            Debug.Log(string.Format("<color=green>{0}</color>", "Load Unlocked Chapter List"));
            string path = Path.Combine(_configDirPath, "unlocked_chapter.txt");
            if (!File.Exists(path)) return new List<string>();

            var fileText = File.ReadAllText(path);

            return VNChapter.ExtractUnlockedChapterList(fileText);
        }

        public void SaveUnlockedChapterList()
        {
            Debug.Log(string.Format("<color=green>{0}</color>","Save Unlocked Chapter List"));
            string path = Path.Combine(_configDirPath, "unlocked_chapter.txt");
            
            var unlockedChapterList = this.GetModel<ChapterModel>().UnlockedChapterList;
            var text = VNChapter.UnlockedChapterListToText(unlockedChapterList);

            File.WriteAllText(path, text);
        }

        public void LoadSystemConfig()
        {
            string path = Path.Combine(_configDirPath, "game_config.txt");
            var systemConfigModel = this.GetModel<ConfigModel>();
            Dictionary<string, float> defaultConfig = new Dictionary<string, float>
            {
                { "bgm_volume", 0.8f },
                { "bgs_volume", 0.5f },
                { "chs_volume", 1.0f },
                { "gms_volume", 0.4f },
                { "text_speed", 0.08f }
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
            systemConfigModel.BgmVolume = defaultConfig["bgm_volume"];
            systemConfigModel.BgsVolume = defaultConfig["bgs_volume"];
            systemConfigModel.ChsVolume = defaultConfig["chs_volume"];
            systemConfigModel.GmsVolume = defaultConfig["gms_volume"];
            systemConfigModel.TextSpeed = defaultConfig["text_speed"];

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
text_speed : {systemConfigModel.TextSpeed}";

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

        public GameObject LoadPrefab(string prefabName)
        {
            GameObject obj = _abDic["prefab"].LoadAsset<GameObject>(prefabName);

            if (obj == null) Debug.Log("AB Prefab Resources Not Found");

            return obj;
        }

        public void LoadAllRes()
        {
            string resPath = Application.streamingAssetsPath + "/";

            _abDic.Add("vnscript", AssetBundle.LoadFromFile(resPath + "vnscript"));
            _abDic.Add("sound", AssetBundle.LoadFromFile(resPath + "sound"));
            _abDic.Add("sprite", AssetBundle.LoadFromFile(resPath + "sprite"));
            _abDic.Add("projectdata", AssetBundle.LoadFromFile(resPath + "projectdata"));
            _abDic.Add("prefab", AssetBundle.LoadFromFile(resPath + "prefab"));
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}