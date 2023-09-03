using UnityEngine;
using VNFramework;

public class ViewController : MonoBehaviour, IController
{
    private GameObject _titleViewPrefab;
    private GameObject _configViewPrefab;
    private GameObject _chapterViewPrefab;
    private GameObject _menuViewPrefab;
    private GameObject _backlogViewPrefab;
    private GameObject _performanceViewPrefab;
    private GameObject _gameSaveViewPrefab;
    private GameObject _chooseViewPrefab;

    private GameObject _titleView;
    private GameObject _configView;
    private GameObject _chapterView;
    private GameObject _menuView;
    private GameObject _backlogView;
    private GameObject _performanceView;
    private GameObject _gameSaveView;
    private GameObject _chooseView;

    public void InitViewController()
    {
        var gameDataStorage = this.GetUtility<GameDataStorage>();

        _titleViewPrefab = gameDataStorage.LoadPrefab("TitleView");
        _configViewPrefab = gameDataStorage.LoadPrefab("ConfigView");
        _chapterViewPrefab = gameDataStorage.LoadPrefab("ChapterView");
        _menuViewPrefab = gameDataStorage.LoadPrefab("MenuView");
        _backlogViewPrefab = gameDataStorage.LoadPrefab("BacklogView");
        _performanceViewPrefab = gameDataStorage.LoadPrefab("PerformanceView");
        _gameSaveViewPrefab = gameDataStorage.LoadPrefab("GameSaveView");
        _chooseViewPrefab = gameDataStorage.LoadPrefab("ChooseView");

        this.RegisterEvent<ShowTitleViewEvent>(_ => ShowTitleView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<ShowChapterViewEvent>(_ => ShowChapterView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<ShowConfigViewEvent>(_ => ShowConfigView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<ShowMenuViewEvent>(_ => ShowMenuView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<ShowBacklogViewEvent>(_ => ShowBacklogView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<ShowPerformanceViewEvent>(_ => ShowPerformanceView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<ShowSaveGameSaveViewEvent>(_ => ShowSaveGameSaveView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<ShowLoadGameSaveViewEvent>(_ => ShowLoadGameSaveView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<ShowChooseViewEvent>(_ => ShowChooseView()).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<HideTitleViewEvent>(_ => HideTitleView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideChapterViewEvent>(_ => HideChapterView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideConfigViewEvent>(_ => HideConfigView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideMenuViewEvent>(_ => HideMenuView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideBacklogViewEvent>(_ => HideBacklogView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HidePerformanceViewEvent>(_ => HidePerformanceView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideGameSaveViewEvent>(_ => HideGameSaveView()).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideChooseViewEvent>(_ => HideChooseView()).UnRegisterWhenGameObjectDestroyed(gameObject);

        Debug.Log("<color=green>Init View Controller</color>");
    }

    public void ShowTitleView()
    {
        Debug.Log("<color=green>View Controller: Show Title View</color>");
        Transform ui = GameObject.Find("UI").transform;
        _titleView = Instantiate(_titleViewPrefab, ui);
    }
    public void HideTitleView()
    {
        Debug.Log("<color=green>View Controller: Hide Title View</color>");
        Destroy(_titleView);
    }

    public void ShowChapterView()
    {
        Debug.Log("<color=green>View Controller: Show Chapter View</color>");
        Transform ui = GameObject.Find("UI").transform;
        _chapterView = Instantiate(_chapterViewPrefab, ui);
    }

    public void HideChapterView()
    {
        Debug.Log("<color=green>View Controller: Hide Chapter View</color>");
        Destroy(_chapterView);
    }

    public void ShowConfigView()
    {
        Debug.Log("<color=green>View Controller: Show Config View</color>");
        Transform ui = GameObject.Find("UI").transform;
        _configView = Instantiate(_configViewPrefab, ui);
    }

    public void HideConfigView()
    {
        Debug.Log("<color=green>View Controller: Hide Config View</color>");
        Destroy(_configView);
    }

    public void ShowMenuView()
    {
        Debug.Log("<color=green>View Controller: Show Menu View</color>");
        Transform ui = GameObject.Find("UI").transform;
        _menuView = Instantiate(_menuViewPrefab, ui);
    }

    public void HideMenuView()
    {
        Debug.Log("<color=green>View Controller: Hide Menu View</color>");
        Destroy(_menuView);
    }

    public void ShowBacklogView()
    {
        Debug.Log("<color=green>View Controller: Show Backlog View</color>");
        Transform ui = GameObject.Find("UI").transform;
        _backlogView = Instantiate(_backlogViewPrefab, ui);
    }

    public void HideBacklogView()
    {
        Debug.Log("<color=green>View Controller: Hide Backlog View</color>");
        Destroy(_backlogView);
    }

    public void ShowPerformanceView()
    {
        Debug.Log("<color=green>View Controller: Show Performance View</color>");
        Transform ui = GameObject.Find("UI").transform;
        _performanceView = Instantiate(_performanceViewPrefab, ui);
        _performanceView.GetComponent<PerformanceViewController>().InitPerformanceView();
        _performanceView.GetComponent<PerformanceController>().InitPerformanceController();
    }

    public void HidePerformanceView()
    {
        Debug.Log("<color=green>View Controller: Hide Performance View</color>");
        Destroy(_performanceView);
    }

    public void ShowSaveGameSaveView()
    {
        Debug.Log("<color=green>View Controller: Show Save Game Save View</color>");
        Transform ui = GameObject.Find("UI").transform;
        _gameSaveView = Instantiate(_gameSaveViewPrefab, ui);
        _gameSaveView.GetComponent<GameSaveViewController>().viewType = GameSaveViewController.GameSaveViewType.Save;
    }

    public void ShowLoadGameSaveView()
    {
        Debug.Log("<color=green>View Controller: Show Load Game Save View</color>");
        Transform ui = GameObject.Find("UI").transform;
        _gameSaveView = Instantiate(_gameSaveViewPrefab, ui);
        _gameSaveView.GetComponent<GameSaveViewController>().viewType = GameSaveViewController.GameSaveViewType.Load;
    }

    public void HideGameSaveView()
    {
        Debug.Log("<color=green>View Controller: Hide Game Save View</color>");
        Destroy(_gameSaveView);
    }

    public void ShowChooseView()
    {
        Debug.Log("<color=green>View Controller: Show Choose View</color>");
        Transform ui = GameObject.Find("UI").transform;
        _chooseView = Instantiate(_chooseViewPrefab, ui);
    }

    public void HideChooseView()
    {
        Debug.Log("<color=green>View Controller: Hide Choose View</color>");
        Destroy(_chooseView);
    }

    public IArchitecture GetArchitecture()
    {
        return VNFrameworkProj.Interface;
    }
}