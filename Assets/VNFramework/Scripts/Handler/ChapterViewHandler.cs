using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VNFramework
{
    public class ChapterViewHandler : MonoBehaviour
    {
        public Scrollbar buttonListScrollbar;
        public GameObject buttonListContent;
        public VerticalLayoutGroup buttonListLayoutGroup;

        public GameObject buttonPrefab;
        public Image chapterPic;
        public TMP_Text chapterResume;


        private void Awake()
        {
            var chapterRecord = AssetsManager.LoadChapterRecord();
            var firstRecord = AssetsManager.GetChapterInfoFromChapterName(chapterRecord[0]);
            chapterPic.sprite = AssetsManager.LoadSprite(firstRecord.ResumePic);
            chapterResume.text = firstRecord.Resume;

            GenerateChapterList(chapterRecord);
        }
        public void GenerateChapterList(List<string> chapterNameList)
        {
            foreach (var chapterName in chapterNameList)
            {
                // 创建按钮实例
                GameObject buttonObject = Instantiate(buttonPrefab, buttonListContent.transform);

                // 对按钮进行初始化
                Button button = buttonObject.GetComponent<Button>();
                button.GetComponentInChildren<TMP_Text>().text = chapterName;

                button.onClick.AddListener(() => OnClickChapterButton(chapterName));

                // 调整布局
                buttonListLayoutGroup.CalculateLayoutInputVertical();
                buttonListLayoutGroup.SetLayoutVertical();
            }
        }

        public void ClearChapterButtonList()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void OnClickChapterButton(string chapterName)
        {
            var info = AssetsManager.GetChapterInfoFromChapterName(chapterName);
            ConfigController.CurrentChapterName = chapterName;
            // resumePic = AssetsManager.LoadSprite(resumePic);
            chapterResume.text = info.Resume;
            chapterPic.sprite = AssetsManager.LoadSprite(info.ResumePic);
        }
    }
}