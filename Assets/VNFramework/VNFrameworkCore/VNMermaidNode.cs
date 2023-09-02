using System.Collections.Generic;

namespace VNFramework.Core
{
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
}