namespace VNFramework
{
    class LoadStartUpSceneCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<LoadStartupSceneEvent>();
        }
    }

    class LoadGameSceneCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<LoadGameSceneEvent>();
        }
    }

    class ExitGameCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ExitGameEvent>();
        }
    }

    class SwitchToFullScreenCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<SwitchToFullScreenEvent>();
        }
    }

    class SwitchToWindowCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<SwitchToWindowEvent>();
        }
    }

    class SaveSystemConfigCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameDataStorage>().SaveSystemConfig();
        }
    }
}