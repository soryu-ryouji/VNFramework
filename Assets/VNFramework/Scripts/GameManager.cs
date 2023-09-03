using System.Collections;
using UnityEditor;
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
            Debug.Log("<color=green>Init Game</color>");
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

            Instantiate(this.GetUtility<GameDataStorage>().LoadPrefab("AudioController"));
            var viewController = Instantiate(this.GetUtility<GameDataStorage>().LoadPrefab("ViewController")).GetComponent<ViewController>();
            viewController.InitViewController();
            viewController.ShowTitleView();
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

            Instantiate(this.GetUtility<GameDataStorage>().LoadPrefab("AudioController"));
            var viewController = Instantiate(this.GetUtility<GameDataStorage>().LoadPrefab("ViewController")).GetComponent<ViewController>();
            viewController.InitViewController();
            viewController.ShowTitleView();
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

            Instantiate(this.GetUtility<GameDataStorage>().LoadPrefab("AudioController"));
            var viewController = Instantiate(this.GetUtility<GameDataStorage>().LoadPrefab("ViewController")).GetComponent<ViewController>();
            viewController.InitViewController();
            viewController.ShowPerformanceView();
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

