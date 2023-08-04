using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace VNFramework
{
    class GameDataStorage : IUtility, ICanGetModel, ICanGetUtility
    {
        private string _configDirPath = Path.Combine(Application.dataPath, "Config");
        private string _systemConfigPath = Path.Combine(Application.dataPath, "Config", "game_config.txt");
        private string _chapterRecordPath = Path.Combine(Application.dataPath, "Config", "chapter_record.txt");

        Dictionary<string, AssetBundle> abDic = new();
        public AudioClip LoadSound(string audioName)
        {
            var ret = abDic["sounds"].LoadAsset<AudioClip>(audioName);
            if (ret == null) this.GetUtility<GameLog>().ErrorLog(string.Format("AudioClip {0} not found", audioName));

            return ret;
        }

        public Sprite LoadSprite(string path)
        {
            var ret = abDic["sprites"].LoadAsset<Sprite>(path);
            if (ret == null) this.GetUtility<GameLog>().ErrorLog(string.Format("Sprite {0} not found", path));

            return ret;
        }

        public string[] LoadVNScript(string scriptName)
        {
            string[] fileLines = abDic["vnscripts"].LoadAsset<TextAsset>(scriptName).text.Split('\n');
            string[] vnScriptLines = fileLines.Select(str => str.TrimEnd('\r', '\n')).ToArray();

            return vnScriptLines;
        }

        public string LoadVNMermaid(string name)
        {
            var file = abDic["vnscripts"].LoadAsset<TextAsset>(name).text;

            return file;
        }

        public SaveFile[] LoadSaveFiles()
        {
            var saveFiles = new SaveFile[60];

            for (int i = 0; i < saveFiles.Length; i++)
            {
                saveFiles[i] = new SaveFile();
            }

            var saveFile = File.ReadAllText(Path.Combine(_configDirPath, "save_file.txt"));

            var regex = new Regex(@"<\|\s*save_index:(\d+)\s*save_date:(.*?)\s*mermaid_node:(.*?)\s*script_index:(\d+)\s*resume_pic:(.*?)\s*resume_text:(.*?)\s*\|>",
            RegexOptions.Singleline);
            var matches = regex.Matches(saveFile);

            foreach (Match match in matches)
            {
                SaveFile save = new SaveFile();
                int index = int.Parse(match.Groups[1].Value);
                save.SaveDate = match.Groups[2].Value.Trim();
                save.MermaidNode = match.Groups[3].Value.Trim();
                save.VNScriptIndex = int.Parse(match.Groups[4].Value);
                save.ResumePic = match.Groups[5].Value.Trim();
                save.ResumeText = match.Groups[6].Value.Trim();

                saveFiles[index] = save;
            }

            return saveFiles;
        }

        public void LoadSystemConfig()
        {
            this.GetUtility<GameLog>().RunningLog("Load System Config");
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

        /// <summary>
        /// 加载已解锁的游戏章节的名称
        /// </summary>
        public string[] LoadUnlockedChapterList()
        {
            // 若存档文件不存在，则在指定目录创建存档文件
            if (!File.Exists(_chapterRecordPath)) CreateBasicUnlockedRecordFile();

            string fileContent = File.ReadAllText(_chapterRecordPath);

            string pattern = @"\[\s*chapter_name\s:\s*(.*?)\s*\]";
            MatchCollection matches = Regex.Matches(fileContent, pattern);

            var unlockedList = matches
                .Select(match => match.Groups[1].Value.Trim())
                .ToArray();

            // 如果unlockedList结果为空，则将文件覆盖并重新读取
            if (unlockedList.Length == 0)
            {
                CreateBasicUnlockedRecordFile();
                fileContent = File.ReadAllText(_chapterRecordPath);
                matches = Regex.Matches(fileContent, pattern);
                unlockedList = matches
                    .Select(match => match.Groups[1].Value.Trim())
                    .ToArray();
            }

            return unlockedList;
        }

        private void CreateBasicUnlockedRecordFile()
        {
            var infoList = LoadChapterInfoList();
            try
            {
                File.WriteAllText(_chapterRecordPath, $"[ chapter_name : {infoList[0].ChapterName} ]");
            }
            catch (Exception ex)
            {
                // 处理可能的异常，比如权限问题等
                Console.WriteLine($"Error creating unlock config file: {ex.Message}");
            }
        }

        public ChapterInfo[] LoadChapterInfoList()
        {
            string content = abDic["vnscripts"].LoadAsset<TextAsset>("chapter_info").text;

            string pattern = @"<\|\s*(\[.*?\])\s*\|>";
            MatchCollection matches = Regex.Matches(content, pattern, RegexOptions.Singleline);

            ChapterInfo[] chapterInfoList = matches
                .Select(match => ParseChapterInfo(match.Groups[1].Value))
                .ToArray();

            return chapterInfoList;
        }

        public void SaveUnlockedChapterList()
        {
            var unlockedChapterList = this.GetModel<ChapterModel>().UnlockedChapterList;

            StringBuilder sb = new();
            var linesToWrite = unlockedChapterList.Select(chapterName => $"[ chapter_name : {chapterName} ]");
            File.WriteAllLines(_chapterRecordPath, linesToWrite);
        }

        private ChapterInfo ParseChapterInfo(string blockContent)
        {
            blockContent = blockContent.Trim();
            string pattern = @"\[\s*(chapter_name|file_name|resume|resume_pic)\s*:\s*(.*?)\s*\]";
            MatchCollection matches = Regex.Matches(blockContent, pattern, RegexOptions.Singleline);

            ChapterInfo chapterInfo = new();

            foreach (Match match in matches.Cast<Match>())
            {
                string key = match.Groups[1].Value;
                string value = match.Groups[2].Value;

                switch (key)
                {
                    case "chapter_name": chapterInfo.ChapterName = value.Trim(); break;
                    case "file_name": chapterInfo.FileName = value.Trim(); break;
                    case "resume": chapterInfo.Resume = value.Trim(); break;
                    case "resume_pic": chapterInfo.ResumePic = value.Trim(); break;
                }
            }

            return chapterInfo;
        }

        public void LoadProjectData()
        {
            var configFile = abDic["game_data"].LoadAsset<TextAsset>("game_info").text.Split('\n');
            string[] configList = configFile.Select(str => str.TrimEnd('\r', '\n')).ToArray();

            var projectModel = this.GetModel<ProjectModel>();
            foreach (var config in configList.Select(ch => ch.Split(":").Select(str => str.Trim())))
            {
                string key = config.ElementAtOrDefault(0);
                string value = config.ElementAtOrDefault(1);

                if (key == "title") projectModel.TitlePic = value;
                else if (key == "start_view_bgm") projectModel.TitleBgm = value;
                else if (key == "start_view_bgp") projectModel.TitleBgp = value;
            }
        }

        public GameObject LoadPrefab(string prefabName)
        {
            GameObject obj = abDic["prefabs"].LoadAsset<GameObject>(prefabName);

            if (obj == null) this.GetUtility<GameLog>().ErrorLog("AB Prefab Resources Not Found");

            return obj;
        }

        public void LoadAllRes()
        {
            string resPath = Application.streamingAssetsPath + "/";

            abDic.Add("vnscripts", AssetBundle.LoadFromFile(resPath + "vnscripts"));
            abDic.Add("sounds", AssetBundle.LoadFromFile(resPath + "sounds"));
            abDic.Add("sprites", AssetBundle.LoadFromFile(resPath + "sprites"));
            abDic.Add("game_data", AssetBundle.LoadFromFile(resPath + "game_data"));
            abDic.Add("prefabs", AssetBundle.LoadFromFile(resPath + "prefabs"));
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}