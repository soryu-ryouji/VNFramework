using UnityEngine;
using VNFramework;

public class ViewController : MonoBehaviour, IController
{
    private static bool _viewControllerIsCreated = false;
    private GameObject _configViewPrefab;
    private GameObject _chapterViewPrefab;
    private GameObject _menuViewPrefab;
    private GameObject _backlogViewPrefab;

    private GameObject _configView;
    private GameObject _chapterView;
    private GameObject _menuView;
    private GameObject _backlogView;
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

        this.RegisterEvent<ShowChapterViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _chapterView = Instantiate(_chapterViewPrefab, ui);
        });
        this.RegisterEvent<ShowConfigViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _configView = Instantiate(_configViewPrefab, ui);
        });
        this.RegisterEvent<ShowMenuViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _menuView = Instantiate(_menuViewPrefab, ui);
        });
        this.RegisterEvent<ShowBacklogViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _backlogView = Instantiate(_backlogViewPrefab, ui);
        });

        this.RegisterEvent<HideChapterViewEvent>(_ => Destroy(_chapterView));
        this.RegisterEvent<HideConfigViewEvent>(_ => Destroy(_configView));
        this.RegisterEvent<HideMenuViewEvent>(_ => Destroy(_menuView));
        this.RegisterEvent<HideBacklogViewEvent>(_ => Destroy(_backlogView));
    }

    public IArchitecture GetArchitecture()
    {
        return VNFrameworkProj.Interface;
    }
}