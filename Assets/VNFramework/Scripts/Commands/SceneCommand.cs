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

    class ExitGameCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }
}