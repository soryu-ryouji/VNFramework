using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using VNFramework.ScriptCompiler;

namespace VNFramework
{
    public class PerformanceController : MonoBehaviour
    {
        public List<string> vnScript;

        public string vnScriptName;

        public int vnScriptIndex;
        private int _vnScriptCount;

        public bool autoExecuteCommand;

        private UnityAction<Hashtable> executeCommand;

        public string VNScriptFileName
        {
            get { return vnScriptName; }
            set
            {
                vnScriptName = value;
                vnScript = VNScript.ParseVNScriptToIL(AssetsManager.LoadVNScript(value).ToArray());
                _vnScriptCount = vnScript.Count;
            }
        }

        private void Awake()
        {
            GameState.NextCommand += NextILCommand;

            executeCommand += ExecuteBgmCommand;
            executeCommand += ExecuteBgsCommand;
            executeCommand += ExecuteChsCommand;
            executeCommand += ExecuteGmsCommand;

            executeCommand += ExecuteBgpCommand;
            executeCommand += ExecuteChlpCommand;
            executeCommand += ExecuteChmpCommand;
            executeCommand += ExecuteChrpCommand;
            executeCommand += ExecuteNameCommand;
            executeCommand += ExecuteDialogueCommand;

            executeCommand += ExecuteGmCommand;

            VNScriptFileName = AssetsManager.GetFileNameFromChapterName(ConfigController.CurrentChapterName);
        }

        private void Start()
        {
            NextILCommand();
        }

        private void OnDestroy()
        {
            GameState.NextCommand -= NextILCommand;

            executeCommand -= ExecuteBgmCommand;
            executeCommand -= ExecuteBgsCommand;
            executeCommand -= ExecuteChsCommand;
            executeCommand -= ExecuteGmsCommand;

            executeCommand -= ExecuteBgpCommand;
            executeCommand -= ExecuteChlpCommand;
            executeCommand -= ExecuteChmpCommand;
            executeCommand -= ExecuteChrpCommand;
            executeCommand -= ExecuteNameCommand;
            executeCommand -= ExecuteDialogueCommand;

            executeCommand -= ExecuteGmCommand;
        }

        public void NextILCommand()
        {
            autoExecuteCommand = true;
            if (GameState.IsDialogueTyping.Invoke())
            {
                GameState.DialogueStop();
                return;
            }

            while (autoExecuteCommand && vnScriptIndex < _vnScriptCount)
            {
                var commands = ILScript.ParseILToAsm(vnScript[vnScriptIndex]);

                foreach (var command in commands)
                {
                    Debug.Log(command);
                    ExecuteAsmCommand(command);
                }

                vnScriptIndex++;
            }
        }

        private void ExecuteAsmCommand(string command)
        {
            var asmHash = AsmScript.ParseAsmToHash(command);
            executeCommand(asmHash);
        }

        #region Execute Picturebox Command
        private void ExecuteBgpCommand(Hashtable hash)
        {
            if ((string)hash["object"] == "bgp") GameState.BgpChanged(hash);
        }

        private void ExecuteChlpCommand(Hashtable hash)
        {
            if ((string)hash["object"] == "ch_left") GameState.ChlpChanged(hash);
        }

        private void ExecuteChmpCommand(Hashtable hash)
        {
            if ((string)hash["object"] == "ch_mid") GameState.ChmpChanged(hash);
        }

        private void ExecuteChrpCommand(Hashtable hash)
        {
            if ((string)hash["object"] == "ch_right") GameState.ChrpChanged(hash);
        }
        #endregion

        #region Execute Textbox Command
        private void ExecuteNameCommand(Hashtable hash)
        {
            if ((string)hash["object"] == "name") GameState.NameChanged(hash);
        }

        private void ExecuteDialogueCommand(Hashtable hash)
        {
            if ((string)hash["object"] == "dialogue")
            {
                GameState.DialogueChanged(hash);
            }
        }
        #endregion

        #region Execute Audio Command
        private void ExecuteBgmCommand(Hashtable hash)
        {
            if( (string)hash["object"] == "bgm" ) GameState.AudioChanged(hash);
        }

        private void ExecuteBgsCommand(Hashtable hash)
        {
            if( (string)hash["object"] == "bgs" ) GameState.AudioChanged(hash);
        }

        private void ExecuteChsCommand(Hashtable hash)
        {
            if( (string)hash["object"] == "chs" ) GameState.AudioChanged(hash);
        }

        private void ExecuteGmsCommand(Hashtable hash)
        {
            if( (string)hash["object"] == "gms" ) GameState.AudioChanged(hash);
        }
        #endregion

        private void ExecuteGmCommand(Hashtable hash)
        {
            if ((string)hash["object"] != "gm") return;

            if ((string)hash["action"] == "stop")
            {
                autoExecuteCommand = false;
            }
            if ((string)hash["action"] == "finish") {
                Debug.Log("Finish");
                AssetsManager.SaveChapterRecord();
                GameState.UIChanged(VNutils.Hash("object", "chapter", "action", "show"));
            }
        }
    }
}
