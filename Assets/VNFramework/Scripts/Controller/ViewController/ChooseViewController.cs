using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VNFramework
{
    public class ChooseViewController : MonoBehaviour,IController
    {
        private GameObject _btnListContent;
        private VerticalLayoutGroup _btnListLayoutGroup;
        private GameObject _btnPrefab;

        private void Start()
        {
            _btnPrefab = this.GetUtility<GameDataStorage>().LoadPrefab("ChapterButton");
            _btnListContent = transform.Find("ButtonList/Content").gameObject;
            _btnListLayoutGroup = _btnListContent.GetComponent<VerticalLayoutGroup>();

            GenerateChooseList(this.GetModel<PerformanceModel>().ChooseList);
        }

        private void GenerateChooseList(List<(string mermaidName, string optionText)> chooseBtnList)
        {
            foreach (var btnData in chooseBtnList)
            {
                GameObject btnObj = Instantiate(_btnPrefab, _btnListContent.transform);
                
                Button button = btnObj.GetComponent<Button>();
                button.GetComponentInChildren<TMP_Text>().text = btnData.optionText;
                button.onClick.AddListener(() =>
                {
                    var performanceModel = this.GetModel<PerformanceModel>();
                    performanceModel.PerformingMermaidName = btnData.mermaidName;
                    performanceModel.PerformingIndex = 0;

                    _btnListLayoutGroup.CalculateLayoutInputVertical();
                    _btnListLayoutGroup.SetLayoutVertical();

                    this.SendCommand<HideChooseViewCommand>();
                    this.SendCommand<InitPerformanceCommand>();
                });
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}