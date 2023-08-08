using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.Linq;

namespace VNFramework.Core
{
    public enum MermaidTag
    {
        Define,
        Link
    }

    public class MermaidNode
    {
        public string NodeName;
        public string ChapterName;

        public List<(MermaidNode node, string optionText)> Children;

        public MermaidNode(string nodeName, string chapterName)
        {
            NodeName = nodeName;
            ChapterName = chapterName;
            Children = new();
        }

        public List<(string childName, string optionText)> GetChildren()
        {
            var list = new List<(string childName, string optionText)>();

            foreach (var child in Children)
            {
                list.Add((child.node.NodeName, child.optionText));
            }

            return list;
        }

        public void AddLinkedNode(MermaidNode node, string optionText)
        {
            Children.Add((node: node, optionText: optionText));
        }

        public MermaidNode GetMermaidNode(string nodeName)
        {
            if (NodeName == nodeName) return this;

            foreach (var child in Children)
            {
                var result = child.node.GetMermaidNode(nodeName);

                if (result != null) return result;
            }

            return null;
        }

        public static MermaidNode GetMermaidNode(MermaidNode parentNode, string nodeName)
        {
            if (parentNode.NodeName == nodeName) return parentNode;

            var result = parentNode.GetMermaidNode(nodeName);
            if (result != null) return result;

            return null;
        }

        public static MermaidNode GetMermaidNode(List<MermaidNode> parentNode, string nodeName)
        {
            foreach (var node in parentNode)
            {
                var result = node.GetMermaidNode(nodeName);
                if (result != null) return result;
            }

            return null;
        }

        public string[] GetNodePaths()
        {
            List<string> paths = new List<string>();
            GenerateNodePath(this, "", paths);
            return paths.ToArray();
        }

        private void GenerateNodePath(MermaidNode node, string path, List<string> paths)
        {
            // 使用深度优先算法输出路径
            path = path + node.NodeName;

            if (node.Children.Count == 0)
            {
                // 已经到达叶子节点，保存路径
                paths.Add(path.TrimEnd(' ', '-'));
            }
            else
            {
                path += " -> ";
                // 继续遍历子节点
                foreach (var child in node.Children)
                {
                    GenerateNodePath(child.node, path, paths);
                }
            }
        }
    }

    public class Mermaid
    {
        // 先为 VNMermaid 添加所有被定义的 Mermaid Node
        // 然后使用 LinkGhostNode 为节点添加连接
        // 最后导出 Mermaid Map

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
                // 使用 ghostNotes 中的数据创建出 fromNode 节点后，将 fromNode 添加进 mermaidNotes中
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
        public void ParseVNMermaid(string mermaidText)
        {
            var definedLines = ExtractMermaidTagText(mermaidText, MermaidTag.Define);
            var linkLines = ExtractMermaidTagText(mermaidText, MermaidTag.Link);

            // Define 语法
            // Mermaid节点名称[章节名称]
            foreach (var line in definedLines)
            {
                var unit = ExtractDefineText(line);
                AddGhostNode(unit.nodeName, unit.chapterName);
            }

            // Link 语法
            // from 节点 --> to 节点 (选项文本)
            // 若不存在选项文本，则演出时不弹出任何选项
            foreach (var line in linkLines)
            {
                var unit = ExtractLinkText(line);
                LinkMermaidNode(unit.fromNode, unit.toNode, unit.optionText);
            }
        }

        public static List<string> ExtractMermaidTagText(string text, MermaidTag mode)
        {
            string pattern = "";
            if (mode == MermaidTag.Define)  pattern = @"\[Define\](.*?)(?=\[Define\]|\[Link\]|\z)";
            else if (mode == MermaidTag.Link)  pattern = @"\[Link\](.*?)(?=\[Define\]|\[Link\]|\z)";

            MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.Singleline);
            var result = new List<string>();
            
            string newlinePattern = @"\r\n|\n|\r";
            for (int i = 0; i < matches.Count; i++)
            {
                var lines = Regex.Split(matches[i].Groups[1].Value, newlinePattern);
                var newLines = lines.Where(line => !string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith("#"));
                result.AddRange(newLines);
            }

            return result;
        }

        public static (string nodeName, string chapterName) ExtractDefineText(string text)
        {
            string pattern = @"^(.*?)\[(.*?)\]$";
            Match match = Regex.Match(text, pattern);

            if (match.Success)
            {
                string nodeName = match.Groups[1].Value.Trim();
                string chapterName = match.Groups[2].Value.Trim();

                if (string.IsNullOrWhiteSpace(nodeName)) throw new ArgumentException("nodeName 缺失");
                if (string.IsNullOrWhiteSpace(chapterName)) throw new ArgumentException("chapterName 缺失");

                return (nodeName, chapterName);
            }
            else
            {
                throw new ArgumentException($"Defeat : extra define text ->{text}");
            }
        }

        public static (string fromNode, string toNode, string optionText) ExtractLinkText(string text)
        {
            string pattern = @"^\s*(.*?)\s*-->\s*(.*?)\s*(?:\((.*?)\))?\s*$";
            Match match = Regex.Match(text, pattern);

            if (match.Success)
            {
                string fromNode = match.Groups[1].Value.Trim();
                string toNode = match.Groups[2].Value.Trim();
                string optionText = match.Groups[3].Success ? match.Groups[3].Value.Trim() : ""; // 设置optionText为空字符串

                // 验证是否缺失任何一个部分，并抛出相应的异常
                if (string.IsNullOrWhiteSpace(fromNode)) throw new ArgumentException("from节点缺失");
                if (string.IsNullOrWhiteSpace(toNode)) throw new ArgumentException("to节点缺失");

                return (fromNode, toNode, optionText);
            }
            else
            {
                throw new ArgumentException($"Defeat : extra link text ->「{text}」");
            }
        }
    }
}