using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System;

namespace VNScriptCompiler
{
    class VNScript
    {
        public static List<string> ParseVNScriptToIL(string[] vnScriptLines)
        {
            var ilList = new List<string>();

            for (int i = 0; i < vnScriptLines.Length; i++)
            {
                string line = vnScriptLines[i].Trim();

                if (line.Length == 0 && ilList[^1] != "[ clear_name ]")
                {
                    ilList.Add("[ clear_name ]");
                }
                else if (line[0] == '#') continue;
                else ilList.AddRange(ParseVNScriptToIL(line));
            }

            return ilList;
        }

        public static List<string> ParseVNScriptToIL(string line)
        {
            var ilList = new List<string>();

            // Note: 继续输出语句 必定是跟在 普通对话语句 后面的，因此继续输出语句中不会包含对对话者名称的检测

            // 当 line 为 IL Command 时
            // 使用 startIndex 与 endIndex 可以提取出放在同一行的多个 IL Command
            // Example: [ bgm_play: audio_name] [ bgm_vol: 0.4 ]
            if (line[0] == '[' && line[^1] == ']')
            {
                int startIndex = 0;
                int endIndex = 1;

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '[')
                    {
                        startIndex = i;
                    }
                    else if (line[i] == ']')
                    {
                        endIndex = i;
                        string unit = line[startIndex..(endIndex + 1)];
                        ilList.Add(unit);
                    }
                }
            }

            // 当 line 为继续输出语句时（不换行版）
            else if (line[0..3] == "-> ")
            {
                line = line[3..];
                var dialogueUnit = ExtractRoleSayContent(line);

                // 当存在 角色语音时，添加 role_say 命令
                if (dialogueUnit.Length == 2) ilList.Add($"[ role_say: {dialogueUnit[0]} ]");
                ilList.Add($"[ dialogue_append: {dialogueUnit[^1]} ]");
            }

            // 当 line 为继续输出语句时（换行版）
            else if (line[0..2] == "> ")
            {
                line = line[2..];
                var dialogueUnit = ExtractRoleSayContent(line);

                // 当存在 角色语音时，添加 role_say 命令
                if (dialogueUnit.Length == 2) ilList.Add($"[ role_say: {dialogueUnit[0]} ]");

                ilList.Add("[ dialogue_newline ]");
                ilList.Add($"[ dialogue_append: {dialogueUnit[^1]} ]");
            }

            // 其他情况统一按照 普通的对话格式 处理
            // Example: 江南: (role_audio)这是我第一次来到这个地方
            else
            {
                string[] parts = line.Split(new[] { ':' }, 2);
                string roleName = parts.Length == 2 ? parts[0].Trim() : "";
                string dialogueContent = parts[parts.Length - 1].Trim();

                // 当 roleName 参数不为空时，添加 role_name 命令
                if (roleName != "") ilList.Add($"[ name: {roleName} ]");

                var dialogueUnit = ExtractRoleSayContent(dialogueContent);

                // 当存在 角色语音时，添加 role_say 命令
                if (dialogueUnit.Length == 2) ilList.Add($"[ role_say: {dialogueUnit[0]} ]");

                ilList.Add($"[ dialogue: {dialogueUnit[^1]} ]");
            }

            return ilList;
        }

        /// <summary>
        /// 解析一个形如 （xxx）xxx 的字符串，将其括号内的内容和括号之后的内容分别提取出来
        /// (角色语音，对话内容)
        /// </summary>
        /// <param name="roleDialogueContent"></param>
        private static string[] ExtractRoleSayContent(string roleDialogueContent)
        {
            var extractedContent = new List<string>(); 
 
            // 使用正则表达式匹配以括号开头的行 
            var regex = new Regex(@"^\((.*?)\)(.*)$"); 
             
            Match match = regex.Match(roleDialogueContent); 
            if (match.Success) 
            { 
                // 获取括号内的内容 
                string content = match.Groups[1].Value.Trim(); 
                // 获取括号之后的内容 
                string remainingContent = match.Groups[2].Value.Trim(); 
                extractedContent.Add(content); 
                extractedContent.Add(remainingContent); 
            } 
            else 
            { 
                extractedContent.Add(roleDialogueContent); 
            } 
 
            return extractedContent.ToArray();
        }
    }

    public class ILScript
    {
        public struct ILunit
        {
            public string CommandName;
            public List<string> P;

            public void PrintUnit()
            {
                Console.WriteLine($"Command Name : |{CommandName}|");
                Console.WriteLine("P : " + string.Join(',', P));
            }
        }

        public static string[] ParseILToAsm(string[] ilLines)
        {
            var asmList = new List<string>();

            foreach (var line in ilLines)
            {
                asmList.AddRange(ParseILToAsm(line));
            }

            return asmList.ToArray();
        }

        public static List<string> ParseILToAsm(string line)
        {
            var unit = ParseILToUnit(line);
            return ParseILToAsm(unit);
        }

        public static List<string> ParseILToAsm(ILunit unit)
        {
            Hashtable commandToAsm = new()
            {
                { "dialogue", "dialogue clear\ndialogue append {0}\ngm stop" },
                { "role_dialogue", "name clear\ndialogue clear\nname append {0}\ndialogue append {1}\ngm stop" },
                { "dialogue_append", "dialogue append {0}\ngm stop" },
                { "dialogue_newline", "dialogue newline" },
                { "clear_dialog", "dialogue clear" },
                { "name", "name clear\nname append {0}" },
                { "clear_name", "name clear" },
                { "bgp", "bgp set {0} {1}" },
                { "bgp_hide", "bgp hide {0}" },
                { "role_pic", "ch_{0} set {1} {2}" },
                { "role_pic_hide", "ch_{0} hide {1}" },
                { "role_act", "ch_{0} {1}" },
                { "bgm_play", "bgm play {0}" },
                { "bgm_stop", "bgm stop" },
                { "bgm_loop", "bgm loop {0}" },
                { "bgm_continue", "bgm continue" },
                { "bgm_vol", "bgm vol {0}" },
                { "bgs_play", "bgs play {0}" },
                { "role_say", "chs play {0}" },
                { "role_vol", "chs vol {0}" },
                { "finish", "gm finish\nbgm stop\nbgs stop\nchs stop" }
            };

            string asmTemplate = Convert.ToString(commandToAsm[unit.CommandName]) ?? "";

            if (unit.P.Count == 0) return new List<string>(asmTemplate.Split('\n'));
            else return new List<string>(string.Format(asmTemplate, unit.P.ToArray()).Split('\n'));
        }


        /// <summary>
        /// 将中间代码转换为中间代码单元
        /// </summary>
        /// <param name="command"></param>
        public static ILunit ParseILToUnit(string command)
        {
            var ilUnit = new ILunit();

            // 去除前后的 [ ] 括号
            command = command.Trim()[1..(command.Length - 1)].Trim();

            // 将中间命令的参数解析出来
            var unit = command.Split(':', 2);

            // 拥有参数的IntermediateCommand会以「:」号将命令名称与参数隔开，因此如果unit长度为2，则说明当前命令是拥有参数的
            if (unit.Length == 2)
            {
                // unit第一个部分是命令名称，第二个部分是参数串。
                // 参数串部分使用「,」号进行分割
                ilUnit.CommandName = unit[0];
                ilUnit.P = SplitComma(unit[1]);
            }
            else
            {
                ilUnit.CommandName = unit[0];
                ilUnit.P = new();
            }

            return ilUnit;
        }

        /// <summary>
        /// 根据逗号分割参数，但是会忽略转义的逗号
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<string> SplitComma(string parameters)
        {
            string pattern = @"(?<!\\),";

            var parameter = Regex.Split(parameters, pattern);
            var result = new List<string>();

            // 移除转义字符，以及参数的前后空格
            for (int i = 0; i < parameter.Length; i++)
            {
                result.Add(parameter[i].Replace("\\,", ",").Trim());
            }

            return result;
        }
    }


    public class AsmScript
    {
        public static Hashtable ParseAsmToHash(string command)
        {
            string[] unit = command.Split(' ');

            var commandMapper = new Dictionary<string, Func<string[], Hashtable>>
            {
                { "dialogue", DialogueAsmToHash },
                { "name", NameAsmToHash },
                { "gm", GmAsmToHash },
                { "bgm", BgmAsmToHash },
                { "bgs", BgsAsmToHash },
                { "chs", ChsAsmToHash },
                { "bgp", BgpAsmToHash },
                { "ch_mid", ChmAsmToHash }
            };

            if (commandMapper.TryGetValue(unit[0], out var asmCommandFunc))
            {
                return asmCommandFunc(unit);
            }

            throw new Exception($"Unknown Asm Command: {command}");
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

        private static string ReplaceEscapeChar(string command)
        {
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