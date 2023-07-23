using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class PerformanceViewController : MonoBehaviour, IController
    {
        private Button _menuViewBtn;
        private Button _backlogViewBtn;
        private Button _configViewBtn;

        private void Start()
        {
            _menuViewBtn = transform.Find("ButtonList/MenuViewBtn").GetComponent<Button>();
            _backlogViewBtn = transform.Find("ButtonList/BacklogViewBtn").GetComponent<Button>();
            _configViewBtn = transform.Find("ButtonList/ConfigViewBtn").GetComponent<Button>();

            _menuViewBtn.onClick.AddListener(this.SendCommand<ShowMenuViewCommand>);
            _backlogViewBtn.onClick.AddListener(this.SendCommand<ShowBacklogViewCommand>);
            _configViewBtn.onClick.AddListener(this.SendCommand<ShowConfigViewCommand>);
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}