using System.Collections.Generic;
using VNFramework.Core;

namespace VNFramework
{
    class MermaidModel : AbstractModel
    {
        private MermaidNode rootNode;
        protected override void OnInit()
        {
            Mermaid mermaid = new();
            string mermaidText = this.GetUtility<GameDataStorage>().LoadVNMermaid("VNMermaid");
            mermaid.ParseVNMermaid(mermaidText);
            rootNode = mermaid.mermaidNodes[0];
        }

        public string GetFirstMermaidName()
        {
            return rootNode.NodeName;
        }

        public List<(string childMermaidName, string optionText)> GetMermaidChildren(string mermaidName)
        {
            var curNode = rootNode.GetMermaidNode(mermaidName);
            var result = curNode.GetChildren();
            return result;
        }

        public string GetFileName(string mermaidName)
        {
            var curNode = rootNode.GetMermaidNode(mermaidName);
            return curNode.ChapterName;
        }
    }
}