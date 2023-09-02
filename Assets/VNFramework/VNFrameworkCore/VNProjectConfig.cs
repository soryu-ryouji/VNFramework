using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VNFramework.Core
{
    class VNProjectConfig
    {
        public enum ProjectConfigType
        {
            TitleView,
            BacklogView,
            PerformanceView,
            GameSaveView,
        }

        public static List<(ProjectConfigType blockType, Dictionary<string, string> property)> ExtractProjectConfig(string configText)
        {
            var result = new List<(ProjectConfigType blockType, Dictionary<string, string> property)>();
            var blocks = ExtractConfigBlock(configText);

            foreach (var (blockType, blockText) in blocks)
            {
                var properties = ExtractBlockProperty(blockText);
                result.Add((blockType, properties));
            }

            return result;
        }

        public static List<(ProjectConfigType blockType, string blockText)> ExtractConfigBlock(string configText)
        {
            string pattern = @"\[(.*?)\]\s*(.*?)\s*(?=\[|\z)";
            MatchCollection matches = Regex.Matches(configText, pattern, RegexOptions.Singleline);
            var result = new List<(ProjectConfigType blockType, string)>();

            foreach (Match match in matches)
            {
                string blockTypeStr = match.Groups[1].Value.Trim();
                string blockText = match.Groups[2].Value.Trim();

                ProjectConfigType blockType;
                if (blockTypeStr == "Title View") blockType = ProjectConfigType.TitleView;
                else if (blockTypeStr == "Game Save View") blockType = ProjectConfigType.GameSaveView;
                else if (blockTypeStr == "Backlog View") blockType = ProjectConfigType.BacklogView;
                else if (blockTypeStr == "Performance View") blockType = ProjectConfigType.PerformanceView;
                else continue;

                result.Add((blockType, blockText));
            }

            return result;
        }

        public static Dictionary<string, string> ExtractBlockProperty(string configLine)
        {
            var result = new Dictionary<string, string>();

            string propertyPattern = @"(?m)^\s*(.*?):\s*(.*?)\s*$";
            MatchCollection matches = Regex.Matches(configLine, propertyPattern, RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                string propertyName = match.Groups[1].Value.Trim();
                string propertyValue = match.Groups[2].Value.Trim();

                result.Add(propertyName, propertyValue);
            }

            return result;
        }
    }
}