using UnityEngine.SceneManagement;
namespace VNFramework
{
    class LoadStartUpSceneCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Load Start Up Scene");
            SceneManager.LoadScene("StartUp");
        }
    }

    class LoadGameSceneCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetUtility<GameLog>().RunningLog("Load Game Scene");
            SceneManager.LoadScene("Game");
        }
    }
}