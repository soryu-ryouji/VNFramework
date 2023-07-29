using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VNFramework.VNScriptCompiler;

namespace VNFramework
{
    public class PerformanceController : MonoBehaviour, IController
    {
        private DialogueViewController _dialogueViewController;
        private List<string> _vnScript;
        private int _scriptIndex;
        private int _vnScriptCount;
        private bool _autoExecuteCommand;
        private UnityAction<Hashtable> executeCommand;

        private PerformingModel _performingModel;

        private VNScript vnScriptCompiler = new();

        private GameLog gameLog;

        private void Start()
        {
            executeCommand += ExecuteAudioCommand;
            executeCommand += ExecuteSpriteCommand;
            executeCommand += ExecuteNameCommand;
            executeCommand += ExecuteDialogueCommand;
            executeCommand += ExecuteGmCommand;

            _dialogueViewController = transform.Find("DialogueView").GetComponent<DialogueViewController>();

            var chapterModel = this.GetModel<ChapterModel>();
            string fileName = chapterModel.GetFileName(chapterModel.CurrentChapter);
            _vnScript = new(this.GetUtility<GameDataStorage>().LoadVNScript(fileName));
            _vnScriptCount = _vnScript.Count;

            _performingModel = this.GetModel<PerformingModel>();
            gameLog = this.GetUtility<GameLog>();

            this.RegisterEvent<LoadNextPerformanceEvent>(_ => NextPerformance());
            NextPerformance();
        }

        private void OnDestroy()
        {
            executeCommand -= ExecuteAudioCommand;
            executeCommand -= ExecuteSpriteCommand;
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
            if (_dialogueViewController.IsAnimating)
            {
                this.SendCommand<StopDialogueAnimCommand>();
                return;
            }

            while (_autoExecuteCommand && _scriptIndex < _vnScriptCount)
            {
                var commands = ILScript.ParseILToAsm(vnScriptCompiler.ParseVNScriptToIL(_vnScript[_scriptIndex])
                    .ToArray());

                foreach (var command in commands)
                {
                    gameLog.RunningLog("asm -> " + command);
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
        private void ExecuteSpriteCommand(Hashtable hash)
        {
            var obj = (string)hash["object"];
            if (obj == "bgp" || obj == "ch_mid")
            {
                var action = (SpriteAction)hash["action"];

                if (action == SpriteAction.Show)
                    this.SendCommand(new ShowSpriteCommand(VNutils.StrToSpriteObj(obj), (string)hash["sprite_name"], (SpriteMode)hash["mode"]));
                else if (action == SpriteAction.Hide)
                    this.SendCommand(new HideSpriteCommand(VNutils.StrToSpriteObj(obj), (SpriteMode)hash["mode"]));
            }
        }
        #endregion

        #region Execute Text box Command
        private void ExecuteNameCommand(Hashtable hash)
        {
            if ((string)hash["object"] != "name") return;

            var action = (string)hash["action"];

            if (action == "append") this.SendCommand(new ChangeNameCommand((string)hash["name"]));
            else if (action == "clear") this.SendCommand(new ChangeNameCommand(""));
        }

        private void ExecuteDialogueCommand(Hashtable hash)
        {
            if ((string)hash["object"] != "dialogue") return;

            var action = (string)hash["action"];

            if (action == "append") this.SendCommand(new AppendDialogueCommand((string)hash["dialogue"]));
            else if (action == "clear") this.SendCommand<ClearDialogueCommand>();
            else if (action == "newline") this.SendCommand<AppendNewlineToDialogueCommand>();
            else if (action == "switch")
            {
                var mode = (string)hash["mode"];
                if (mode == "full") this.SendCommand<OpenFullDialogueBoxCommand>();
                else if (mode == "norm") this.SendCommand<OpenNormDialogueBoxCommand>();
            }
        }
        #endregion

        private void ExecuteAudioCommand(Hashtable hash)
        {
            string obj = (string)hash["object"];

            if (obj == "bgm" || obj == "bgs" || obj == "chs" || obj == "gms")
            {
                var action = (AudioAction)hash["action"];
                if (action == AudioAction.Play) this.SendCommand(new PlayAudioCommand((string)hash["audio_name"], VNutils.StrToAudioPlayer(obj)));
                else if (action == AudioAction.Stop) this.SendCommand(new StopAudioCommand(VNutils.StrToAudioPlayer(obj)));
            }
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
