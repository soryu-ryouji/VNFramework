using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class BacklogViewController : MonoBehaviour, IController
    {
        private TMP_Text _backlogTextBox;
        private Image _backlogBgp;
        private Button _backBtn;
        private TMP_Text _backBtnText;

        private DialogueModel _dialogueModel;

        private void Start()
        {
            _dialogueModel = this.GetModel<DialogueModel>();
            var projectModel = this.GetModel<ProjectModel>();

            // 获取 View 组件
            _backlogBgp = transform.Find("ViewBgp").GetComponent<Image>();
            _backlogTextBox = transform.Find("ScrollView/Text").GetComponent<TMP_Text>();
            _backBtn = transform.Find("BackBtn").GetComponent<Button>();
            _backBtnText = _backBtn.transform.Find("Text").GetComponent<TMP_Text>();

            // 对 BacklogView 外观进行初始化
            _backlogTextBox.text = string.Join("\n\n", _dialogueModel.GetHistoricalDialogues());
            _backlogBgp.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.BacklogViewBgp);
            _backlogTextBox.color = VNutils.StrToColor(projectModel.BacklogViewTextColor);
            _backBtnText.color = VNutils.StrToColor(projectModel.BacklogViewBackButtonTextColor);

            _backBtn.onClick.AddListener(this.SendCommand<HideBacklogViewCommand>);
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}