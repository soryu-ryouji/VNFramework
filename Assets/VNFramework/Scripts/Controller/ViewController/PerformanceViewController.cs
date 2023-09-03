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

        public void InitPerformanceView()
        {
            // 获取 Button
            _menuViewBtn = transform.Find("ButtonList/MenuViewBtn").GetComponent<Button>();
            _backlogViewBtn = transform.Find("ButtonList/BacklogViewBtn").GetComponent<Button>();
            _configViewBtn = transform.Find("ButtonList/ConfigViewBtn").GetComponent<Button>();
            _saveDataBtn = transform.Find("ButtonList/SaveDataBtn").GetComponent<Button>();

            // 添加 Button事件
            _menuViewBtn.onClick.AddListener(this.SendCommand<ShowMenuViewCommand>);
            _backlogViewBtn.onClick.AddListener(this.SendCommand<ShowBacklogViewCommand>);
            _configViewBtn.onClick.AddListener(this.SendCommand<ShowConfigViewCommand>);
            _saveDataBtn.onClick.AddListener(this.SendCommand<ShowSaveGameSaveViewCommand>);

            // 获取 Button Image
            _menuViewBtnImage = _menuViewBtn.GetComponent<Image>();
            _backlogViewBtnImage = _backlogViewBtn.GetComponent<Image>();
            _configViewBtnImage = _configViewBtn.GetComponent<Image>();
            _saveDataBtnImage = _saveDataBtn.GetComponent<Image>();

            // 添加 Button Image Sprite
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