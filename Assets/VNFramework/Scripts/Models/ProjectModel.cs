namespace VNFramework
{
    class ProjectModel : AbstractModel
    {
        public string TitleViewLogo { get ;set; }
        public string TitleViewBgp { get; set; }
        public string TitleViewBgm { get; set; }
        public string GameSaveViewBgp { get; set; }
        public string GameSaveViewBgm { get; set; }
        public string GameSaveViewGalleryItemPic { get; set; }
        public string GameSaveViewGalleryListPic { get; set; }
        public string BacklogViewBgp { get; set; }
        public string BacklogViewTextColor { get; set; }
        public string NormDialogueBoxPic { get; set; }
        public string FullDialogueBoxPic { get; set; }
        public string NormNameBoxPic { get; set; }
        public string PerformanceViewMenuViewButtonPic { get; set; }
        public string PerformanceViewBacklogViewButtonPic { get; set; }
        public string PerformanceViewConfigViewButtonPic { get; set; }
        public string BacklogViewBackButtonTextColor { get; set; }
        public string PerformanceViewSaveGameSaveViewButtonPic { get; set; }

        public void InitModel()
        {
            this.GetUtility<GameDataStorage>().LoadProjectConfig();
        }

        protected override void OnInit()
        {
        }
    }
}