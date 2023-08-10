namespace VNFramework 
{ 
    class SaveSystemConfigCommand : AbstractCommand 
    { 
        protected override void OnExecute() 
        { 
            this.GetUtility<GameDataStorage>().SaveSystemConfig(); 
        } 
    } 
}