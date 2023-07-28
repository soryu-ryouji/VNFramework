using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System;

namespace VNFramework.VNScriptCompiler
{
    class VNScript
    {
        private enum ScriptLineType
        {
            Annotation,
            Blank,
            Command,
            FullDialogue,
            NormDialogue,
        }

        private static ScriptLineType IdentifyLineType(string line)
        {
            if (line.Length == 0) return ScriptLineType.Blank;
            if (line[0] == '#') return ScriptLineType.Annotation;

            if (line[0] == '[' && line[^1] == ']') return ScriptLineType.Command;
            else if (line[0..2] == "| ") return ScriptLineType.FullDialogue;
            else return ScriptLineType.NormDialogue;
        }

        bool lastTypeIsClearName = false;
        bool isFullDialogueMode = false;
        bool isNotInitDialogueMode = true;

        public List<string> ParseVNScriptToIL(string line)
        {
            var ilList = new List<string>();
            line = line.Trim();
            var lineType = IdentifyLineType(line);

            // 多条空行只添加一条 [ clear_name ] 指令
            if (lineType == ScriptLineType.Blank && lastTypeIsClearName == false)
            {
                ilList.Add("[ clear_name ]");
                lastTypeIsClearName = true;
            }
            else if (lineType == ScriptLineType.Annotation)
            {

            }
            else if (lineType == ScriptLineType.Command)
            {
                ilList.AddRange(ParseVNScriptCommandLine(line));
                lastTypeIsClearName = false;
            }
            else if (lineType == ScriptLineType.FullDialogue)
            {
                if (isFullDialogueMode == false)
                {
                    ilList.Add("[ open_full_dialogue_box ]");
                }
                else if (isNotInitDialogueMode)
                {
                    ilList.Add("[ open_full_dialogue_box ]");
                    isNotInitDialogueMode = false;
                }
                ilList.AddRange(ParseVNScriptDialogueLine(line[2..]));
                isFullDialogueMode = true;
                lastTypeIsClearName = false;
            }
            else if (lineType == ScriptLineType.NormDialogue)
            {
                if (isFullDialogueMode == true)
                {
                    ilList.Add("[ open_norm_dialogue_box ]");
                }
                else if (isNotInitDialogueMode)
                {
                    ilList.Add("[ open_norm_dialogue_box ]");
                    isNotInitDialogueMode = false;
                }
                ilList.AddRange(ParseVNScriptDialogueLine(line));
                isFullDialogueMode = false;
                lastTypeIsClearName = false;
            }

            return ilList;
        }

        public static List<string> ParseAllVNScriptToIL(string[] vnScriptLines)
        {
            var ilList = new List<string>();

            bool lastTypeIsClearName = false;
            bool isFullDialogueMode = false;
            bool isNotInitDialogueMode = true;

            for (int i = 0; i < vnScriptLines.Length; i++)
            {
                string line = vnScriptLines[i].Trim();
                var lineType = IdentifyLineType(line);

                // 多条空行只添加一条 [ clear_name ] 指令
                if (lineType == ScriptLineType.Blank && lastTypeIsClearName == false)
                {
                    ilList.Add("[ clear_name ]");
                    lastTypeIsClearName = true;
                    continue;
                }

                if (lineType == ScriptLineType.Annotation) continue;

                if (lineType == ScriptLineType.Command)
                {
                    ilList.AddRange(ParseVNScriptCommandLine(line));
                    lastTypeIsClearName = false;
                    continue;
                }

                if (lineType == ScriptLineType.FullDialogue)
                {
                    if (isFullDialogueMode == false)
                    {
                        ilList.Add("[ open_full_dialogue_box ]");
                    }
                    else if (isNotInitDialogueMode)
                    {
                        ilList.Add("[ open_full_dialogue_box ]");
                        isNotInitDialogueMode = false;
                    }
                    ilList.AddRange(ParseVNScriptDialogueLine(line[2..]));
                    isFullDialogueMode = true;
                    lastTypeIsClearName = false;

                    continue;
                }

                if (lineType == ScriptLineType.NormDialogue)
                {
                    if (isFullDialogueMode == true)
                    {
                        ilList.Add("[ open_norm_dialogue_box ]");
                    }
                    else if (isNotInitDialogueMode)
                    {
                        ilList.Add("[ open_norm_dialogue_box ]");
                        isNotInitDialogueMode = false;
                    }
                    ilList.AddRange(ParseVNScriptDialogueLine(line));
                    isFullDialogueMode = false;
                    lastTypeIsClearName = false;

                    continue;
                }
            }

            return ilList;
        }

        private static List<string> ParseVNScriptCommandLine(string line)
        {
            // 当 line 为 IL Command 时
            // 使用 startIndex 与 endIndex 可以提取出放在同一行的多个 IL Command
            // Example: [ bgm_play: audio_name] [ bgm_vol: 0.4 ]
            var ilList = new List<string>();
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

            return ilList;
        }

        private static List<string> ParseVNScriptDialogueLine(string line)
        {
            var ilList = new List<string>();

            // Note: 继续输出语句 必定是跟在 普通对话语句 后面的，因此继续输出语句中不会包含对对话者名称的检测
            // 当 line 为继续输出语句时（换行版）
            if (line[0] == '>')
            {
                ilList.Add("[ dialogue_newline ]");
                line = line[1..].Trim();
                if (line != "")
                {
                    var dialogueUnit = ExtractRoleSayContent(line);

                    // 当存在 角色语音时，添加 role_say 命令
                    if (dialogueUnit.Length == 2) ilList.Add($"[ role_say: {dialogueUnit[0]} ]");
                    ilList.Add($"[ dialogue_append: {dialogueUnit[^1]} ]");
                }
            }

            // 当 line 为继续输出语句时（不换行版）
            else if (line[0..2] == "->")
            {
                line = line[2..].Trim();
                var dialogueUnit = ExtractRoleSayContent(line);

                // 当存在 角色语音时，添加 role_say 命令
                if (dialogueUnit.Length == 2) ilList.Add($"[ role_say: {dialogueUnit[0]} ]");
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
                { "open_full_dialogue_box", "dialogue clear\ndialogue switch full" },
                { "open_norm_dialogue_box", "dialogue clear\ndialogue switch norm" },
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
                { "bgm", AudioAsmToHash },
                { "bgs", AudioAsmToHash },
                { "chs", AudioAsmToHash },
                { "bgp", SpriteAsmToHash },
                { "ch_left", SpriteAsmToHash },
                { "ch_right", SpriteAsmToHash },
                { "ch_mid", SpriteAsmToHash }
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
            else if (unit[1] == "switch")
            {
                hashtable.Add("action", "switch");
                hashtable.Add("mode", unit[2]);
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

        private static Hashtable AudioAsmToHash(string[] unit)
        {
            var hash = new Hashtable { {"object",unit[0]} };
            var action = unit[1];
            if (action == "play")
            {
                hash.Add("action", AudioAction.Play);
                hash.Add("audio_name", unit[2]);
            }
            else if (action == "stop") hash.Add("action",AudioAction.Stop);
            else if (action == "vol")
            {
                hash.Add("action", AudioAction.Vol);
                hash.Add("volume", unit[2]);
            }
            else if (action == "loop")
            {
                hash.Add("action", AudioAction.Loop);
                hash.Add("is_loop", unit[2]);
            }

            return hash;
        }

        private static Hashtable SpriteAsmToHash(string[] unit)
        {
            var hash = new Hashtable { { "object", unit[0] } };

            var action = unit[1];
            if (action == "set")
            {
                hash.Add("action", SpriteAction.Show);
                hash.Add("sprite_name", unit[2]);

                if (unit[3] == "fading") hash.Add("mode", SpriteMode.Fading);
                else if (unit[3] == "immediate") hash.Add("mode", SpriteMode.Immediate);
            }
            else if (action == "hide")
            {
                hash.Add("action", SpriteAction.Hide);

                if (unit[2] == "fading") hash.Add("mode", SpriteMode.Fading);
                else if (unit[2] == "immediate") hash.Add("mode", SpriteMode.Immediate);
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