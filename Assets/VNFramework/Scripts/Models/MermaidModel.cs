using VNFramework.VNMermaid;

namespace VNFramework
{
    class MermaidModel : AbstractModel
    {
        private MermaidNode rootNode;
        protected override void OnInit()
        {
            Mermaid mermaid = new();
            string mermaidText = this.GetUtility<GameDataStorage>().LoadVNMermaid("VNMermaid");
            this.GetUtility<GameLog>().DebugLog("VNMermaid Text" + mermaidText);
            mermaid.ParseVNMermaid(mermaidText);
            rootNode = mermaid.mermaidNodes[0];
        }
    }
}