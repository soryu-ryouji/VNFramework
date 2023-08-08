using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using VNFramework.Core;

namespace VNFramework
{
    public class PerformanceController : MonoBehaviour, IController
    {
        private DialogueViewController _dialogueViewController;
        private bool _autoExecuteCommand;
        private PerformanceModel _performanceModel;
        private VNScriptCompiler _compiler;

        private void Start()
        {
            _dialogueViewController = transform.Find("DialogueView").GetComponent<DialogueViewController>();
            
            _performanceModel = this.GetModel<PerformanceModel>();
            var chapterModel = this.GetModel<ChapterModel>();
            
            string fileName = chapterModel.GetFileName(chapterModel.CurrentChapter);
            var fileLines = this.GetUtility<GameDataStorage>().LoadVNScript(fileName);
            _compiler = new(fileLines);

            this.RegisterEvent<LoadNextPerformanceEvent>(_ => NextPerformance());
            NextPerformance();
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

            while (_autoExecuteCommand && _compiler.ScriptCountDown() > 0)
            {
                var asmList = _compiler.NextAsmList();

                foreach (var asm in asmList)
                {
                    Debug.Log("asm -> " + asm);
                    ExecuteAsmCommand(asm);
                }
            }
        }

        private void ExecuteAsmCommand(VNScriptAsm asm)
        {
            switch (asm.Obj)
            {
                case AsmObj.dialogue:
                    ExecuteDialogueCommand(asm);
                    break;
                case AsmObj.name:
                    ExecuteNameCommand(asm);
                    break;
                case AsmObj.bgm:
                    ExecuteAudioCommand(asm);
                    break;
                case AsmObj.bgs:
                    ExecuteAudioCommand(asm);
                    break;
                case AsmObj.chs:
                    ExecuteAudioCommand(asm);
                    break;
                case AsmObj.gms:
                    ExecuteAudioCommand(asm);
                    break;
                case AsmObj.ch_left:
                    ExecuteSpriteCommand(asm);
                    break;
                case AsmObj.ch_mid:
                    ExecuteSpriteCommand(asm);
                    break;
                case AsmObj.ch_right:
                    ExecuteSpriteCommand(asm);
                    break;
                case AsmObj.bgp:
                    ExecuteSpriteCommand(asm);
                    break;
                case AsmObj.gm:
                    ExecuteGmCommand(asm);
                    break;
            }
        }

        #region Execute Picture box Command
        private void ExecuteSpriteCommand(VNScriptAsm hash)
        {
            if (hash.Action == "show")
                this.SendCommand(new ShowSpriteCommand(hash.Obj, hash.Parameters[0], hash.Parameters[1]));
            else if (hash.Action == "hide")
                this.SendCommand(new HideSpriteCommand(hash.Obj, hash.Parameters[0]));
        }
        #endregion

        #region Execute Text box Command
        private void ExecuteNameCommand(VNScriptAsm hash)
        {
            if (hash.Action == "append") this.SendCommand(new ChangeNameCommand(hash.Parameters[0]));
            else if (hash.Action == "clear") this.SendCommand(new ChangeNameCommand(""));
        }

        private void ExecuteDialogueCommand(VNScriptAsm hash)
        {
            if (hash.Action == "append") this.SendCommand(new AppendDialogueCommand(hash.Parameters[0]));
            else if (hash.Action == "clear") this.SendCommand<ClearDialogueCommand>();
            else if (hash.Action == "newline") this.SendCommand<AppendNewlineToDialogueCommand>();
            else if (hash.Action == "switch")
            {
                if (hash.Parameters[0] == "full") this.SendCommand<OpenFullDialogueBoxCommand>();
                else if (hash.Parameters[0] == "norm") this.SendCommand<OpenNormDialogueBoxCommand>();
            }
        }
        #endregion

        private void ExecuteAudioCommand(VNScriptAsm asm)
        {
            if (asm.Action == "play") this.SendCommand(new PlayAudioCommand(asm.Parameters[0], asm.Obj));
            else if (asm.Action == "stop") this.SendCommand(new StopAudioCommand(asm.Obj));
        }

        private void ExecuteGmCommand(VNScriptAsm hash)
        {
            if (hash.Action == "stop")
            {
                _autoExecuteCommand = false;
            }
            if (hash.Action == "finish")
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
