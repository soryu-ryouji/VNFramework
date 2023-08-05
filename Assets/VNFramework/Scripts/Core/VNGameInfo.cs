using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VNFramework.Utils
{
    public enum GameInfoType
    {
        TitleView,
        BacklogView,
        PerformanceView,
        GameSaveView,
    }

    class VNGameInfo
    {
        public static List<(GameInfoType blockType, Dictionary<string, string> property)> ExtractGameInfo(string configText)
        {
            var result = new List<(GameInfoType blockType, Dictionary<string, string> property)>();
            var blocks = ExtractConfigBlock(configText);

            foreach (var (blockType, blockText) in blocks)
            {
                var properties = ExtractBlockProperty(blockText);
                result.Add((blockType, properties));
            }

            return result;
        }

        public static List<(GameInfoType blockType, string blockText)> ExtractConfigBlock(string configText)
        {
            string pattern = @"\[(.*?)\]\s*(.*?)\s*(?=\[|\z)";
            MatchCollection matches = Regex.Matches(configText, pattern, RegexOptions.Singleline);
            var result = new List<(GameInfoType blockType, string)>();

            foreach (Match match in matches)
            {
                string blockTypeStr = match.Groups[1].Value.Trim();
                string blockText = match.Groups[2].Value.Trim();

                GameInfoType blockType;
                if (blockTypeStr == "Title View") blockType = GameInfoType.TitleView;
                else if (blockTypeStr == "Game Save View") blockType = GameInfoType.GameSaveView;
                else if (blockTypeStr == "Backlog View") blockType = GameInfoType.BacklogView;
                else if (blockTypeStr == "Performance View") blockType = GameInfoType.PerformanceView;
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