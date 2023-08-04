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
            _titlePic = transform.Find("TitlePic").GetComponent<Image>();
            _bgp = transform.Find("ViewBgp").GetComponent<Image>();
            _startBtn = transform.Find("ButtonList/StartButton").GetComponent<Button>();
            _loadBtn = transform.Find("ButtonList/LoadButton").GetComponent<Button>();
            _chapterViewBtn = transform.Find("ButtonList/ChapterButton").GetComponent<Button>();
            _configViewBtn = transform.Find("ButtonList/ConfigButton").GetComponent<Button>();
            _exitGameBtn = transform.Find("ButtonList/ExitButton").GetComponent<Button>();

            var projectModel = this.GetModel<ProjectModel>();
            _titlePic.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.TitlePic);
            _bgp.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.TitleBgp);
            this.SendCommand(new PlayAudioCommand(projectModel.TitleBgm, VNutils.StrToAudioPlayer("bgm")));

            _startBtn.onClick.AddListener(() =>
            {
                var chapterModel = this.GetModel<ChapterModel>();
                chapterModel.CurrentChapter = chapterModel.ChapterInfoList[0].ChapterName;
                this.SendCommand<LoadGameSceneCommand>();
            });

            _loadBtn.onClick.AddListener(() => this.SendCommand<ShowSaveFileViewCommand>());
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
