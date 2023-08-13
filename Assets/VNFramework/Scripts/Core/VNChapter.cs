using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VNFramework.Core
{
    class VNChapter
    {
        public static List<string> ExtractUnlockedChapterList(string text)
        {
            var unlockedChapterList = new List<string>();
            string pattern = @"\[\s*mermaid_name\s*:\s*(.*?)\s*\]";
            MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.Singleline);

            foreach (Match match in matches.Cast<Match>())
            {
                string value = match.Groups[1].Value;
                unlockedChapterList.Add(value);
            }

            return unlockedChapterList;
        }

        public static string UnlockedChapterListToText(List<string> unlockedChapterList)
        {
            if (unlockedChapterList.Count == 0) return "";

            var sb = new StringBuilder();
            foreach (var chapter in unlockedChapterList)
            {
                sb.AppendLine($"[mermaid_name:{chapter}]");
            }

            return sb.ToString();
        }


        public static List<ChapterInfo> ExtractChapterInfo(string text)
        {
            string pattern = @"<\|\s*(\[.*?\])\s*\|>";
            MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.Singleline);

            List<ChapterInfo> chapterInfoList = matches
                .Select(match => ParseChapterInfo(match.Groups[1].Value))
                .ToList();
            
            return chapterInfoList;
        }

        private static ChapterInfo ParseChapterInfo(string blockContent)
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
    }
}