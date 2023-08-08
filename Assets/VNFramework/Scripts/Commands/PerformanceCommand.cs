namespace VNFramework
{
    public class NextPerformanceCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<LoadNextPerformanceEvent>();
        }
    }
}