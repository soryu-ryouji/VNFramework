using UnityEngine;
using VNFramework.Core;
using System.Collections.Generic;

namespace VNFramework
{
    public class PerformanceController : MonoBehaviour, IController
    {
        private bool _autoExecuteCommand;
        private PerformanceModel _performanceModel;
        private MermaidModel _mermaidModel;
        private VNScriptCompiler _compiler;
        private DialogueModel _dialogueModel;

        private void Start()
        {
            this.RegisterEvent<LoadNextPerformanceEvent>(_ => NextPerformance()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<InitPerformanceEvent>(_ => InitPerformance()).UnRegisterWhenGameObjectDestroyed(gameObject);

            InitPerformance();
        }

        private void InitPerformance()
        {
            _performanceModel = this.GetModel<PerformanceModel>();
            _mermaidModel = this.GetModel<MermaidModel>();
            _dialogueModel = this.GetModel<DialogueModel>();
            _dialogueModel.InitModel();

            var nodeName = _performanceModel.PerformingMermaidName;
            var fileName = _mermaidModel.GetFileName(nodeName);
            var fileLines = this.GetUtility<GameDataStorage>().LoadVNScript(fileName);
            
            // 对演出进行初始化
            var performanceState = VNScriptCompiler.GetPerformanceStateByIndex(fileLines, _performanceModel.PerformingIndex, out _compiler);
            _compiler.InitByLine(performanceState.ScriptIndex);

            if (performanceState.isFullDialogueBox) ExecuteDialogueCommand(new VNScriptAsm(AsmObj.dialogue, "switch", new() { "full" }));
            else { ExecuteDialogueCommand(new VNScriptAsm(AsmObj.dialogue, "switch", new() { "norm" })); }

            if (!string.IsNullOrWhiteSpace(performanceState.Bgm))
                ExecuteAudioCommand(new VNScriptAsm(AsmObj.bgm, "play", new() { performanceState.Bgm }));
            if (!string.IsNullOrWhiteSpace(performanceState.Bgp))
                ExecuteSpriteCommand(new VNScriptAsm(AsmObj.bgp, "show", new List<string> { performanceState.Bgp, "immediate" }));
            if (!string.IsNullOrWhiteSpace(performanceState.ChMid))
                ExecuteSpriteCommand(new VNScriptAsm(AsmObj.ch_mid, "show", new List<string> { performanceState.ChMid, "immediate" }));
            if (!string.IsNullOrWhiteSpace(performanceState.ChRight))
                ExecuteSpriteCommand(new VNScriptAsm(AsmObj.ch_right, "show", new List<string> { performanceState.ChRight, "immediate" }));
            if (!string.IsNullOrWhiteSpace(performanceState.ChLeft))
                ExecuteSpriteCommand(new VNScriptAsm(AsmObj.ch_left, "show", new List<string> { performanceState.ChLeft, "immediate" }));

            NextPerformance();
        }

        private void NextPerformance()
        {
            if (_performanceModel.IsOpenChooseView == false) NextILCommand();
        }

        private void NextILCommand()
        {
            _autoExecuteCommand = true;
            if (_dialogueModel.isAnimating)
            {
                this.SendCommand<StopDialogueAnimCommand>();
                return;
            }

            if (_compiler.ScriptCountDown() <= 0)
            {
                _performanceModel.IsOpenChooseView = true;
                var children = _mermaidModel.GetMermaidChildren(_performanceModel.PerformingMermaidName);
                if (children.Count > 0)
                {
                    // 当只有一个选项时，直接跳转到下一个演出
                    if (children.Count == 1 || children[0].optionText == "")
                    {
                        _performanceModel.PerformingMermaidName = children[0].childMermaidName;
                        _performanceModel.PerformingIndex = 0;
                        _performanceModel.IsOpenChooseView = false;
                        InitPerformance();
                    }
                    else
                    {
                        _performanceModel.ChooseList = children;
                        this.SendCommand<ShowChooseViewCommand>();
                    }
                }
                else
                {
                    // 演出结束，回到开始界面
                    this.SendCommand<LoadStartUpSceneCommand>();
                }

                return;
            }

            while (_autoExecuteCommand && _compiler.ScriptCountDown() > 0)
            {
                var asmList = _compiler.NextAsmList();
                _performanceModel.PerformingIndex = _compiler.ScriptIndex;

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
        private void ExecuteSpriteCommand(VNScriptAsm asm)
        {
            if (asm.Action == "show")
                this.SendCommand(new ShowSpriteCommand(asm.Obj, asm.Parameters[0], asm.Parameters[1]));
            else if (asm.Action == "hide")
                this.SendCommand(new HideSpriteCommand(asm.Obj, asm.Parameters[0]));
        }
        #endregion

        #region Execute Text box Command
        private void ExecuteNameCommand(VNScriptAsm asm)
        {
            if (asm.Action == "append") this.SendCommand(new ChangeNameCommand(asm.Parameters[0]));
            else if (asm.Action == "clear") this.SendCommand(new ChangeNameCommand(""));
        }

        private void ExecuteDialogueCommand(VNScriptAsm asm)
        {
            if (asm.Action == "append") this.SendCommand(new AppendDialogueCommand(asm.Parameters[0]));
            else if (asm.Action == "clear") this.SendCommand<ClearDialogueCommand>();
            else if (asm.Action == "newline") this.SendCommand<AppendNewlineToDialogueCommand>();
            else if (asm.Action == "switch")
            {
                if (asm.Parameters[0] == "full") this.SendCommand<OpenFullDialogueBoxCommand>();
                else if (asm.Parameters[0] == "norm") this.SendCommand<OpenNormDialogueBoxCommand>();
            }
        }
        #endregion

        private void ExecuteAudioCommand(VNScriptAsm asm)
        {
            if (asm.Action == "play") this.SendCommand(new PlayAudioCommand(asm.Parameters[0], asm.Obj));
            else if (asm.Action == "stop") this.SendCommand(new StopAudioCommand(asm.Obj));
        }

        private void ExecuteGmCommand(VNScriptAsm asm)
        {
            if (asm.Action == "stop")
            {
                _autoExecuteCommand = false;
            }
            if (asm.Action == "finish")
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
