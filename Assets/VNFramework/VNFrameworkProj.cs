namespace VNFramework
{
    class VNFrameworkProj : Architecture<VNFrameworkProj>
    {
        protected override void Init()
        {
            this.RegisterUtility(new GameDataStorage());
            
            this.RegisterModel(new ProjectModel());
            this.RegisterModel(new ConfigModel());
            this.RegisterModel(new PerformingModel());
            this.RegisterModel(new ChapterModel());
            this.RegisterModel(new DialogueModel());
        }
    }
}