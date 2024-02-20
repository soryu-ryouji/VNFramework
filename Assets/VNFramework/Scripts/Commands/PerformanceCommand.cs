using System.Collections.Generic;
using UnityEngine;

namespace VNFramework
{
    public class NextPerformanceCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<LoadNextPerformanceEvent>();
        }
    }

    public class InitPerformanceCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<InitPerformanceEvent>();
        }
    }

    public class InitPerformanceEnvironmentCommand : AbstractCommand
    {
        private PerformanceState _state;

        public InitPerformanceEnvironmentCommand(PerformanceState state)
        {
            _state = state;
        }

        protected override void OnExecute()
        {
            if (_state.isFullDialogueBox) this.SendCommand(new ExecuteDialogueCommand(new VNScriptAsm(AsmObj.dialogue, "switch", new() { "full" })));
            else this.SendCommand(new ExecuteDialogueCommand(new VNScriptAsm(AsmObj.dialogue, "switch", new() { "norm" })));

            if (!string.IsNullOrWhiteSpace(_state.Bgm))
                this.SendCommand(new ExecuteAudioCommand(new VNScriptAsm(AsmObj.bgm, "play", new() { _state.Bgm })));
            if (!string.IsNullOrWhiteSpace(_state.Bgp))
                this.SendCommand(new ExecuteSpriteCommand(new VNScriptAsm(AsmObj.bgp, "show", new List<string> { _state.Bgp, "immediate" })));
            if (!string.IsNullOrWhiteSpace(_state.ChMid))
                this.SendCommand(new ExecuteSpriteCommand(new VNScriptAsm(AsmObj.ch_mid, "show", new List<string> { _state.ChMid, "immediate" })));
            if (!string.IsNullOrWhiteSpace(_state.ChRight))
                this.SendCommand(new ExecuteSpriteCommand(new VNScriptAsm(AsmObj.ch_right, "show", new List<string> { _state.ChRight, "immediate" })));
            if (!string.IsNullOrWhiteSpace(_state.ChLeft))
                this.SendCommand(new ExecuteSpriteCommand(new VNScriptAsm(AsmObj.ch_left, "show", new List<string> { _state.ChLeft, "immediate" })));
        }
    }

    public class LoadNextMermaidOrEndGameCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var performanceModel = this.GetModel<PerformanceModel>();
            var mermaidModel = this.GetModel<MermaidModel>();

            performanceModel.IsOpenChooseView = true;
            var children = mermaidModel.GetMermaidChildren(performanceModel.PerformingMermaidName);
            if (children.Count > 0)
            {
                // 当只有一个选项时，直接跳转到下一个演出
                // 当有两个以上选项时，弹出 Choose View 界面
                if (children.Count == 1 || children[0].optionText == "")
                {
                    performanceModel.PerformingMermaidName = children[0].childMermaidName;
                    performanceModel.PerformingIndex = 0;
                    performanceModel.IsOpenChooseView = false;
                    this.SendEvent<InitPerformanceEvent>();
                }
                else
                {
                    performanceModel.ChooseList = children;
                    this.SendCommand<ShowChooseViewCommand>();
                }
            }
            else
            {
                // 演出结束，回到开始界面
                this.SendCommand<LoadStartUpSceneCommand>();
            }
        }
    }

    public class ExecuteAsmCommand : AbstractCommand
    {
        private VNScriptAsm _asm;

        public ExecuteAsmCommand(VNScriptAsm asm)
        {
            _asm = asm;
        }

        protected override void OnExecute()
        {
            switch (_asm.Obj)
            {
                case AsmObj.dialogue: this.SendCommand(new ExecuteDialogueCommand(_asm)); break;
                case AsmObj.name: this.SendCommand(new ExecuteNameCommand(_asm)); break;

                case AsmObj.bgm: this.SendCommand(new ExecuteAudioCommand(_asm)); break;
                case AsmObj.bgs: this.SendCommand(new ExecuteAudioCommand(_asm)); break;
                case AsmObj.chs: this.SendCommand(new ExecuteAudioCommand(_asm)); break;
                case AsmObj.gms: this.SendCommand(new ExecuteAudioCommand(_asm)); break;

                case AsmObj.ch_left: this.SendCommand(new ExecuteSpriteCommand(_asm)); break;
                case AsmObj.ch_mid: this.SendCommand(new ExecuteSpriteCommand(_asm)); break;
                case AsmObj.ch_right: this.SendCommand(new ExecuteSpriteCommand(_asm)); break;
                case AsmObj.bgp: this.SendCommand(new ExecuteSpriteCommand(_asm)); break;

                case AsmObj.gm: this.SendCommand(new ExecuteGmCommand(_asm)); break;
            }
        }
    }

    class ExecuteNameCommand : AbstractCommand
    {
        private VNScriptAsm _asm;

        public ExecuteNameCommand(VNScriptAsm asm)
        {
            _asm = asm;
        }

        protected override void OnExecute()
        {
            if (_asm.Action == "append") this.SendCommand(new ChangeNameCommand(_asm.Parameters[0]));
            else if (_asm.Action == "clear") this.SendCommand(new ChangeNameCommand(""));
        }
    }

    class ExecuteDialogueCommand : AbstractCommand
    {
        private VNScriptAsm _asm;

        public ExecuteDialogueCommand(VNScriptAsm asm)
        {
            _asm = asm;
        }

        protected override void OnExecute()
        {
            Debug.Log($"Dialogue Command : {_asm.ToString()}");
            if (_asm.Action == "append") this.SendCommand(new AppendDialogueCommand(_asm.Parameters[0]));
            else if (_asm.Action == "clear") this.SendCommand<ClearDialogueCommand>();
            else if (_asm.Action == "newline") this.SendCommand<AppendNewlineToDialogueCommand>();
            else if (_asm.Action == "switch")
            {
                if (_asm.Parameters[0] == "full") this.SendCommand<OpenFullDialogueBoxCommand>();
                else if (_asm.Parameters[0] == "norm") this.SendCommand<OpenNormDialogueBoxCommand>();
            }
        }
    }

    class ExecuteAudioCommand : AbstractCommand
    {
        private VNScriptAsm _asm;

        public ExecuteAudioCommand(VNScriptAsm asm)
        {
            _asm = asm;
        }

        protected override void OnExecute()
        {
            if (_asm.Action == "play") this.SendCommand(new PlayAudioCommand(_asm.Parameters[0], _asm.Obj));
            else if (_asm.Action == "stop") this.SendCommand(new StopAudioCommand(_asm.Obj));
        }
    }

    class ExecuteSpriteCommand : AbstractCommand
    {
        private VNScriptAsm _asm;

        public ExecuteSpriteCommand(VNScriptAsm asm)
        {
            _asm = asm;
        }

        protected override void OnExecute()
        {
            if (_asm.Action == "show") this.SendCommand(new ShowSpriteCommand(_asm.Obj, _asm.Parameters[0], _asm.Parameters[1]));
            else if (_asm.Action == "hide") this.SendCommand(new HideSpriteCommand(_asm.Obj, _asm.Parameters[0]));
        }
    }


    class ExecuteGmCommand : AbstractCommand
    {
        private VNScriptAsm _asm;

        public ExecuteGmCommand(VNScriptAsm asm)
        {
            _asm = asm;
        }

        protected override void OnExecute()
        {
            if (_asm.Action == "stop") this.GetModel<PerformanceModel>().IsAutoExecuteCommand = false;
        }
    }
}