using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;
using VNFramework.Core;

namespace VNFramework
{
    class GameDataStorage : IUtility, ICanGetModel, ICanGetUtility
    {
        private string _configDirPath = Path.Combine(Application.dataPath, "Config");
        private string _systemConfigPath = Path.Combine(Application.dataPath, "Config", "game_config.txt");

        Dictionary<string, AssetBundle> abDic = new();
        public AudioClip LoadSound(string audioName)
        {
            var ret = abDic["sound"].LoadAsset<AudioClip>(audioName);
            if (ret == null) Debug.Log(string.Format("AudioClip {0} not found", audioName));

            return ret;
        }

        public Sprite LoadSprite(string path)
        {
            if (path == "")
            {
                Debug.Log("Sprite Name Is Null");
                return null;
            }
            var ret = abDic["sprite"].LoadAsset<Sprite>(path);
            if (ret == null) Debug.Log(string.Format("Sprite {0} not found", path));

            return ret;
        }

        public string[] LoadVNScript(string scriptName)
        {
            string[] fileLines = abDic["vnscript"].LoadAsset<TextAsset>(scriptName).text.Split('\n');
            string[] vnScriptLines = fileLines.Select(str => str.TrimEnd('\r', '\n')).ToArray();

            return vnScriptLines;
        }

        public string LoadVNMermaid(string name)
        {
            var file = abDic["vnscript"].LoadAsset<TextAsset>(name).text;

            return file;
        }

        public GameSave[] LoadGameSave()
        {
            string path = Path.Combine(_configDirPath, "save_file.txt");
            if (!File.Exists(path)) return new GameSave[60];

            string gameSaveText = File.ReadAllText(path);
            string pattern = @"<\|\s*(\[.*?\])\s*\|>";
            MatchCollection matches = Regex.Matches(gameSaveText, pattern, RegexOptions.Singleline);

            GameSave[] curGameSaves = matches
                .Select(match => ParseGameSaveItem(match.Groups[1].Value))
                .ToArray();

            var gameSaves = new GameSave[60];

            foreach (var save in curGameSaves)
            {
                var index = save.SaveIndex;
                gameSaves[index] = save;
            }

            return gameSaves;
        }

        private GameSave ParseGameSaveItem(string blockContent)
        {
            blockContent = blockContent.Trim();
            string pattern = @"\[\s*(save_index|save_date|mermaid_node|resume_pic|resume_text)\s*:\s*(.*?)\s*\]";
            MatchCollection matches = Regex.Matches(blockContent, pattern, RegexOptions.Singleline);

            var gameSave = new GameSave();

            foreach (Match match in matches.Cast<Match>())
            {
                string key = match.Groups[1].Value;
                string value = match.Groups[2].Value;

                switch (key)
                {
                    case "save_index": gameSave.SaveIndex = Convert.ToInt32(value.Trim()); break;
                    case "save_date": gameSave.SaveDate = value.Trim(); break;
                    case "mermaid_node": gameSave.MermaidNode = value.Trim(); break;
                    case "script_index": gameSave.VNScriptIndex = Convert.ToInt32(value.Trim()); break;
                    case "resume_pic": gameSave.ResumePic = value.Trim(); break;
                    case "resume_text": gameSave.ResumeText = value.Trim(); break;
                }
            }

            return gameSave;
        }

        public void SaveGameSave()
        {
            var gameSaves = this.GetModel<GameSaveModel>().GameSaves;
            var sb = new StringBuilder();
            for (int i = 0; i < gameSaves.Length; i++)
            {
                if (gameSaves[i] != null && !string.IsNullOrWhiteSpace(gameSaves[i].SaveDate))
                {
                    sb.Append(@$"<|
    [ save_index: {i} ]
    [ save_date: {gameSaves[i].SaveDate} ]
    [ mermaid_node: {gameSaves[i].MermaidNode} ]
    [ script_index: {gameSaves[i].VNScriptIndex} ]
    [ resume_pic: {gameSaves[i].ResumePic} ]
    [ resume_text: {gameSaves[i].ResumeText} ]
|>
");
                }
            }

            var gameSaveText = sb.ToString();
            var path = Path.Combine(_configDirPath, "save_file.txt");
            File.WriteAllText(path,gameSaveText);
        }

        public List<string> LoadUnlockedChapterList()
        {
            Debug.Log(string.Format("<color=green>{0}</color>", "Load Unlocked Chapter List"));
            string path = Path.Combine(_configDirPath, "unlocked_chapter.txt");
            if (!File.Exists(path)) return new List<string>();

            var fileText = File.ReadAllText(path);

            var unlockedChapterList = new List<string>();
            string pattern = @"\[\s*mermaid_name\s*:\s*(.*?)\s*\]";
            MatchCollection matches = Regex.Matches(fileText, pattern, RegexOptions.Singleline);

            foreach (Match match in matches.Cast<Match>())
            {
                string value = match.Groups[1].Value;
                unlockedChapterList.Add(value);
            }

            return unlockedChapterList;
        }

        public void SaveUnlockedChapterList()
        {
            Debug.Log(string.Format("<color=green>{0}</color>","Save Unlocked Chapter List"));
            var unlockedChapterList = this.GetModel<ChapterModel>().UnlockedChapterList;

            if (unlockedChapterList.Count == 0) return;

            var sb = new StringBuilder();
            foreach(var chapter in unlockedChapterList)
            {
                sb.AppendLine($"[mermaid_name:{chapter}]");
            }

            string path = Path.Combine(_configDirPath, "unlocked_chapter.txt");
            File.WriteAllText(path, sb.ToString());
        }

        public void LoadSystemConfig()
        {
            var systemConfigModel = this.GetModel<ConfigModel>();
            Dictionary<string, float> defaultConfig = new Dictionary<string, float>
            {
                { "bgm_volume", 0.8f },
                { "bgs_volume", 0.5f },
                { "chs_volume", 1.0f },
                { "gms_volume", 0.4f },
                { "text_speed", 0.08f }
            };

            if (File.Exists(_systemConfigPath))
            {
                string[] configList = File.ReadAllLines(_systemConfigPath);

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
            var systemConfigModel = this.GetModel<ConfigModel>();
            var configStr = @$"bgm_volume : {systemConfigModel.BgmVolume}
bgs_volume : {systemConfigModel.BgsVolume}
chs_volume : {systemConfigModel.ChsVolume}
gms_volume : {systemConfigModel.GmsVolume}
text_speed : {systemConfigModel.TextSpeed}";

            if (!Directory.Exists(_configDirPath)) Directory.CreateDirectory(_configDirPath);

            File.WriteAllText(_systemConfigPath, configStr);
        }

        public List<ChapterInfo> LoadChapterInfoList()
        {
            string content = abDic["vnscript"].LoadAsset<TextAsset>("chapter_info").text;

            string pattern = @"<\|\s*(\[.*?\])\s*\|>";
            MatchCollection matches = Regex.Matches(content, pattern, RegexOptions.Singleline);

            List<ChapterInfo> chapterInfoList = matches
                .Select(match => ParseChapterInfo(match.Groups[1].Value))
                .ToList();

            return chapterInfoList;
        }

        private ChapterInfo ParseChapterInfo(string blockContent)
        {
            blockContent = blockContent.Trim();
            string pattern = @"\[\s*(mermaid_name|resume|resume_pic)\s*:\s*(.*?)\s*\]";
            MatchCollection matches = Regex.Matches(blockContent, pattern, RegexOptions.Singleline);

            ChapterInfo chapterInfo = new();

            foreach (Match match in matches.Cast<Match>())
            {
                string key = match.Groups[1].Value;
                string value = match.Groups[2].Value;

                switch (key)
                {
                    case "mermaid_name": chapterInfo.MermaidName = value.Trim(); break;
                    case "resume": chapterInfo.ResumeText = value.Trim(); break;
                    case "resume_pic": chapterInfo.ResumePic = value.Trim(); break;
                }
            }

            return chapterInfo;
        }

        public void LoadProjectData()
        {
            var configFile = abDic["projectdata"].LoadAsset<TextAsset>("game_info").text;
            
            var gameInfoModel = this.GetModel<ProjectModel>();
            
            var gameInfo = VNGameInfo.ExtractGameInfo(configFile);
            foreach (var (blockType, property) in gameInfo)
            {
                if (blockType == GameInfoType.TitleView)
                {
                    gameInfoModel.TitleViewLogo = property["logo"];
                    gameInfoModel.TitleViewBgm = property["bgm"];
                    gameInfoModel.TitleViewBgp = property["bgp"];
                }
                else if (blockType == GameInfoType.GameSaveView)
                {
                    gameInfoModel.GameSaveViewBgm = property["bgm"];
                    gameInfoModel.GameSaveViewBgp = property["bgp"];
                    gameInfoModel.GameSaveViewGalleryItemPic = property["gallery_item_pic"];
                    gameInfoModel.GameSaveViewGalleryListPic = property["gallery_list_pic"];
                }
                else if (blockType == GameInfoType.PerformanceView)
                {
                    gameInfoModel.NormDialogueBoxPic = property["norm_dialogue_box_pic"];
                    gameInfoModel.FullDialogueBoxPic = property["full_dialogue_box_pic"];
                    gameInfoModel.NormNameBoxPic = property["norm_name_box_pic"];
                    gameInfoModel.PerformanceViewMenuViewButtonPic = property["menu_view_button_pic"];
                    gameInfoModel.PerformanceViewBacklogViewButtonPic = property["backlog_view_button_pic"];
                    gameInfoModel.PerformanceViewConfigViewButtonPic = property["config_view_button_pic"];
                    gameInfoModel.PerformanceViewSaveGameSaveViewButtonPic = property["save_view_button_pic"];
                }
                else if (blockType == GameInfoType.BacklogView)
                {
                    gameInfoModel.BacklogViewBgp = property["bgp"];
                    gameInfoModel.BacklogViewTextColor = property["text_color"];
                    gameInfoModel.BacklogViewBackButtonTextColor = property["back_button_text_color"];
                }
            }
        }

        public GameObject LoadPrefab(string prefabName)
        {
            GameObject obj = abDic["prefab"].LoadAsset<GameObject>(prefabName);

            if (obj == null) Debug.Log("AB Prefab Resources Not Found");

            return obj;
        }

        public void LoadAllRes()
        {
            string resPath = Application.streamingAssetsPath + "/";

            abDic.Add("vnscript", AssetBundle.LoadFromFile(resPath + "vnscript"));
            abDic.Add("sound", AssetBundle.LoadFromFile(resPath + "sound"));
            abDic.Add("sprite", AssetBundle.LoadFromFile(resPath + "sprite"));
            abDic.Add("projectdata", AssetBundle.LoadFromFile(resPath + "projectdata"));
            abDic.Add("prefab", AssetBundle.LoadFromFile(resPath + "prefab"));
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}