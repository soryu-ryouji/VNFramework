using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace VNFramework
{
    public class ChapterViewController : MonoBehaviour, IController
    {
        private GameObject _buttonPrefab;
        private Scrollbar _buttonListScrollbar;
        private GameObject _buttonListContent;
        private VerticalLayoutGroup _buttonListLayoutGroup;
        private Image _chapterPic;
        private Image _resumePic;
        private TMP_Text _resumeText;
        private Button _backChapterBtn;
        private Button _loadChapterBtn;

        private ChapterModel _chapterModel;

        private void Start()
        {
            _chapterModel = this.GetModel<ChapterModel>();

            _buttonPrefab = this.GetUtility<GameDataStorage>().LoadPrefab("ChapterButton");
            _buttonListScrollbar = transform.Find("ButtonListScrollbar").GetComponent<Scrollbar>();
            _buttonListContent = transform.Find("ChapterList/Content").gameObject;
            _buttonListLayoutGroup = _buttonListContent.GetComponent<VerticalLayoutGroup>();
            _chapterPic = transform.Find("ChapterViewBgp").GetComponent<Image>();
            _resumePic = transform.Find("ResumePic").GetComponent<Image>();
            _resumeText = transform.Find("ResumeText").GetComponent<TMP_Text>();
            _backChapterBtn = transform.Find("BackButton").GetComponent<Button>();
            _loadChapterBtn = transform.Find("LoadChapterButton").GetComponent<Button>();

            _backChapterBtn.onClick.AddListener(this.SendCommand<HideChapterViewCommand>);
            _loadChapterBtn.onClick.AddListener(() =>
            {
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    this.SendCommand<HidePerformanceViewCommand>();
                }

                var performanceModel = this.GetModel<PerformanceModel>();
                performanceModel.PerformingMermaidName = _chapterModel.CurrentChapter;
                performanceModel.PerformingIndex = 0;

                this.SendCommand<LoadGameSceneCommand>();
            });

            var firstChapter = _chapterModel.ChapterInfoList[0];
            _resumePic.sprite = this.GetUtility<GameDataStorage>().LoadSprite(firstChapter.ResumePic);
            _resumeText.text = firstChapter.ResumeText;

            GenerateChapterList(_chapterModel.UnlockedChapterList);
            _buttonListScrollbar.value = 1;
        }

        public void GenerateChapterList(List<string> unlockedChapterList)
        {
            foreach (var mermaidName in unlockedChapterList)
            {
                // 创建按钮实例
                GameObject buttonObject = Instantiate(_buttonPrefab, _buttonListContent.transform);

                // 对按钮进行初始化
                var chapterInfo = this.GetModel<ChapterModel>().GetChapterInfo(mermaidName);
                Button button = buttonObject.GetComponent<Button>();
                button.GetComponentInChildren<TMP_Text>().text = chapterInfo.ShowName;
                button.onClick.AddListener(() => OnClickChapterButton(chapterInfo));

                // 调整布局
                _buttonListLayoutGroup.CalculateLayoutInputVertical();
                _buttonListLayoutGroup.SetLayoutVertical();
            }
        }

        public void ClearChapterButtonList()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void OnClickChapterButton(ChapterInfo chapterInfo)
        {
            _chapterModel.CurrentChapter = chapterInfo.MermaidName;
            _resumeText.text = chapterInfo.ResumeText;
            _resumePic.sprite = this.GetUtility<GameDataStorage>().LoadSprite(chapterInfo.ResumePic);
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}