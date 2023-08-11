using UnityEngine;

namespace VNFramework
{
    class ProjectModel : AbstractModel
    {
        private string _titleViewLogo;
        private string _titleViewBgp;
        private string _titleViewBgm;
        private string _gameSaveViewBgp;
        private string _gameSaveViewBgm;
        private string _gameSaveViewGalleryItemPic;
        private string _gameSaveViewGalleryListPic;
        private string _backlogViewBgp;
        private string _backlogViewTextColor;
        private string _backlogViewBackButtonTextColor;
        private string _normDialogueBoxPic;
        private string _fullDialogueBoxPic;
        private string _nameBoxPic;
        private string _performanceViewMenuViewButtonImage;
        private string _performanceViewBacklogViewButtonImage;
        private string _performanceViewConfigViewButtonImage;
        private string _performanceViewSaveGameSaveViewButtonImage;

        public string TitleViewLogo { get => _titleViewLogo; set => _titleViewLogo = value; }
        public string TitleViewBgp { get => _titleViewBgp; set => _titleViewBgp = value; }
        public string TitleViewBgm { get => _titleViewBgm; set => _titleViewBgm = value; }
        public string GameSaveViewBgp { get => _gameSaveViewBgp; set => _gameSaveViewBgp = value; }
        public string GameSaveViewBgm { get => _gameSaveViewBgm; set => _gameSaveViewBgm = value; }
        public string GameSaveViewGalleryItemPic { get => _gameSaveViewGalleryItemPic; set => _gameSaveViewGalleryItemPic = value; }
        public string GameSaveViewGalleryListPic { get => _gameSaveViewGalleryListPic; set => _gameSaveViewGalleryListPic = value; }
        public string BacklogViewBgp { get => _backlogViewBgp; set => _backlogViewBgp = value; }
        public string BacklogViewTextColor { get => _backlogViewTextColor; set => _backlogViewTextColor = value; }
        public string NormDialogueBoxPic { get => _normDialogueBoxPic; set => _normDialogueBoxPic = value; }
        public string FullDialogueBoxPic { get => _fullDialogueBoxPic; set => _fullDialogueBoxPic = value; }
        public string NormNameBoxPic { get => _nameBoxPic; set => _nameBoxPic = value; }
        public string PerformanceViewMenuViewButtonPic { get => _performanceViewMenuViewButtonImage; set => _performanceViewMenuViewButtonImage = value; }
        public string PerformanceViewBacklogViewButtonPic { get => _performanceViewBacklogViewButtonImage; set => _performanceViewBacklogViewButtonImage = value; }
        public string PerformanceViewConfigViewButtonPic { get => _performanceViewConfigViewButtonImage; set => _performanceViewConfigViewButtonImage = value; }
        public string BacklogViewBackButtonTextColor { get => _backlogViewBackButtonTextColor; set => _backlogViewBackButtonTextColor = value; }
        public string PerformanceViewSaveGameSaveViewButtonPic { get => _performanceViewSaveGameSaveViewButtonImage; set => _performanceViewSaveGameSaveViewButtonImage = value; }

        protected override void OnInit()
        {
            this.GetUtility<GameDataStorage>().LoadProjectData();
        }
    }
}