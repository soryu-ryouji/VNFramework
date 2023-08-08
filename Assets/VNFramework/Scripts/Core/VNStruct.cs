using System.Collections.Generic;

namespace VNFramework
{
    public enum AsmObj
    {
        dialogue,
        name,
        bgp,
        ch_left,
        ch_mid,
        ch_right,
        bgm,
        bgs,
        chs,
        gms,
        gm,
    }

    public class VNScriptIL
    {
        public string CommandName;
        public List<string> Parameters;

        public VNScriptIL()
        {
            CommandName = "";
            Parameters = new();
        }

        public VNScriptIL(string commandName)
        {
            CommandName = commandName;
            Parameters = new();
        }

        public VNScriptIL(string commandName, List<string> parameters)
        {
            CommandName = commandName;
            Parameters = parameters;
        }
    }

    public class VNScriptAsm
    {
        public AsmObj Obj;
        public string Action;
        public List<string> Parameters;

        public VNScriptAsm()
        {
            Action = "";
            Parameters = new();
        }

        public VNScriptAsm(AsmObj obj, string action, List<string> parameters)
        {
            Obj = obj;
            Action = action;
            Parameters = parameters;
        }

        public VNScriptAsm(AsmObj obj, string action)
        {
            Obj = obj;
            Action = action;
            Parameters = new();
        }

        public override string ToString()
        {
            return $"{Obj} {Action} {string.Join(" ", Parameters)}";
        }
    }

    public class PerformanceState
    {
        public string MermaidName;
        public int ScriptIndex;
        public string Bgm;
        public string Bgp;
        public string ChLeft;
        public string ChRight;
        public string ChMid;

        public PerformanceState()
        {
            MermaidName = "";
            ScriptIndex = 0;
            Bgm = "";
            Bgp = "";
            ChLeft = "";
            ChRight = "";
            ChMid = "";
        }

        public override string ToString()
        {
            return @$"Index: {ScriptIndex}
Mermaid: {MermaidName}
Bgm: {Bgm}
Bgp: {Bgp}
ChLeft: {ChLeft}
ChRight: {ChRight}
ChMid: {ChMid}";
        }
    }

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