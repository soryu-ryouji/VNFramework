using UnityEngine;
using VNFramework;

public class ViewController : MonoBehaviour, IController
{
    private static bool _viewControllerIsCreated = false;
    private GameObject _configViewPrefab;
    private GameObject _chapterViewPrefab;
    private GameObject _menuViewPrefab;
    private GameObject _backlogViewPrefab;
    private GameObject _performanceViewPrefab;

    private GameObject _configView;
    private GameObject _chapterView;
    private GameObject _menuView;
    private GameObject _backlogView;
    private GameObject _performanceView;
    private void Awake()
    {
        if (!_viewControllerIsCreated)
        {
            DontDestroyOnLoad(gameObject);
            _viewControllerIsCreated = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _configViewPrefab = Resources.Load<GameObject>("Prefabs/ConfigView");
        _chapterViewPrefab = Resources.Load<GameObject>("Prefabs/ChapterView");
        _menuViewPrefab = Resources.Load<GameObject>("Prefabs/MenuView");
        _backlogViewPrefab = Resources.Load<GameObject>("Prefabs/BacklogView");
        _performanceViewPrefab = Resources.Load<GameObject>("Prefabs/PerformanceView");

        this.RegisterEvent<ShowChapterViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _chapterView = Instantiate(_chapterViewPrefab, ui);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<ShowConfigViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _configView = Instantiate(_configViewPrefab, ui);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<ShowMenuViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _menuView = Instantiate(_menuViewPrefab, ui);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<ShowBacklogViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _backlogView = Instantiate(_backlogViewPrefab, ui);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<ShowPerformanceViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _performanceView = Instantiate(_performanceViewPrefab, ui);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<HideChapterViewEvent>(_ => Destroy(_chapterView)).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideConfigViewEvent>(_ => Destroy(_configView)).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideMenuViewEvent>(_ => Destroy(_menuView)).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideBacklogViewEvent>(_ => Destroy(_backlogView)).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HidePerformanceViewEvent>(_ => Destroy(_performanceView)).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    public IArchitecture GetArchitecture()
    {
        return VNFrameworkProj.Interface;
    }
}