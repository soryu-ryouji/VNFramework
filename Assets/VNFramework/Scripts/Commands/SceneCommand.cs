using UnityEngine.SceneManagement;
namespace VNFramework
{
    class LoadStartUpSceneCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            SceneManager.LoadScene("StartUp");
        }
    }

    class LoadGameSceneCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            SceneManager.LoadScene("Game");
        }
    }
}