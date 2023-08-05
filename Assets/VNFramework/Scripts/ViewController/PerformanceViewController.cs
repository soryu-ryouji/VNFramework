using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class PerformanceViewController : MonoBehaviour, IController
    {
        private Button _menuViewBtn;
        private Button _backlogViewBtn;
        private Button _configViewBtn;

        private Image _menuViewBtnImage;
        private Image _backlogViewButtonImage;
        private Image _configViewButtonImage;

        private void Start()
        {
            _menuViewBtn = transform.Find("ButtonList/MenuViewBtn").GetComponent<Button>();
            _backlogViewBtn = transform.Find("ButtonList/BacklogViewBtn").GetComponent<Button>();
            _configViewBtn = transform.Find("ButtonList/ConfigViewBtn").GetComponent<Button>();

            _menuViewBtnImage = transform.Find("ButtonList/MenuViewBtn").GetComponent<Image>();
            _backlogViewButtonImage = transform.Find("ButtonList/BacklogViewBtn").GetComponent<Image>();
            _configViewButtonImage = transform.Find("ButtonList/ConfigViewBtn").GetComponent<Image>();

            _menuViewBtn.onClick.AddListener(this.SendCommand<ShowMenuViewCommand>);
            _backlogViewBtn.onClick.AddListener(this.SendCommand<ShowBacklogViewCommand>);
            _configViewBtn.onClick.AddListener(this.SendCommand<ShowConfigViewCommand>);

            var projectModel = this.GetModel<ProjectModel>();
            _menuViewBtnImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.PerformanceViewMenuViewButtonPic);
            _backlogViewButtonImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.PerformanceViewBacklogViewButtonPic);
            _configViewButtonImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.PerformanceViewConfigViewButtonPic);
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}