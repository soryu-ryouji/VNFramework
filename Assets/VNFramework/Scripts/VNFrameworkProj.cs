namespace VNFramework
{
    class VNFrameworkProj : Architecture<VNFrameworkProj>
    {
        protected override void Init()
        {
            this.RegisterUtility(new GameDataStorage());
            this.RegisterModel(new GameSaveModel());
            this.RegisterModel(new ProjectModel());
            this.RegisterModel(new ConfigModel());
            this.RegisterModel(new PerformanceModel());
            this.RegisterModel(new ChapterModel());
            this.RegisterModel(new MermaidModel());
            this.RegisterModel(new DialogModel());
            this.RegisterModel(new I18nModel());
        }
    }
}
