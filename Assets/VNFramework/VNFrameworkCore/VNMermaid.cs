using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

namespace VNFramework.Core
{
    public class Mermaid
    {
        private static readonly Regex defineRegex = new(@"^\s*(.*?)\s*\[\s*(.*?)\s*\]\s*$", RegexOptions.Singleline);
        private static readonly Regex linkRegex = new(@"^\s*(.*?)\s*-->\s*(\|\s*(.*?)\s*\|)?\s*(.*?)\s*$", RegexOptions.Singleline);

        public List<MermaidNode> mermaidNodes;
        private List<(string nodeName, string chapterName)> ghostNodes;

        public Mermaid()
        {
            mermaidNodes = new();
            ghostNodes = new();
        }

        public void AddGhostNode(string nodeName, string chapterName)
        {
            ghostNodes.Add((nodeName, chapterName));
        }

        public List<string> GetGhostNodeList()
        {
            var list = new List<string>();

            foreach (var node in ghostNodes)
            {
                list.Add(node.nodeName);
            }

            return list;
        }

        public List<string> GetMermaidNodeList()
        {
            var list = new List<string>();

            foreach (var node in mermaidNodes)
            {
                list.Add(node.NodeName);
            }

            return list;
        }

        public void LinkMermaidNode(string from, string to, string optionText)
        {
            MermaidNode fromNode = MermaidNode.GetMermaidNode(mermaidNodes, from);
            if (fromNode == null)
            {
                // 若 mermaidNotes 中不存在 fromNode 节点，则尝试从 ghost中创建
                // 使用 ghostNotes 中的数据创建出 fromNode 节点后，将 fromNode 添加进 mermaidNodes 中
                // fromNode 添加进 mermaidNotes 后，从 ghostNodes 中移除 fromNode 的数据
                var index = GetGhostNodeIndex(from);
                if (index == -1) throw new ArgumentException($"fromNode 「{from}」 not found");

                var ghostNode = ghostNodes[index];
                fromNode = new MermaidNode(ghostNode.nodeName, ghostNode.chapterName);

                mermaidNodes.Add(fromNode);
                ghostNodes.RemoveAt(index);
            }

            MermaidNode toNode;
            int toNodeIndex = GetMermaidNodeIndex(to);

            if (toNodeIndex == -1)
            {
                toNode = MermaidNode.GetMermaidNode(mermaidNodes, to);
                if (toNode == null)
                {
                    var index = GetGhostNodeIndex(to);
                    if (index == -1) throw new ArgumentException($"toNode 「{from}」 not found");

                    var ghostNode = ghostNodes[index];
                    toNode = new MermaidNode(ghostNode.nodeName, ghostNode.chapterName);

                    ghostNodes.RemoveAt(index);
                }
                fromNode.AddLinkedNode(toNode, optionText);
            }
            else
            {
                toNode = mermaidNodes[toNodeIndex];
                fromNode.AddLinkedNode(toNode, optionText);
                mermaidNodes.RemoveAt(toNodeIndex);
            }
        }

        public string[] GetMermaidMap()
        {
            var paths = new List<string>();

            foreach (var node in mermaidNodes)
            {
                paths.AddRange(node.GetNodePaths());
            }

            return paths.ToArray();
        }

        public int GetGhostNodeIndex(string nodeName)
        {
            for (int i = 0; i < ghostNodes.Count; i++)
            {
                if (ghostNodes[i].nodeName == nodeName) return i;
            }

            return -1;
        }

        public int GetMermaidNodeIndex(string nodeName)
        {
            for (int i = 0; i < mermaidNodes.Count; i++)
            {
                if (mermaidNodes[i].NodeName == nodeName) return i;
            }

            return -1;
        }

        /// <summary>
        /// 解析符合 VNMermaid 语法规范的字符串
        /// </summary>
        /// <param name="mermaidLines"></param>
        public void ParseVNMermaid(string[] mermaidText)
        {
            // 分别提取出所有的定义和链接字符串
            // 解析所有定义后，再进行字符串链接

            var (definedLines, linkLines) = ExtractMermaidText(mermaidText);

            // Define 语法
            foreach (var line in definedLines)
            {
                var (nodeName, chapterName) = ExtractDefineNode(line);
                AddGhostNode(nodeName, chapterName);
            }

            // Link 语法
            foreach (var line in linkLines)
            {
                var unit = ExtractLinkNode(line);
                LinkMermaidNode(from: unit.fromNode, to: unit.toNode, optionText: unit.optionText);
            }
        }

        public static (List<string> defineLines, List<string> linkLines) ExtractMermaidText(string[] mermaidLines)
        {
            var defineLines = new List<string>();
            var linkLines = new List<string>();

            for (int i = 0; i < mermaidLines.Length; i++)
            {
                string line = mermaidLines[i];
                var trimmedLine = line.Trim();

                if (string.IsNullOrEmpty(trimmedLine)) continue;

                if (defineRegex.IsMatch(trimmedLine))
                {
                    defineLines.Add(trimmedLine);
                }
                else if (linkRegex.IsMatch(trimmedLine))
                {
                    linkLines.Add(trimmedLine);
                }
                else if (!string.IsNullOrEmpty(line) && line[0] != '#')
                {
                    throw new ArgumentException($"VN Mermaid Defeat : line : {i} extra text ->「{trimmedLine}」");
                }
            }

            return (defineLines, linkLines);
        }

        public static (string nodeName, string chapterName) ExtractDefineNode(string defineLine)
        {
            Match match = defineRegex.Match(defineLine);

            if (match.Success)
            {
                string nodeName = match.Groups[1].Value;
                string chapterName = match.Groups[2].Value;

                if (string.IsNullOrWhiteSpace(nodeName) || string.IsNullOrWhiteSpace(chapterName))
                {
                    throw new ArgumentException($"VN Mermaid Defeat : extra define text ->「{defineLine}」");
                }

                return (nodeName, chapterName);
            }
            else
            {
                throw new ArgumentException($"VN Mermaid Defeat : extra define text ->「{defineLine}」");
            }
        }

        public static (string fromNode, string optionText, string toNode) ExtractLinkNode(string linkLine)
        {
            Match match = linkRegex.Match(linkLine);

            if (match.Success)
            {
                // match.Groups 是匹配所使用的文本串
                // Groups[1] 是 fromNode
                // Groups[2] 是 |OptionText|
                // Groups[3] 是 OptionText
                // Groups[4] 是 toNode
                string fromNode, toNode, optionText;
                fromNode = match.Groups[1].Value;
                toNode = match.Groups[^1].Value;
                optionText = match.Groups[3].Value;

                if (string.IsNullOrEmpty(fromNode) || string.IsNullOrEmpty(toNode))
                {
                    throw new ArgumentException($"VN Mermaid Defeat : extra link text ->「{linkLine}」");
                }

                return (fromNode, optionText, toNode);
            }
            else
            {
                throw new ArgumentException($"VN Mermaid Defeat : extra link text ->「{linkLine}」");
            }
        }
    }
}