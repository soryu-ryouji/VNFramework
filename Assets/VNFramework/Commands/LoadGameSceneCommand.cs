namespace VNFramework
{
    class LoadGameSceneCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game");
        }
    }
}