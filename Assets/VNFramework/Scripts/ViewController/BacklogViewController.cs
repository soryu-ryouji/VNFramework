using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class BacklogViewController : MonoBehaviour, IController
    {
        private RectTransform _backlogViewPos;
        private TMP_Text _backlogTextBox;
        private Image _backlogBgp;
        private ScrollRect scrollRect;

        private Button _backBtn;

        private DialogueModel _dialogueModel;

        private void Awake()
        {
            _dialogueModel = this.GetModel<DialogueModel>();

            _backlogViewPos = gameObject.GetComponent<RectTransform>();
            _backlogBgp = transform.Find("ViewBgp").GetComponent<Image>();
            _backlogTextBox = transform.Find("ScrollView/Text").GetComponent<TMP_Text>();
            scrollRect = transform.Find("ScrollView").GetComponent<ScrollRect>();
            _backBtn = transform.Find("BackBtn").GetComponent<Button>();

            _backBtn.onClick.AddListener(this.SendCommand<HideBacklogViewCommand>);

            _backlogTextBox.text = string.Join("\n\n",_dialogueModel.GetHistoricalDialogues());
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}