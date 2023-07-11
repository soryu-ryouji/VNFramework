using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEditor;

namespace VNFramework.ScriptCompiler
{
    public class ILScript
    {
        public class IntermediateUnit
        {
            public string CommandName = "";
            public List<string> Parameters = new();
        }

        /// <summary>
        /// 获取中间代码组，然后将其转换为汇编代码组
        /// </summary>
        /// <param name="lines"></param>
        public static string[] ParseILToAsm(string[] lines)
        {
            var assemblyCommands = new List<string>();

            foreach (var line in lines)
            {
                // 将 line 转换为中间代码单元，根据中间代码单元生成汇编代码数组
                var intermediateUnit = ParseILToUnit(line);

                assemblyCommands.AddRange(ParseILToAsm(intermediateUnit));
            }

            return assemblyCommands.ToArray();
        }

        public static List<string> ParseILToAsm(string command)
        {
            var unit = ParseILToUnit(command);
            return ParseILToAsm(unit);
        }

        /// <summary>
        /// 获取一个中间代码单元，然后将其转换对应的汇编代码数组
        /// </summary>
        /// <param name="unit"></param>
        public static List<string> ParseILToAsm(IntermediateUnit unit)
        {
            List<string> ret = new();

            if (unit.CommandName == "dialogue")
            {
                ret.Add("dialogue clear");
                ret.Add($"dialogue append {unit.Parameters[0]}");
                ret.Add("gm stop");
            }
            else if (unit.CommandName == "role_dialogue")
            {
                ret.Add("name clear");
                ret.Add("dialogue clear");
                ret.Add($"name append {unit.Parameters[0]}");
                ret.Add($"dialogue append {unit.Parameters[1]}");
                ret.Add("gm stop");
            }
            else if (unit.CommandName == "dialogue_append")
            {
                ret.Add($"dialogue append {unit.Parameters[0]}");
                ret.Add("gm stop");
            }
            else if (unit.CommandName == "dialogue_newline")
            {
                ret.Add("dialogue newline");
            }
            else if (unit.CommandName == "clear_dialog")
            {
                ret.Add("dialogue clear");
            }
            else if (unit.CommandName == "name")
            {
                ret.Add("name clear");
                ret.Add($"name append {unit.Parameters[0]}");
            }
            else if (unit.CommandName == "clear_name")
            {
                ret.Add("name clear");
            }
            else if (unit.CommandName == "bgp")
            {
                ret.Add($"bgp set {unit.Parameters[0]} {unit.Parameters[1]}");
            }
            else if (unit.CommandName == "bgp_hide")
            {
                ret.Add($"bgp hide {unit.Parameters[0]}");
            }
            else if (unit.CommandName == "role_pic")
            {
                ret.Add($"ch_{unit.Parameters[0]} set {unit.Parameters[1]} {unit.Parameters[2]}");
            }
            else if (unit.CommandName == "role_pic_hide")
            {
                ret.Add($"ch_{unit.Parameters[0]} hide {unit.Parameters[1]} {unit.Parameters[2]}");
            }
            else if (unit.CommandName == "role_act")
            {
                ret.Add($"ch_{unit.Parameters[0]} {unit.Parameters[1]}");
            }
            else if (unit.CommandName == "bgm_play")
            {
                ret.Add($"bgm play {unit.Parameters[0]}");
            }
            else if (unit.CommandName == "bgm_stop")
            {
                ret.Add("bgm stop");
            }
            else if (unit.CommandName == "bgm_loop")
            {
                ret.Add($"bgm loop {unit.Parameters[0]}");
            }
            else if (unit.CommandName == "bgm_continue")
            {
                ret.Add("bgm continue");
            }
            else if (unit.CommandName == "bgm_vol")
            {
                ret.Add($"bgm vol {unit.Parameters[0]}");
            }
            else if (unit.CommandName == "bgs_play")
            {
                ret.Add($"se play {unit.Parameters[0]}");
            }
            else if (unit.CommandName == "bgs_play")
            {
                ret.Add($"bgs play {unit.Parameters[0]}");
            }
            else if (unit.CommandName == "role_say")
            {
                ret.Add($"role play {unit.Parameters[0]}");
            }
            else if (unit.CommandName == "role_vol")
            {
                ret.Add($"role vol {unit.Parameters[0]}");
            }
            else if (unit.CommandName == "finish")
            {
                ret.Add("gm finish");
            }

            return ret;
        }

        /// <summary>
        /// 将中间代码转换为中间代码单元
        /// </summary>
        /// <param name="command"></param>
        public static IntermediateUnit ParseILToUnit(string command)
        {
            var intermediateUnit = new IntermediateUnit();

            // 去除前后的 [ ] 括号
            command = command.Trim()[1..(command.Length - 1)].Trim();

            // 将中间命令的参数解析出来
            var unit = command.Split(':', 2);

            // 拥有参数的IntermediateCommand会以「:」号将命令名称与参数隔开，因此如果unit长度为2，则说明当前命令是拥有参数的
            if (unit.Length == 2)
            {
                // unit第一个部分是命令名称，第二个部分是参数串。
                // 参数串部分使用「,」号进行分割
                intermediateUnit.CommandName = unit[0];
                intermediateUnit.Parameters = SplitParameters(unit[1]);
            }
            else
            {
                intermediateUnit.CommandName = unit[0];
            }

            return intermediateUnit;
        }

        /// <summary>
        /// 根据逗号分割参数，但是会忽略转义的逗号
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<string> SplitParameters(string parameters)
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
}