using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class PerformanceViewController : MonoBehaviour, IController
    {
        private Button _menuViewBtn;
        private Button _backlogViewBtn;
        private Button _hideDialogueBtn;

        private void Start()
        {
            _menuViewBtn = transform.Find("ButtonArea/ButtonList/MenuViewBtn").GetComponent<Button>();
            _backlogViewBtn = transform.Find("ButtonArea/ButtonList/BacklogViewBtn").GetComponent<Button>();
            _hideDialogueBtn = transform.Find("ButtonArea/ButtonList/HideDialogueBtn").GetComponent<Button>();

            _menuViewBtn.onClick.AddListener(this.SendCommand<ShowMenuViewCommand>);
            _backlogViewBtn.onClick.AddListener(this.SendCommand<ShowBacklogViewCommand>);
            _hideDialogueBtn.onClick.AddListener(this.SendCommand<HideDialoguePanelCommand>);
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}