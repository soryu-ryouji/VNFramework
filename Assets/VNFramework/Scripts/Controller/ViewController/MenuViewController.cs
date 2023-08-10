using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class MenuViewController : MonoBehaviour, IController
    {
        private Button _titleBtn;
        private Button _chapterBtn;
        private Button _configViewBtn;
        private Button _exitBtn;
        private Button _backMenuBtn;


        private void Start()
        {
            _titleBtn = transform.Find("ButtonList/TitleButton").GetComponent<Button>();
            _chapterBtn = transform.Find("ButtonList/ChapterButton").GetComponent<Button>();
            _configViewBtn = transform.Find("ButtonList/ConfigButton").GetComponent<Button>();
            _exitBtn = transform.Find("ButtonList/ExitButton").GetComponent<Button>();
            _backMenuBtn = transform.Find("ButtonList/BackButton").GetComponent<Button>();

            _titleBtn.onClick.AddListener(this.SendCommand<LoadStartUpSceneCommand>);
            _chapterBtn.onClick.AddListener(this.SendCommand<ShowChapterViewCommand>);
            _configViewBtn.onClick.AddListener(this.SendCommand<ShowConfigViewCommand>);
            _backMenuBtn.onClick.AddListener(this.SendCommand<HideMenuViewCommand>);
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}