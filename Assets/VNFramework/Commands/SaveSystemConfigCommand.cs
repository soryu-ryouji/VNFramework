namespace VNFramework 
{ 
    class SaveSystemConfigCommand : AbstractCommand 
    { 
        protected override void OnExecute() 
        { 
            this.GetUtility<GameLog>().RunningLog("Save System Config Command"); 
            this.GetUtility<GameDataStorage>().SaveSystemConfig(); 
        } 
    } 
}