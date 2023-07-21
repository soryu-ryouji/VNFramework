using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VNFramework.ScriptCompiler;

namespace VNFramework
{
    public class PerformanceController : MonoBehaviour, IController
    {
        private DialoguePanelController _dialoguePanelController;
        private List<string> _vnScript;
        private string _vnScriptName;
        private int _scriptIndex;
        private int _vnScriptCount;
        private bool _autoExecuteCommand;
        private UnityAction<Hashtable> executeCommand;

        private PerformingModel _performingModel;

        private void Start()
        {
            executeCommand += ExecuteAudioCommand;
            executeCommand += ExecuteBgpCommand;
            executeCommand += ExecuteChlpCommand;
            executeCommand += ExecuteChmpCommand;
            executeCommand += ExecuteChrpCommand;
            executeCommand += ExecuteNameCommand;
            executeCommand += ExecuteDialogueCommand;
            executeCommand += ExecuteGmCommand;

            _dialoguePanelController = transform.Find("DialoguePanel").GetComponent<DialoguePanelController>();

            var chapterModel = this.GetModel<ChapterModel>();
            string fileName = chapterModel.GetFileName(chapterModel.CurrentChapter);
            _vnScript = VNScript.ParseVNScriptToIL(this.GetUtility<GameDataStorage>().LoadVNScript(fileName));
            _vnScriptCount = _vnScript.Count;

            _performingModel = this.GetModel<PerformingModel>();

            this.RegisterEvent<LoadNextPerformanceEvent>(_ => NextPerformance());
            NextPerformance();
        }

        private void OnDestroy()
        {
            executeCommand -= ExecuteAudioCommand;

            executeCommand -= ExecuteBgpCommand;
            executeCommand -= ExecuteChlpCommand;
            executeCommand -= ExecuteChmpCommand;
            executeCommand -= ExecuteChrpCommand;
            executeCommand -= ExecuteNameCommand;
            executeCommand -= ExecuteDialogueCommand;

            executeCommand -= ExecuteGmCommand;
        }

        private void NextPerformance()
        {
            NextILCommand();
        }

        private void NextILCommand()
        {
            _autoExecuteCommand = true;
            if (_dialoguePanelController.IsAnimating)
            {
                this.SendCommand<StopDialogueAnimCommand>();
                return;
            }

            while (_autoExecuteCommand && _scriptIndex < _vnScriptCount)
            {
                var commands = ILScript.ParseILToAsm(_vnScript[_scriptIndex]);

                foreach (var command in commands)
                {
                    Debug.Log(command);
                    ExecuteAsmCommand(command);
                }

                _scriptIndex++;
            }
        }

        private void ExecuteAsmCommand(string command)
        {
            var asmHash = AsmScript.ParseAsmToHash(command);
            executeCommand(asmHash);
        }

        #region Execute Picture box Command
        private void ExecuteBgpCommand(Hashtable hash)
        {
            if ((string)hash["object"] != "bgp") return;

            GameState.BgpChanged(hash);
        }

        private void ExecuteChlpCommand(Hashtable hash)
        {
            if ((string)hash["object"] != "ch_left") return;

            GameState.ChlpChanged(hash);
        }

        private void ExecuteChmpCommand(Hashtable hash)
        {
            if ((string)hash["object"] != "ch_mid") return;
            GameState.ChmpChanged(hash);
        }

        private void ExecuteChrpCommand(Hashtable hash)
        {
            if ((string)hash["object"] != "ch_right") return;

            GameState.ChrpChanged(hash);
        }
        #endregion

        #region Execute Text box Command
        private void ExecuteNameCommand(Hashtable hash)
        {
            if ((string)hash["object"] != "name") return;

            var action = (string)hash["action"];

            if (action == "append") this.SendCommand(new ChangeNameCommand((string)hash["name"]));
            else if (action == "clear") this.SendCommand(new ChangeNameCommand(""));
            else Debug.LogWarning("Name Command Not Found");
        }

        private void ExecuteDialogueCommand(Hashtable hash)
        {
            if ((string)hash["object"] != "dialogue") return;

            var action = (string)hash["action"];

            if (action == "append") this.SendCommand(new AppendDialogueCommand((string)hash["dialogue"]));
            else if (action == "clear") this.SendCommand<ClearDialogueCommand>();
            else if (action == "newline") this.SendCommand<AppendNewlineToDialogueCommand>();
            else Debug.LogWarning("Dialogue Command Not Found");
        }
        #endregion

        private void ExecuteAudioCommand(Hashtable hash)
        {
            string obj = (string)hash["object"];

            if (obj == "bgm" || obj == "bgs" || obj == "chs" || obj == "gms") GameState.AudioChanged(hash);
        }

        private void ExecuteGmCommand(Hashtable hash)
        {
            if ((string)hash["object"] != "gm") return;

            if ((string)hash["action"] == "stop")
            {
                _autoExecuteCommand = false;
            }
            if ((string)hash["action"] == "finish")
            {
                Debug.Log("Finish");
                this.SendCommand(new UnlockedChapterCommand(this.GetModel<ChapterModel>().CurrentChapter));
                this.SendCommand<ShowChapterViewCommand>();
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
