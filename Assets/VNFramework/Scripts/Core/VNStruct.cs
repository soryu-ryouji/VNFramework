namespace VNFramework
{
    public enum AudioPlayer
    {
        Bgm,
        Bgs,
        Chs,
        Gms,
        Null
    }

    public enum AudioAction
    {
        Play,
        Stop,
        Loop,
        Vol
    }

    public enum SpriteMode
    {
        Fading,
        Immediate
    }

    public enum SpriteObj
    {
        ChLeft,
        ChRight,
        ChMid,
        Bgp,
        Null,
    }

    public enum SpriteAction
    {
        Show,
        Hide,
        Shake,
        MoveLR,
    }

    public class SaveFile
    {
        public string SaveDate;
        public string MermaidNode;
        public int VNScriptIndex;
        public string ResumePic;
        public string ResumeText;

        public SaveFile(string saveDate, string mermaidNode, int vNScriptIndex, string resumePic, string resumeText)
        {
            SaveDate = saveDate;
            MermaidNode = mermaidNode;
            VNScriptIndex = vNScriptIndex;
            ResumePic = resumePic;
            ResumeText = resumeText;
        }
        public SaveFile()
        {
            SaveDate = null;
            MermaidNode = null;
            VNScriptIndex = 0;
            ResumePic = null;
            ResumeText = null;
        }
    }
}