using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace VNFramework
{
    class GameDataStorage : IUtility, ICanGetModel
    {
        Dictionary<string, AssetBundle> abDic = new();
        public AudioClip LoadSound(string audioName)
        {
            var ret = abDic["sounds"].LoadAsset<AudioClip>(audioName);
            if (ret == null) Debug.LogError(string.Format("AudioClip {0} not found", audioName));

            return ret;
        }

        public Sprite LoadSprite(string path)
        {
            var ret = abDic["sprites"].LoadAsset<Sprite>(path);
            if (ret == null) Debug.LogError(string.Format("Sprite {0} not found", path));

            return ret;
        }

        public string[] LoadVNScript(string scriptName)
        {
            string[] fileLines = abDic["vnscripts"].LoadAsset<TextAsset>(scriptName).text.Split('\n');
            string[] vnScriptLines = fileLines.Select(str => str.TrimEnd('\r', '\n')).ToArray();

            return vnScriptLines;
        }

        public void LoadSystemConfig()
        {
            var configFilePath = Path.Combine(Application.dataPath, "Config", "game_config.txt");
            var systemConfigModel = this.GetModel<ConfigModel>();
            // 若文件不存在，则使用默认配置
            if (!File.Exists(configFilePath))
            {
                systemConfigModel.BgmVolume = 0.8f;
                systemConfigModel.BgsVolume = 0.5f;
                systemConfigModel.ChsVolume = 1.0f;
                systemConfigModel.GmsVolume = 0.4f;
                systemConfigModel.TextSpeed = 0.08f;
                SaveSystemConfig();
            }

            string[] configList = File.ReadAllLines(configFilePath);

            foreach (var config in configList.Select(ch => ch.Split(":").Select(str => str.Trim())))
            {
                string key = config.ElementAtOrDefault(0);
                string value = config.ElementAtOrDefault(1);

                if (key == "bgm_volume") systemConfigModel.BgmVolume = Convert.ToSingle(value);
                else if (key == "bgs_volume") systemConfigModel.BgsVolume = Convert.ToSingle(value);
                else if (key == "chs_volume") systemConfigModel.ChsVolume = Convert.ToSingle(value);
                else if (key == "gms_volume") systemConfigModel.GmsVolume = Convert.ToSingle(value);
                else if (key == "text_speed") systemConfigModel.TextSpeed = Convert.ToSingle(value);
            }
        }

        public void SaveSystemConfig()
        {
            var configFolderPath = Path.Combine(Application.dataPath, "Config");
            var configFilePath = Path.Combine(configFolderPath, "game_config.txt");

            var systemConfigModel = this.GetModel<ConfigModel>();
            var configStr = @$"bgm_volume : {systemConfigModel.BgmVolume}
bgs_volume : {systemConfigModel.BgsVolume}
chs_volume : {systemConfigModel.ChsVolume}
gms_volume : {systemConfigModel.GmsVolume}
text_speed : {systemConfigModel.TextSpeed}";

            // Create the directory if it does not exist
            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
            }

            // Save the configuration to the file
            using (StreamWriter sw = new StreamWriter(configFilePath))
            {
                sw.Write(configStr);
            }
        }

        /// <summary>
        /// 加载已解锁的游戏章节的名称
        /// </summary>
        public string[] LoadUnlockedChapterList()
        {
            var unlockConfigPath = Path.Combine(Application.dataPath, "Config", "chapter_record.txt");

            // 若存档文件不存在，则在指定目录创建存档文件
            if (!File.Exists(unlockConfigPath))
            {
                var infoList = LoadChapterInfoList();
                using (StreamWriter sw = new StreamWriter(unlockConfigPath))
                {
                    sw.Write($"[ chapter_name : {infoList[0].ChapterName} ]");
                }
            }

            string fileContent = File.ReadAllText(unlockConfigPath);

            string pattern = @"\[\s*chapter_name\s:\s*(.*?)\s*\]";
            MatchCollection matches = Regex.Matches(fileContent, pattern);

            var unlockedList = matches
                .Cast<Match>()
                .Select(match => match.Groups[1].Value.Trim())
                .ToArray();

            return unlockedList;
        }

        public ChapterInfo[] LoadChapterInfoList()
        {
            string content = abDic["vnscripts"].LoadAsset<TextAsset>("chapter_info").text;

            string pattern = @"<\|\s*(\[.*?\])\s*\|>";
            MatchCollection matches = Regex.Matches(content, pattern, RegexOptions.Singleline);

            ChapterInfo[] chapterInfoList = matches
                .Cast<Match>()
                .Select(match => ParseChapterInfo(match.Groups[1].Value))
                .ToArray();

            return chapterInfoList;
        }

        public void SaveUnlockedChapterList()
        {
            var unlockConfigPath = Path.Combine(Application.dataPath, "Config", "chapter_record.txt");
            var unlockedChapterList = this.GetModel<ChapterModel>().UnlockedChapterList;

            StringBuilder sb = new();

            foreach (var chapterName in unlockedChapterList)
            {
                sb.Append($"[ chapter_name : {chapterName} ]\n");
            }

            if (File.Exists(unlockConfigPath))
            {
                File.CreateText(unlockConfigPath);
            }

            using (StreamWriter sw = new StreamWriter(unlockConfigPath))
            {
                sw.Write(sb);
            }
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

                if (key == "title") projectModel.Title = value;
                else if (key == "start_view_bgm") projectModel.TitleBgm = value;
                else if (key == "start_view_bgp") projectModel.TitleBgp = value;
            }
        }

        public GameObject LoadPrefab(string prefabName)
        {
            GameObject obj = abDic["prefabs"].LoadAsset<GameObject>(prefabName);

            if (obj == null) Debug.Log("AB Prefab Resources Not Found");

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