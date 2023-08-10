using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class GameSaveViewController : MonoBehaviour, IController
    {
        public enum GameSaveViewType
        {
            Save,
            Load
        }

        public GameSaveViewType viewType;
        private GameSaveViewGalleryItemHandle[] _galleryBtns = new GameSaveViewGalleryItemHandle[6];
        private Button[] _pageBtns = new Button[10];

        private Button _backBtn;

        private int _currentPage = 0;

        private GameSaveModel _gameSaveModel;

        private void Start()
        {
            _galleryBtns[0] = transform.Find("Gallery/Item00").GetComponent<GameSaveViewGalleryItemHandle>();
            _galleryBtns[1] = transform.Find("Gallery/Item01").GetComponent<GameSaveViewGalleryItemHandle>();
            _galleryBtns[2] = transform.Find("Gallery/Item02").GetComponent<GameSaveViewGalleryItemHandle>();
            _galleryBtns[3] = transform.Find("Gallery/Item03").GetComponent<GameSaveViewGalleryItemHandle>();
            _galleryBtns[4] = transform.Find("Gallery/Item04").GetComponent<GameSaveViewGalleryItemHandle>();
            _galleryBtns[5] = transform.Find("Gallery/Item05").GetComponent<GameSaveViewGalleryItemHandle>();

            _galleryBtns[0].btn.onClick.AddListener(() => { SaveOrLoadGameSave(_galleryBtns[0].Index); UpdateView(); });
            _galleryBtns[1].btn.onClick.AddListener(() => { SaveOrLoadGameSave(_galleryBtns[1].Index); UpdateView(); });
            _galleryBtns[2].btn.onClick.AddListener(() => { SaveOrLoadGameSave(_galleryBtns[2].Index); UpdateView(); });
            _galleryBtns[3].btn.onClick.AddListener(() => { SaveOrLoadGameSave(_galleryBtns[3].Index); UpdateView(); });
            _galleryBtns[4].btn.onClick.AddListener(() => { SaveOrLoadGameSave(_galleryBtns[4].Index); UpdateView(); });
            _galleryBtns[5].btn.onClick.AddListener(() => { SaveOrLoadGameSave(_galleryBtns[5].Index); UpdateView(); });

            _pageBtns[0] = transform.Find("GalleryList/Button00").GetComponent<Button>();
            _pageBtns[1] = transform.Find("GalleryList/Button01").GetComponent<Button>();
            _pageBtns[2] = transform.Find("GalleryList/Button02").GetComponent<Button>();
            _pageBtns[3] = transform.Find("GalleryList/Button03").GetComponent<Button>();
            _pageBtns[4] = transform.Find("GalleryList/Button04").GetComponent<Button>();
            _pageBtns[5] = transform.Find("GalleryList/Button05").GetComponent<Button>();
            _pageBtns[6] = transform.Find("GalleryList/Button06").GetComponent<Button>();
            _pageBtns[7] = transform.Find("GalleryList/Button07").GetComponent<Button>();
            _pageBtns[8] = transform.Find("GalleryList/Button08").GetComponent<Button>();
            _pageBtns[9] = transform.Find("GalleryList/Button09").GetComponent<Button>();

            _pageBtns[0].onClick.AddListener(() => { _currentPage = 0; UpdateGalleryBtnIndex(); UpdateView(); });
            _pageBtns[1].onClick.AddListener(() => { _currentPage = 1; UpdateGalleryBtnIndex(); UpdateView(); });
            _pageBtns[2].onClick.AddListener(() => { _currentPage = 2; UpdateGalleryBtnIndex(); UpdateView(); });
            _pageBtns[3].onClick.AddListener(() => { _currentPage = 3; UpdateGalleryBtnIndex(); UpdateView(); });
            _pageBtns[4].onClick.AddListener(() => { _currentPage = 4; UpdateGalleryBtnIndex(); UpdateView(); });
            _pageBtns[5].onClick.AddListener(() => { _currentPage = 5; UpdateGalleryBtnIndex(); UpdateView(); });
            _pageBtns[6].onClick.AddListener(() => { _currentPage = 6; UpdateGalleryBtnIndex(); UpdateView(); });
            _pageBtns[7].onClick.AddListener(() => { _currentPage = 7; UpdateGalleryBtnIndex(); UpdateView(); });
            _pageBtns[8].onClick.AddListener(() => { _currentPage = 8; UpdateGalleryBtnIndex(); UpdateView(); });
            _pageBtns[9].onClick.AddListener(() => { _currentPage = 9; UpdateGalleryBtnIndex(); UpdateView(); });

            _backBtn = transform.Find("BackButton").GetComponent<Button>();
            _backBtn.onClick.AddListener(() => this.SendCommand<HideGameSaveViewCommand>());
            
            _gameSaveModel = this.GetModel<GameSaveModel>();
            UpdateView();
        }

        private void UpdateView()
        {
            var gameSaves = _gameSaveModel.GameSaves;
            Debug.Log("Game Save View Current Page : " + _currentPage);
            for (int i = 0; i < _galleryBtns.Length; i++)
            {
                int index = i + _currentPage * 6;
                _galleryBtns[i].Index = index;
                _galleryBtns[i].SetGameSaveItem(gameSaves[index]);
            }
        }

        private void UpdateGalleryBtnIndex()
        {
            for (int i = 0; i < _galleryBtns.Length; i++)
            {
                _galleryBtns[i].Index = i + _currentPage * 6;
            }
        }

        private void SaveOrLoadGameSave(int index)
        {
            if (viewType == GameSaveViewType.Save) SaveGameSave(index);
            else if (viewType == GameSaveViewType.Load) LoadGameSave(index);
        }

        private void SaveGameSave(int index)
        {
            var performanceModel = this.GetModel<PerformanceModel>();
            var currentTime = System.DateTime.Now;
            var gameSave = new GameSave()
            {
                SaveDate = currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                MermaidNode = performanceModel.PerformingMermaidName,
                VNScriptIndex = performanceModel.PerformingIndex,
                ResumePic = performanceModel.BgpName,
                ResumeText = performanceModel.PerformingDialogue
            };

            _gameSaveModel.SetGameSave(index, gameSave);
        }

        private void LoadGameSave(int index)
        {
            // 若点击的是空对象，则直接忽略
            var gameSave = _gameSaveModel.GetGameSave(index);
            if (string.IsNullOrWhiteSpace(gameSave.SaveDate)) return;
            
            var performanceModel = this.GetModel<PerformanceModel>();
            performanceModel.PerformingMermaidName = gameSave.MermaidNode;
            performanceModel.PerformingIndex = gameSave.VNScriptIndex;
            this.SendCommand<LoadGameSceneCommand>();
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}