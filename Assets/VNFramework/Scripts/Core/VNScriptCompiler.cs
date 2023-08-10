using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System;

namespace VNFramework.Core
{
    public class VNScriptCompiler
    {
        #region VNScript

        private string[] _vnScriptLines;
        private int _vnScriptLen;
        private int _scriptIndex = 0;

        public int ScriptIndex { get => _scriptIndex; }

        public VNScriptCompiler(string[] vnScriptLines)
        {
            this._vnScriptLines = vnScriptLines;
            _vnScriptLen = _vnScriptLines.Length;
        }

        public int ScriptCountDown()
        {
            return _vnScriptLen - _scriptIndex;
        }

        public List<VNScriptAsm> NextAsmList()
        {
            var ilList = ParseVNScriptLineToIL(_vnScriptLines[_scriptIndex]);
            var asmList = ParseILLinesToAsm(ilList);

            if (_scriptIndex < _vnScriptLen) _scriptIndex++;

            return asmList;
        }

        private enum ScriptLineType
        {
            Annotation,
            Blank,
            Command,
            FullDialogue,
            NormDialogue,
        }

        public void InitByLine(int index)
        {
            _scriptIndex = 0;
            while (_scriptIndex < index)
            {
                NextAsmList();
            }
        }

        public static PerformanceState GetPerformanceStateByIndex(string[] vnScript, int index, out VNScriptCompiler vnScriptCompiler)
        {
            var performanceState = new PerformanceState();
            var compiler = new VNScriptCompiler(vnScript);

            int lastBlock = 0;

            for (int i = 0; compiler.ScriptCountDown() > 0 && i < index; i++)
            {
                var asmList = compiler.NextAsmList();
                foreach (var asm in asmList)
                {
                    // 图片与音乐的状态获取
                    if (asm.Obj == AsmObj.bgm && asm.Action == "play") performanceState.Bgm = asm.Parameters[0];
                    else if (asm.Obj == AsmObj.bgm && asm.Action == "Stop") performanceState.Bgm = "";

                    if (asm.Obj == AsmObj.bgp && asm.Action == "show") performanceState.Bgp = asm.Parameters[0];
                    else if (asm.Obj == AsmObj.bgp && asm.Action == "hide") performanceState.Bgp = "";

                    if (asm.Obj == AsmObj.ch_left && asm.Action == "show") performanceState.ChLeft = asm.Parameters[0];
                    else if (asm.Obj == AsmObj.ch_left && asm.Action == "hide") performanceState.ChLeft = "";

                    if (asm.Obj == AsmObj.ch_right && asm.Action == "show") performanceState.ChRight = asm.Parameters[0];
                    else if (asm.Obj == AsmObj.ch_right && asm.Action == "hide") performanceState.ChRight = "";

                    if (asm.Obj == AsmObj.ch_mid && asm.Action == "show") performanceState.ChMid = asm.Parameters[0];
                    else if (asm.Obj == AsmObj.ch_mid && asm.Action == "hide") performanceState.ChMid = "";

                    // 文本框状态获取
                    // 判断当前最后的对话 Block，实现全屏文字演出的完整性
                    if (asm.Obj == AsmObj.dialogue && asm.Action == "switch" && asm.Parameters[0] == "full")
                    {
                        lastBlock = i;
                        performanceState.isFullDialogueBox = true;
                    }
                    else if (asm.Obj == AsmObj.dialogue && asm.Action == "switch" && asm.Parameters[0] == "norm")
                    {
                        performanceState.isFullDialogueBox = false;
                    }
                    else if (asm.Obj == AsmObj.dialogue && asm.Action == "clear")
                    {
                        lastBlock = i;
                    }
                }

                if (performanceState.isFullDialogueBox == false) lastBlock = i;
            }

            performanceState.ScriptIndex = lastBlock;
            vnScriptCompiler = compiler;
            return performanceState;
        }


        private static ScriptLineType IdentifyLineType(string line)
        {
            if (line.Length == 0) return ScriptLineType.Blank;
            if (line[0] == '#') return ScriptLineType.Annotation;

            if (line[0] == '[' && line[^1] == ']') return ScriptLineType.Command;
            else if (line[0..2] == "| ") return ScriptLineType.FullDialogue;
            else return ScriptLineType.NormDialogue;
        }

        bool lastTypeIsClearDialogue = false;
        bool isFullDialogueMode = false;
        bool isNotInitDialogueMode = true;
        public List<VNScriptIL> ParseVNScriptLineToIL(string line)
        {
            var ilList = new List<VNScriptIL>();
            line = line.Trim();
            var lineType = IdentifyLineType(line);

            if (lineType == ScriptLineType.Annotation) return ilList;

            // 多条空行只添加一条 [ clear_dialogue ] 指令
            if (lineType == ScriptLineType.Blank && lastTypeIsClearDialogue == false)
            {
                ilList.Add(new VNScriptIL("clear_dialogue"));
                lastTypeIsClearDialogue = true;
            }
            else if (lineType == ScriptLineType.Command)
            {
                ilList.AddRange(ParseVNScriptCommandSyntax(line));
                lastTypeIsClearDialogue = false;
            }
            else if (lineType == ScriptLineType.FullDialogue)
            {
                if (isNotInitDialogueMode)
                {
                    ilList.Add(new VNScriptIL("open_full_dialogue_box"));
                    isNotInitDialogueMode = false;
                }
                else if (isFullDialogueMode == false)
                {
                    ilList.Add(new VNScriptIL("open_full_dialogue_box"));
                }

                ilList.AddRange(ParseVNScriptDialogueSyntax(line[2..]));
                isFullDialogueMode = true;
                lastTypeIsClearDialogue = false;
            }
            else if (lineType == ScriptLineType.NormDialogue)
            {
                if (isFullDialogueMode == true)
                {
                    ilList.Add(new VNScriptIL("open_norm_dialogue_box"));
                }
                else if (isNotInitDialogueMode)
                {
                    ilList.Add(new VNScriptIL("open_norm_dialogue_box"));
                    isNotInitDialogueMode = false;
                }
                ilList.AddRange(ParseVNScriptDialogueSyntax(line));
                isFullDialogueMode = false;
                lastTypeIsClearDialogue = false;
            }

            return ilList;
        }

        private static List<VNScriptIL> ParseVNScriptCommandSyntax(string line)
        {
            // 当 line 为 IL Command 时
            // 使用 startIndex 与 endIndex 可以提取出放在同一行的多个 IL Command
            // Example: [ bgm_play: audio_name] [ bgm_vol: 0.4 ]
            var ilList = new List<VNScriptIL>();
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
                        ilList.Add(ParseILToStruct(unit));
                    }
                }
            }

            return ilList;
        }

        private static List<VNScriptIL> ParseVNScriptDialogueSyntax(string line)
        {
            var ilList = new List<VNScriptIL>();

            // Note: 继续输出语句 必定是跟在 普通对话语句 后面的，因此继续输出语句中不会包含对对话者名称的检测
            // 当 line 为继续输出语句时（换行版）
            if (line[0] == '>')
            {
                line = line[1..].Trim();
                ilList.Add(new VNScriptIL("dialogue_newline"));
                if (line != "")
                {
                    var dialogueUnit = ExtractRoleSayUnit(line);

                    // 当存在 角色语音时，添加 role_say 命令
                    if (dialogueUnit.Length == 2)
                    {
                        ilList.Add(new VNScriptIL("role_say", parameters: new() { dialogueUnit[0] }));
                    }

                    ilList.Add(new VNScriptIL("dialogue_append", parameters: new() { dialogueUnit[^1] }));
                }
            }

            // 当 line 为继续输出语句时（不换行版）
            else if (line[0..2] == "->")
            {
                line = line[2..].Trim();
                var dialogueUnit = ExtractRoleSayUnit(line);

                // 当存在 角色语音时，添加 role_say 命令
                if (dialogueUnit.Length == 2) ilList.Add(new VNScriptIL("role_say", new() { dialogueUnit[0] }));

                ilList.Add(new VNScriptIL("dialogue_append", new() { dialogueUnit[^1] }));
            }

            // 其他情况统一按照 普通的对话格式 处理
            // Example: 江南: (role_audio)这是我第一次来到这个地方
            else
            {
                string[] parts = line.Split(new[] { ':' }, 2);
                string roleName = parts.Length == 2 ? parts[0].Trim() : "";
                string dialogueContent = parts[^1].Trim();

                // 当 roleName 参数不为空时，添加 role_name 命令
                if (roleName != "") ilList.Add(new VNScriptIL("name", new() { roleName }));

                var dialogueUnit = ExtractRoleSayUnit(dialogueContent);

                // 当存在 角色语音时，添加 role_say 命令
                if (dialogueUnit.Length == 2) ilList.Add(new VNScriptIL("role_say", new() { dialogueUnit[0] }));
                ilList.Add(new VNScriptIL("dialogue", new() { dialogueUnit[^1] }));
            }

            return ilList;
        }
        #endregion

        #region IL

        public static List<VNScriptAsm> ParseILLinesToAsm(List<VNScriptIL> ilList)
        {
            var asmList = new List<VNScriptAsm>();

            foreach (var line in ilList)
            {
                asmList.AddRange(ParseILToAsm(line));
            }

            return asmList;
        }

        public static List<VNScriptAsm> ParseILToAsm(VNScriptIL ilUnit)
        {
            var asmList = new List<VNScriptAsm>();

            switch (ilUnit.CommandName)
            {
                case "open_full_dialogue_box":
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "clear"));
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "switch", new() { "full" }));
                    break;
                case "open_norm_dialogue_box":
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "clear"));
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "switch", new() { "norm" }));
                    break;
                case "dialogue":
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "clear"));
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "append", ilUnit.Parameters));
                    asmList.Add(new VNScriptAsm(AsmObj.gm, "stop"));
                    break;
                case "dialogue_append":
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "append", ilUnit.Parameters));
                    asmList.Add(new VNScriptAsm(AsmObj.gm, "stop"));
                    break;
                case "role_dialogue":
                    asmList.Add(new VNScriptAsm(AsmObj.name, "clear"));
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "clear"));
                    asmList.Add(new VNScriptAsm(AsmObj.name, "append", ilUnit.Parameters));
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "append", ilUnit.Parameters));
                    asmList.Add(new VNScriptAsm(AsmObj.gm, "stop"));
                    break;
                case "dialogue_newline":
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "newline"));
                    break;
                case "clear_dialogue":
                    asmList.Add(new VNScriptAsm(AsmObj.dialogue, "clear"));
                    asmList.Add(new VNScriptAsm(AsmObj.name, "clear"));
                    break;
                case "name":
                    asmList.Add(new VNScriptAsm(AsmObj.name, "clear"));
                    asmList.Add(new VNScriptAsm(AsmObj.name, "append", ilUnit.Parameters));
                    break;
                case "bgp":
                    asmList.Add(new VNScriptAsm(AsmObj.bgp, "show", ilUnit.Parameters));
                    break;
                case "bgp_hide":
                    asmList.Add(new VNScriptAsm(AsmObj.bgp, "hide", ilUnit.Parameters));
                    break;
                case "role_pic":
                    asmList.Add(new VNScriptAsm(ilUnit.Parameters[0] switch
                    {
                        "mid" => AsmObj.ch_mid,
                        "left" => AsmObj.ch_left,
                        "right" => AsmObj.ch_right,
                        _ => AsmObj.ch_mid
                    },
                        "show",
                        new () {ilUnit.Parameters[1], ilUnit.Parameters[2]}));
                    break;
                case "role_pic_hide":
                    asmList.Add(new VNScriptAsm(ilUnit.Parameters[0] switch
                    {
                        "mid" => AsmObj.ch_mid,
                        "left" => AsmObj.ch_left,
                        "right" => AsmObj.ch_right,
                        _ => AsmObj.ch_mid
                    },
                        "hide",
                        new () {ilUnit.Parameters[1] }));
                    break;
                case "role_act":
                    asmList.Add(new VNScriptAsm(ilUnit.Parameters[0] switch
                    {
                        "mid" => AsmObj.ch_mid,
                        "left" => AsmObj.ch_left,
                        "right" => AsmObj.ch_right,
                        _ => AsmObj.ch_mid
                    },
                        ilUnit.Parameters[1]
                    ));
                    break;
                case "bgm_play":
                    asmList.Add(new VNScriptAsm(AsmObj.bgm, "play", ilUnit.Parameters));
                    break;
                case "bgm_stop":
                    asmList.Add(new VNScriptAsm(AsmObj.bgm, "stop"));
                    break;
                case "bgm_loop":
                    asmList.Add(new VNScriptAsm(AsmObj.bgm, "loop", ilUnit.Parameters));
                    break;
                case "bgm_continue":
                    asmList.Add(new VNScriptAsm(AsmObj.bgm, "continue"));
                    break;
                case "bgm_vol":
                    asmList.Add(new VNScriptAsm(AsmObj.bgm, "vol", ilUnit.Parameters));
                    break;
                case "bgs_play":
                    asmList.Add(new VNScriptAsm(AsmObj.bgs, "play", ilUnit.Parameters));
                    break;
                case "role_say":
                    asmList.Add(new VNScriptAsm(AsmObj.chs, "play", ilUnit.Parameters));
                    break;
                case "role_vol":
                    asmList.Add(new VNScriptAsm(AsmObj.chs, "vol", ilUnit.Parameters));
                    break;
            }

            return asmList;
        }

        /// <summary>
        /// 将中间代码转换为中间代码单元
        /// </summary>
        /// <param name="command"></param>
        public static VNScriptIL ParseILToStruct(string command)
        {
            var ilUnit = new VNScriptIL();

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
                ilUnit.Parameters = SplitComma(unit[1]);
            }
            else
            {
                ilUnit.CommandName = unit[0];
                ilUnit.Parameters = new();
            }

            return ilUnit;
        }
        #endregion

        #region Tools

        /// <summary>
        /// 解析一个形如 （xxx）xxx 的字符串，将其括号内的内容和括号之后的内容分别提取出来
        /// (角色语音，对话内容)
        /// </summary>
        /// <param name="roleDialogueContent"></param>
        private static string[] ExtractRoleSayUnit(string roleDialogueContent)
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
        #endregion
    }
}