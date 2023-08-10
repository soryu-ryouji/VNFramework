namespace VNFramework
{
    public class NextPerformanceCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<LoadNextPerformanceEvent>();
        }
    }

    public class InitPerformanceCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<InitPerformanceEvent>();
        }
    }
}