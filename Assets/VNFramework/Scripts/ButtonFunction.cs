using UnityEngine;
using UnityEngine.SceneManagement;

namespace VNFramework
{
    public class ButtonFunction : MonoBehaviour
    {
        public void ShowConfigView()
        {
            GameState.UIChanged(VNutils.Hash(
                "object", "config",
                "action", "show"
            ));
        }

        public void HideConfigView()
        {
            GameState.UIChanged(VNutils.Hash(
                "object", "config",
                "action", "hide"
            ));
            ConfigController.SaveConfigToFile();
        }

        public void ShowBacklogView()
        {
            GameState.UIChanged(VNutils.Hash(
                "object", "backlog",
                "action", "show"
            ));
        }

        public void HideBacklogView()
        {
            GameState.UIChanged(VNutils.Hash(
                "object", "backlog",
                "action", "hide"
            ));
        }

        public void HideDialoguePanel()
        {
            GameState.UIChanged(VNutils.Hash(
                "object", "dialogue",
                "action", "hide"
            ));
        }

        public void ShowMenuView()
        {
            GameState.UIChanged(VNutils.Hash(
                "object", "menu",
                "action", "show"
            ));
        }

        public void HideMenuView()
        {
            GameState.UIChanged(VNutils.Hash(
                "object", "menu",
                "action", "hide"
            ));
        }

        public void ShowChapterView()
        {
            GameState.UIChanged(VNutils.Hash(
                "object", "chapter",
                "action", "show"
            ));
        }
        public void HideChapterView()
        {
            GameState.UIChanged(VNutils.Hash(
                "object", "chapter",
                "action", "hide"
            ));
        }

        public void StartGame()
        {
            var chapterInfo = AssetsManager.LoadChapterInfo();
            ConfigController.CurrentChapterName = chapterInfo[0].ChapterName;
            SceneManager.LoadScene("Game");
        }

        public void LoadStartView()
        {
            SceneManager.LoadScene("StartUp");
        }

        public void LoadChapter()
        {
            SceneManager.LoadScene("Game");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}
