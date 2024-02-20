using UnityEngine;
using VNFramework.Core;

namespace VNFramework
{
    public class PerformanceController : MonoBehaviour, IController
    {
        private PerformanceModel _performanceModel;
        private MermaidModel _mermaidModel;
        private VNScriptCompiler _compiler;
        private DialogModel _dialogModel;
        private DialogueViewController _dialogueViewController;

        public void InitController()
        {
            Debug.Log("<color=green>Init Performance Controller</color>");
            _performanceModel = this.GetModel<PerformanceModel>();
            _mermaidModel = this.GetModel<MermaidModel>();
            _dialogModel = this.GetModel<DialogModel>();

            _dialogueViewController = transform.Find("DialogView").GetComponent<DialogueViewController>();
            _dialogueViewController.Init();

            InitPerformance();
            this.RegisterEvent<LoadNextPerformanceEvent>(_ => NextPerformance()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<InitPerformanceEvent>(_ => InitPerformance()).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void InitPerformance()
        {
            _dialogModel.InitModel();
            _performanceModel.Init();

            var nodeName = _performanceModel.PerformingMermaidName;
            var fileName = _mermaidModel.GetFileName(nodeName);
            var fileLines = this.GetUtility<GameDataStorage>().LoadVNScript(fileName);

            // 每次演出进行初始化时，尝试将当前的章节添加进UnlockedChapterList中
            this.GetModel<ChapterModel>().TryAddUnlockedChapter(_performanceModel.PerformingMermaidName);

            // 对演出进行初始化
            var performanceState = VNScriptCompiler.GetPerformanceStateByIndex(fileLines, _performanceModel.PerformingIndex);
            _compiler = new VNScriptCompiler(fileLines);
            _compiler.InitByLine(performanceState.ScriptIndex);

            this.SendCommand(new InitPerformanceEnvironmentCommand(performanceState));

            Debug.Log("<color=green>Init Performance</color>");
        }

        private void NextPerformance()
        {
            if (_performanceModel.IsOpenChooseView) return;

            _performanceModel.IsAutoExecuteCommand = true;

            // 当对话动画正在播放时，此次只负责结束对话动画
            if (_dialogModel.IsAnimating) { this.SendCommand<StopDialogueAnimCommand>(); return; }

            // 当剧本已经演出完毕时，尝试加载下一章动画，或者回到开始界面
            if (_compiler.ScriptCountDown() <= 0) { this.SendCommand<LoadNextMermaidOrEndGameCommand>(); return; }

            // 正常情况下，在 auto execute 为真时，不断向下执行
            while (_performanceModel.IsAutoExecuteCommand && _compiler.ScriptCountDown() > 0)
            {
                var asmList = _compiler.NextAsmList();
                _performanceModel.PerformingIndex = _compiler.ScriptIndex;

                foreach (var asm in asmList)
                {
                    Debug.Log("asm -> " + asm);
                    this.SendCommand(new ExecuteAsmCommand(asm));
                }
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
