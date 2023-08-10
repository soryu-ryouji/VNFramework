using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class PerformanceViewController : MonoBehaviour, IController
    {
        private Button _menuViewBtn;
        private Button _backlogViewBtn;
        private Button _configViewBtn;
        private Button _saveDataBtn;

        private Image _menuViewBtnImage;
        private Image _backlogViewBtnImage;
        private Image _configViewBtnImage;
        private Image _saveDataBtnImage;

        private void Start()
        {
            _menuViewBtn = transform.Find("ButtonList/MenuViewBtn").GetComponent<Button>();
            _backlogViewBtn = transform.Find("ButtonList/BacklogViewBtn").GetComponent<Button>();
            _configViewBtn = transform.Find("ButtonList/ConfigViewBtn").GetComponent<Button>();
            _saveDataBtn = transform.Find("ButtonList/SaveDataBtn").GetComponent<Button>();

            _menuViewBtnImage = transform.Find("ButtonList/MenuViewBtn").GetComponent<Image>();
            _backlogViewBtnImage = transform.Find("ButtonList/BacklogViewBtn").GetComponent<Image>();
            _configViewBtnImage = transform.Find("ButtonList/ConfigViewBtn").GetComponent<Image>();
            _saveDataBtnImage = transform.Find("ButtonList/SaveDataBtn").GetComponent<Image>();

            _menuViewBtn.onClick.AddListener(this.SendCommand<ShowMenuViewCommand>);
            _backlogViewBtn.onClick.AddListener(this.SendCommand<ShowBacklogViewCommand>);
            _configViewBtn.onClick.AddListener(this.SendCommand<ShowConfigViewCommand>);
            _saveDataBtn.onClick.AddListener(this.SendCommand<ShowSaveGameSaveViewCommand>);

            var projectModel = this.GetModel<ProjectModel>();
            _menuViewBtnImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.PerformanceViewMenuViewButtonPic);
            _backlogViewBtnImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.PerformanceViewBacklogViewButtonPic);
            _configViewBtnImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.PerformanceViewConfigViewButtonPic);
            _saveDataBtnImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.PerformanceViewSaveGameSaveViewButtonPic);
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}