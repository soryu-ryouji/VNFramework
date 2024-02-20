using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VNFramework
{
    public class GameManager : MonoBehaviour, IController
    {
        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                    if (instance == null)
                    {
                        GameObject managerObject = new GameObject("GameManager");
                        instance = managerObject.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitGame();
        }

        public void InitGame()
        {
            this.RegisterEvent<LoadGameSceneEvent>(_ => LoadGameScene());
            this.RegisterEvent<LoadStartupSceneEvent>(_ => LoadStartupScene());
            this.RegisterEvent<ExitGameEvent>(_ => ExitGame());
            this.RegisterEvent<SwitchToFullScreenEvent>(_ => SwitchToFullScreenMode());
            this.RegisterEvent<SwitchToWindowEvent>(_ => SwitchToWindowMode());

            this.GetUtility<GameDataStorage>().LoadAllRes();

            this.GetModel<ConfigModel>().InitModel();
            this.GetModel<GameSaveModel>().InitModel();
            this.GetModel<ProjectModel>().InitModel();
            this.GetModel<ChapterModel>().InitModel();
            this.GetModel<MermaidModel>().InitModel();
            this.GetModel<I18nModel>().InitModel();
            this.GetComponent<AudioController>().InitController();

            this.GetComponent<InputMapper>().Init();

            ViewController.Instance.ShowTitleView();

            Debug.Log("<color=green>Init Game Success</color>");
        }

        public void LoadStartupScene()
        {
            StartCoroutine(LoadStartupSceneCoroutine());
        }

        private IEnumerator LoadStartupSceneCoroutine()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StartUp", LoadSceneMode.Single);

            // 等待场景加载完成
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            ViewController.Instance.ShowTitleView();

            Debug.Log("<color=green>Load Startup Scene Success</color>");
        }

        public void LoadGameScene()
        {
            StartCoroutine(LoadGameSceneCoroutine());
        }

        private IEnumerator LoadGameSceneCoroutine()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            ViewController.Instance.ShowPerformanceView();

            Debug.Log("<color=green>Load Game Scene Success</color>");
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        public void SwitchToFullScreenMode()
        {
            Debug.Log("<color=green>Switch to full screen mode</color>");
            Screen.fullScreen = true;
        }

        public void SwitchToWindowMode()
        {
            Debug.Log("<color=green>Switch to window mode</color>");
            Screen.fullScreen = false;
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}

