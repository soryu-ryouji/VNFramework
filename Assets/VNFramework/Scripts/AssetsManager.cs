using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

namespace VNFramework
{
    public class AssetsManager : MonoBehaviour
    {
        public static AudioClip LoadSound(string path)
        {
            var ret = Resources.Load<AudioClip>(path);
            if (ret == null)
            {
                Debug.LogError(string.Format("AudioClip {0} not found", path));
            }

            return ret;
        }

        public static Sprite LoadSprite(string path)
        {
            var ret = Resources.Load<Sprite>(path);
            if (ret == null)
            {
                Debug.LogError(string.Format("Sprite {0} not found", path));
            }

            return ret;
        }

        public static List<string> LoadVNScript(string scriptName)
        {
            string[] file = Resources.Load<TextAsset>(scriptName).text.Split('\n');
            var lines = new List<string>();
            for (int i = 0; i < file.Length; i++)
            {
                lines.Add(file[i].TrimEnd('\r', '\n'));
            }

            return lines;
        }

        public static Hashtable LoadGameConfig()
        {
            var configFilePath = Path.Combine(Application.dataPath, "Config", "game_config.txt");
            string[] configList = File.ReadAllLines(configFilePath);


            Hashtable hash = new();

            for (int i = 0; i < configList.Length; i++)
            {
                var configUnit = configList[i].Split(":");

                // 清除前后空格
                for (int n = 0; n < configUnit.Length; n++)
                {
                    configUnit[n] = configUnit[n].Trim();
                }

                Debug.Log($"{configUnit[0]} : {configUnit[1]}");

                if (configUnit[0] == "bgm_volume") hash.Add("bgm_volume", Convert.ToSingle(configUnit[1]));
                else if (configUnit[0] == "bgs_volume") hash.Add("bgs_volume", Convert.ToSingle(configUnit[1]));
                else if (configUnit[0] == "chs_volume") hash.Add("chs_volume", Convert.ToSingle(configUnit[1]));
                else if (configUnit[0] == "gms_volume") hash.Add("gms_volume", Convert.ToSingle(configUnit[1]));
                else if (configUnit[0] == "text_speed") hash.Add("text_speed", Convert.ToSingle(configUnit[1]));
            }

            return hash;
        }

        public static void SaveGameConfig(string config)
        {
            var configFilePath = Path.Combine(Application.dataPath, "Config", "game_config.txt");
            using (StreamWriter sw = new StreamWriter(configFilePath))
            {
                sw.Write(config);
            }
        }

        public static List<string> LoadChapterRecord()
        {
            var recordFilePath = Path.Combine(Application.dataPath, "Config", "chapter_record.txt");
            if (File.Exists(recordFilePath) == false)
            {
                var chapterInfoList = LoadChapterInfo();

                using (StreamWriter sw = new StreamWriter(recordFilePath))
                {
                    sw.WriteLine("[ chapter_name: " + chapterInfoList[0].ChapterName + " ]");
                }

                return new List<string> { chapterInfoList[0].ChapterName };
            }

            string fileContent = File.ReadAllText(recordFilePath);

            string pattern = @"\[\s*chapter_name:\s*(.*?)\s*\]";
            MatchCollection matches = Regex.Matches(fileContent, pattern);

            var chapterRecord = new List<string>();

            foreach (Match match in matches)
            {
                string extractedText = match.Groups[1].Value.Trim();
                chapterRecord.Add(extractedText);
            }

            return chapterRecord;
        }

        public static string GetFileNameFromChapterName(string chapterName)
        {
            var chapterInfoList = LoadChapterInfo();

            foreach (var chapterInfo in chapterInfoList)
            {
                if (chapterInfo.ChapterName == chapterName)
                {
                    return chapterInfo.FileName;
                }
            }

            Debug.Log("chapter name not found");
            return null;
        }


        public class ChapterInfo
        {
            public string ChapterName { get; set; }
            public string FileName { get; set; }
            public string Resume { get; set; }
            public string ResumePic { get; set; }

            public void Print()
            {
                Console.WriteLine("chapter_name : " + ChapterName);
                Console.WriteLine("file_name : " + FileName);
                Console.WriteLine("resume: " + Resume);
                Console.WriteLine("resume_pic: " + ResumePic);
            }
        }

        public static List<ChapterInfo> LoadChapterInfo()
        {
            var filePath = Path.Combine(Application.dataPath,"Config","chapter_info.txt");
            string content = File.ReadAllText(filePath);

            string pattern = @"<\|\s*(\[.*?\])\s*\|>";
            MatchCollection matches = Regex.Matches(content, pattern, RegexOptions.Singleline);

            List<ChapterInfo> parameterLists = new();

            foreach (Match match in matches.Cast<Match>())
            {
                string blockContent = match.Groups[1].Value;

                var para = ParseChapterInfo(blockContent);
                parameterLists.Add(para);
            }

            return parameterLists;
        }

        private static ChapterInfo ParseChapterInfo(string blockContent)
        {
            blockContent = blockContent.Trim();
            string pattern = @"\[\s*(chapter_name|file_name|resume|resume_pic)\s*:\s*(.*?)\s*\]";
            MatchCollection matches = Regex.Matches(blockContent, pattern, RegexOptions.Singleline);

            ChapterInfo parameter = new();

            foreach (Match match in matches.Cast<Match>())
            {
                string key = match.Groups[1].Value;
                string value = match.Groups[2].Value;

                switch (key)
                {
                    case "chapter_name": parameter.ChapterName = value.Trim(); break;
                    case "file_name": parameter.FileName = value.Trim(); break;
                    case "resume": parameter.Resume = value.Trim(); break;
                    case "resume_pic": parameter.ResumePic = value.Trim(); break;
                }
            }

            return parameter;
        }

        public static ChapterInfo GetChapterInfoFromChapterName(string name)
        {
            var chapterInfoList = LoadChapterInfo();
            for (int i = 0; i < chapterInfoList.Count; i++)
            {
                if (chapterInfoList[i].ChapterName == name)
                {
                    return chapterInfoList[i];
                }
            }

            Debug.Log("chapter name not found");
            return null;
        }

        public static void SaveChapterRecord()
        {
            var record = LoadChapterRecord();
            Debug.Log("Save Chapter Record : Old Recrod = " + string.Join(",", record.ToArray()));

            // 当前章节是不最新章节，不需要更新
            if (record[record.Count - 1] != ConfigController.CurrentChapterName) return;

            var chapterInfo = LoadChapterInfo();

            Debug.Log("Load Chapter Info");

            for (int i = 0; i < chapterInfo.Count; i++)
            {
                // 当前章节不是最后一章，更新记录
                if (chapterInfo[i].ChapterName == ConfigController.CurrentChapterName && i+1 != chapterInfo.Count)
                {
                    record.Add(chapterInfo[i + 1].ChapterName);
                    break;
                }
            }

            string recordFilePath = Path.Combine(Application.dataPath, "Config", "chapter_record.txt");
            using (StreamWriter sw = new StreamWriter(recordFilePath))
            {
                foreach (var chapterName in record)
                {
                    sw.WriteLine("[ chapter_name: " + chapterName + " ]");
                }
            }
        }
    }
}