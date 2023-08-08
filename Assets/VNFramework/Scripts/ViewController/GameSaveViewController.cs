using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class GameSaveViewController : MonoBehaviour, IController
    {
        private GameSaveViewGalleryItemHandle[] _gameSaveItems = new GameSaveViewGalleryItemHandle[6];
        private Button[] _galleryButtons = new Button[10];

        private Button _backButton;

        private int _currentPage = 0;

        private GameSaveModel _gameSaveModel;

        private void Awake()
        {
            _gameSaveItems[0] = transform.Find("Gallery/Item00").GetComponent<GameSaveViewGalleryItemHandle>();
            _gameSaveItems[1] = transform.Find("Gallery/Item01").GetComponent<GameSaveViewGalleryItemHandle>();
            _gameSaveItems[2] = transform.Find("Gallery/Item02").GetComponent<GameSaveViewGalleryItemHandle>();
            _gameSaveItems[3] = transform.Find("Gallery/Item03").GetComponent<GameSaveViewGalleryItemHandle>();
            _gameSaveItems[4] = transform.Find("Gallery/Item04").GetComponent<GameSaveViewGalleryItemHandle>();
            _gameSaveItems[5] = transform.Find("Gallery/Item05").GetComponent<GameSaveViewGalleryItemHandle>();

            _galleryButtons[0] = transform.Find("GalleryList/Button00").GetComponent<Button>();
            _galleryButtons[1] = transform.Find("GalleryList/Button01").GetComponent<Button>();
            _galleryButtons[2] = transform.Find("GalleryList/Button02").GetComponent<Button>();
            _galleryButtons[3] = transform.Find("GalleryList/Button03").GetComponent<Button>();
            _galleryButtons[4] = transform.Find("GalleryList/Button04").GetComponent<Button>();
            _galleryButtons[5] = transform.Find("GalleryList/Button05").GetComponent<Button>();
            _galleryButtons[6] = transform.Find("GalleryList/Button06").GetComponent<Button>();
            _galleryButtons[7] = transform.Find("GalleryList/Button07").GetComponent<Button>();
            _galleryButtons[8] = transform.Find("GalleryList/Button08").GetComponent<Button>();
            _galleryButtons[9] = transform.Find("GalleryList/Button09").GetComponent<Button>();
            
            _galleryButtons[0].onClick.AddListener(() => { _currentPage = 0; UpdateView(); });
            _galleryButtons[1].onClick.AddListener(() => { _currentPage = 1; UpdateView(); });
            _galleryButtons[2].onClick.AddListener(() => { _currentPage = 2; UpdateView(); });
            _galleryButtons[3].onClick.AddListener(() => { _currentPage = 3; UpdateView(); });
            _galleryButtons[4].onClick.AddListener(() => { _currentPage = 4; UpdateView(); });
            _galleryButtons[5].onClick.AddListener(() => { _currentPage = 5; UpdateView(); });
            _galleryButtons[6].onClick.AddListener(() => { _currentPage = 6; UpdateView(); });
            _galleryButtons[7].onClick.AddListener(() => { _currentPage = 7; UpdateView(); });
            _galleryButtons[8].onClick.AddListener(() => { _currentPage = 8; UpdateView(); });
            _galleryButtons[9].onClick.AddListener(() => { _currentPage = 9; UpdateView(); });

            _backButton = transform.Find("BackButton").GetComponent<Button>();
            _backButton.onClick.AddListener(() => { this.SendCommand<HideGameSaveViewCommand>(); });
        }

        private void Start()
        {
            _gameSaveModel = this.GetModel<GameSaveModel>();
            UpdateView();
        }

        private void UpdateView()
        {
            var gameSaves = _gameSaveModel.GetGameSaves();
            this.GetUtility<GameLog>().RunningLog("Game Save View Current Page : " + _currentPage);
            for (int i = 0; i < _gameSaveItems.Length; i++)
            {
                int index = i + _currentPage * 6;
                _gameSaveItems[i].SetGameSaveItem(gameSaves[index]);
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}