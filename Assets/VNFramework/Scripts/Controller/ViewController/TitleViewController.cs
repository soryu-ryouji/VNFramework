using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class TitleViewController : MonoBehaviour, IController
    {
        private Image _titlePic;
        private Image _bgp;

        private Button _startBtn;
        private Button _loadBtn;
        private Button _chapterViewBtn;
        private Button _configViewBtn;
        private Button _exitGameBtn;

        private void Start()
        {
            var projectModel = this.GetModel<ProjectModel>();

            // 对 View 进行初始化
            _titlePic = transform.Find("TitlePic").GetComponent<Image>();
            _bgp = transform.Find("ViewBgp").GetComponent<Image>();
            _titlePic.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.TitleViewLogo);
            _bgp.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.TitleViewBgp);
            this.SendCommand(new PlayAudioCommand(projectModel.TitleViewBgm, AsmObj.bgm));

            _startBtn = transform.Find("ButtonList/StartButton").GetComponent<Button>();
            _loadBtn = transform.Find("ButtonList/LoadButton").GetComponent<Button>();
            _chapterViewBtn = transform.Find("ButtonList/ChapterButton").GetComponent<Button>();
            _configViewBtn = transform.Find("ButtonList/ConfigButton").GetComponent<Button>();
            _exitGameBtn = transform.Find("ButtonList/ExitButton").GetComponent<Button>();

            _startBtn.onClick.AddListener(() =>
            {
                var mermaidModel = this.GetModel<MermaidModel>();
                var performanceModel = this.GetModel<PerformanceModel>();
                performanceModel.PerformingMermaidName = mermaidModel.GetFirstMermaidName();
                performanceModel.PerformingIndex = 0;
                this.SendCommand<LoadGameSceneCommand>();
            });

            _loadBtn.onClick.AddListener(() => this.SendCommand<ShowLoadGameSaveViewCommand>());
            _chapterViewBtn.onClick.AddListener(() => this.SendCommand<ShowChapterViewCommand>());
            _configViewBtn.onClick.AddListener(() => this.SendCommand<ShowConfigViewCommand>());
            _exitGameBtn.onClick.AddListener(() => Application.Quit());
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
