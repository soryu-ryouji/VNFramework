using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace VNFramework.ScriptCompiler
{
    class VNScript
    {
        public static string[] ParseVNScriptToAsm(string vnScript)
        {
            var il = ParseVNScriptToIL(vnScript);
            var asm = ILScript.ParseILToAsm(il.ToArray());
            return asm;
        }

        /// <summary>
        /// 根据VNScript格式的脚本，生成中间代码
        /// </summary>
        /// <param name="lines"></param>
        public static List<string> ParseVNScriptToIL(string[] lines)
        {
            var result = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                // 当行内容为空时 [ clear_name ] 命令
                if (line.Length == 0)
                {
                    //如果上一条命令已经是 [ clear_name ]，则不再添加
                    if (result[^1] != "[ clear_name ]")
                    {
                        result.Add("[ clear_name ]");
                    }
                }
                // 当行内容为注释时，跳过
                else if (line[0] == '#')
                {
                    continue;
                }
                // 当行内容不属于以上两种情况时，将其解析为中间代码
                else
                {
                    result.AddRange(ParseVNScriptToIL(line));
                }
            }


            return result;
        }

        /// <summary>
        /// 根据VNScript格式的脚本，生成中间代码
        /// </summary>
        /// <param name="lines"></param>
        public static List<string> ParseVNScriptToIL(string line)
        {
            var result = new List<string>();
            // 当 line 为 IntermediateCommand 格式时
            if (line[0] == '[' && line[^1] == ']')
            {
                int startIndex = 0;
                int endIndex;

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
                        result.Add(unit);
                    }
                }
            }
            // 当 line 为继续输出语句时（不换行版）
            else if (line[0..3] == "-> ")
            {
                line = line[3..];
                var content = ExtractContentInParentheses(line.Trim());
                // 如果对话语句内存在括号，则添加一条播放角色语音的命令
                if (content.Length == 2)
                {
                    result.Add($"[ role_say: {content[0]} ]");
                    result.Add($"[ dialogue_append: {content[1]} ]");
                }
                else
                {
                    result.Add($"[ dialogue_append: {content[0]} ]");
                }
            }
            // 当 line 为继续输出语句时（换行版）
            else if (line[0..2] == "> ")
            {
                line = line[2..];
                var content = ExtractContentInParentheses(line.Trim());
                // 如果对话语句内存在括号，则添加一条播放角色语音的命令
                if (content.Length == 2)
                {
                    result.Add($"[ role_say: {content[0]} ]");
                    result.Add("[ dialogue_newline ]");
                    result.Add($"[ dialogue_append: {content[1]} ]");
                }
                else
                {
                    result.Add("[ dialogue_newline ]");
                    result.Add($"[ dialogue_append: {content[0]} ]");
                }
            }
            // 其他的情况统一按照 对话格式 处理
            else
            {
                // 当存在角色名称时，[0]对话人名称，[1]对话内容
                // 否则，[0]对话内容
                string[] parts = line.Split(new[] { ':' }, 2);
                string roleName = "";
                string roleSay;

                if (parts.Length == 2)
                {
                    roleName = parts[0].Trim();
                    roleSay = parts[1].Trim();
                }
                else
                {
                    roleSay = parts[0].Trim();
                }

                if (!String.IsNullOrEmpty(roleName))
                {
                    result.Add($"[ name: {parts[0]} ]");
                }

                var content = ExtractContentInParentheses(roleSay.Trim());
                // 如果对话语句内存在播放语音指令，则添加一条播放角色语音的命令
                if (content.Length == 2)
                {
                    result.Add($"[ role_say: {content[0]} ]");
                    roleSay = content[1];
                }

                result.Add($"[ dialogue: {roleSay} ]");
            }

            return result;
        }

        /// <summary>
        /// 解析一个形如 （xxx）xxx 的字符串，将其括号内的内容和括号之后的内容分别提取出来
        /// </summary>
        /// <param name="input"></param>
        private static string[] ExtractContentInParentheses(string input)
        {
            var extractedContent = new List<string>();

            // 使用正则表达式匹配以括号开头的行
            var regex = new Regex(@"^\((.*?)\)(.*)$");
            
            Match match = regex.Match(input);
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
                extractedContent.Add(input);
            }

            return extractedContent.ToArray();
        }
    }
}