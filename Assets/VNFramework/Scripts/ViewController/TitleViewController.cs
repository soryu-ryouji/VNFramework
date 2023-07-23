using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class TitleViewController : MonoBehaviour, IController
    {
        private Image _titlePic;
        private Image _bgp;

        private Button _startBtn;
        private Button _chapterViewBtn;
        private Button _configViewBtn;
        private Button _exitGameBtn;

        private void Start()
        {
            _titlePic = transform.Find("TitlePic").GetComponent<Image>();
            _bgp = transform.Find("ViewBgp").GetComponent<Image>();
            _startBtn = transform.Find("ButtonList/StartButton").GetComponent<Button>();
            _chapterViewBtn = transform.Find("ButtonList/ChapterButton").GetComponent<Button>();
            _configViewBtn = transform.Find("ButtonList/ConfigButton").GetComponent<Button>();
            _exitGameBtn = transform.Find("ButtonList/ExitButton").GetComponent<Button>();

            var projectModel = this.GetModel<ProjectModel>();
            _titlePic.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.TitlePic);
            _bgp.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.TitleBgp);
            this.SendCommand(new PlayAudioCommand(projectModel.TitleBgm, "bgm"));

            _startBtn.onClick.AddListener(StartGame);
            _chapterViewBtn.onClick.AddListener(ShowChapterView);
            _configViewBtn.onClick.AddListener(ShowConfigView);
            _exitGameBtn.onClick.AddListener(ExitGame);
        }

        public void StartGame()
        {
            var chapterModel = this.GetModel<ChapterModel>();
            chapterModel.CurrentChapter = chapterModel.ChapterInfoList[0].ChapterName;
            this.SendCommand<LoadGameSceneCommand>();
        }

        public void ShowChapterView()
        {
            this.SendCommand<ShowChapterViewCommand>();
        }

        public void ShowConfigView()
        {
            this.SendCommand<ShowConfigViewCommand>();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
