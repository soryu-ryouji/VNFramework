using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace VNFramework.ScriptCompiler
{
    public class AsmScript
    {
        /// <summary>
        /// 解析Asm Command，将其拆分为Hash
        /// </summary>
        /// <param name="command"></param>
        public static Hashtable ParseAsmToHash(string command)
        {
            var unit = command.Split(' ');

            if (unit[0] == "dialogue") return DialogueAsmToHash(unit);
            else if (unit[0] == "name") return NameAsmToHash(unit);
            else if (unit[0] == "gm") return GmAsmToHash(unit);
            else if (unit[0] == "bgm") return BgmAsmToHash(unit);
            else if (unit[0] == "bgs") return BgsAsmToHash(unit);
            else if (unit[0] == "chs") return ChsAsmToHash(unit);

            else if (unit[0] == "bgp") return BgpAsmToHash(unit);
            else if (unit[0] == "ch_mid") return ChmAsmToHash(unit);
            else throw new Exception($"Unknown Asm Command: {command}");
        }

        private static Hashtable DialogueAsmToHash(string[] unit)
        {
            var hashtable = new Hashtable { { "object", "dialogue" } };

            if (unit[1] == "append")
            {
                hashtable.Add("action", "append");
                hashtable.Add("dialogue", ReplaceEscapeChar(unit[2]));
            }
            else if (unit[1] == "clear")
            {
                hashtable.Add("action", "clear");
            }
            else if (unit[1] == "newline")
            {
                hashtable.Add("action", "newline");
            }

            return hashtable;
        }

        private static Hashtable NameAsmToHash(string[] unit)
        {
            var hashtable = new Hashtable { { "object", "name" } };

            if (unit[1] == "append")
            {
                hashtable.Add("action", "append");
                hashtable.Add("name", ReplaceEscapeChar(unit[2]));
            }
            else if (unit[1] == "clear")
            {
                hashtable.Add("action", "clear");
            }

            return hashtable;
        }

        private static Hashtable GmAsmToHash(string[] unit)
        {
            var hash = new Hashtable
            {
                { "object", "gm" },
                { "action", unit[1] }
            };

            return hash;
        }

        private static Hashtable BgmAsmToHash(string[] unit)
        {
            var hash = new Hashtable
            {
                {"object","bgm"}
            };

            if (unit[1] == "play")
            {
                hash.Add("action", "play");
                hash.Add("audio_name", unit[2]);
            }
            else if (unit[1] == "stop")
            {
                hash.Add("action", "stop");
            }
            else if (unit[1] == "vol")
            {
                hash.Add("action", "vol");
                hash.Add("volume", unit[2]);
            }
            else if (unit[1] == "loop")
            {
                hash.Add("action", "loop");
                hash.Add("is_loop", unit[2]);
            }

            return hash;
        }

        private static Hashtable BgsAsmToHash(string[] unit)
        {
            var hash = new Hashtable
            {
                {"object","bgs"}
            };

            if (unit[1] == "play")
            {
                hash.Add("operate", "play");
                hash.Add("audio_name", unit[2]);
            }

            return hash;
        }

        private static Hashtable ChsAsmToHash(string[] unit)
        {
            var hash = new Hashtable
            {
                {"object","chs"}
            };

            if (unit[1] == "play")
            {
                hash.Add("action", "play");
                hash.Add("audio_name", unit[2]);
            }

            return hash;
        }

        #region Picture
        private static Hashtable BgpAsmToHash(string[] unit)
        {
            var hash = new Hashtable { { "object", "bgp" } };

            hash.Add("action", unit[1]);
            if (unit[1] == "set")
            {
                hash.Add("sprite_name", unit[2]);
                hash.Add("mode", unit[3]);
            }
            else if (unit[1] == "hide")
            {
                hash.Add("mode", unit[2]);
            }

            return hash;
        }

        private static Hashtable ChmAsmToHash(string[] unit)
        {
            var hash = new Hashtable { { "object", "ch_mid" } };
            hash.Add("action", unit[1]);
            if (unit[1] == "set")
            {
                hash.Add("sprite_name", unit[2]);
                hash.Add("mode", unit[3]);
            }
            else if (unit[1] == "hide")
            {
                hash.Add("mode", unit[2]);
            }

            return hash;
        }

        #endregion

        /// <summary>
        /// 替换字符串中的转义字符
        /// </summary>
        /// <param name="command"></param>
        private static string ReplaceEscapeChar(string command)
        {
            // 将\ns替换为相应数量的空格
            string newStr = Regex.Replace(command, @"\\(\d*)s", match =>
            {
                string numStr = match.Groups[1].Value;
                int spaceCount = string.IsNullOrEmpty(numStr) ? 1 : int.Parse(numStr);
                var spaces = new string(' ', spaceCount);
                return spaces;
            });

            return newStr;
        }
    }
}