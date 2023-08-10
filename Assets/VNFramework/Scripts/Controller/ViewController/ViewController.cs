using UnityEngine;
using UnityEngine.SceneManagement;
using VNFramework;

public class ViewController : MonoBehaviour, IController
{
    private GameObject _configViewPrefab;
    private GameObject _chapterViewPrefab;
    private GameObject _menuViewPrefab;
    private GameObject _backlogViewPrefab;
    private GameObject _performanceViewPrefab;
    private GameObject _gameSaveViewPrefab;
    private GameObject _chooseViewPrefab;

    private GameObject _configView;
    private GameObject _chapterView;
    private GameObject _menuView;
    private GameObject _backlogView;
    private GameObject _performanceView;
    private GameObject _gameSaveView;
    private GameObject _chooseView;

    private void Start()
    {
        var gameDataStorage = this.GetUtility<GameDataStorage>();
        _configViewPrefab = gameDataStorage.LoadPrefab("ConfigView");
        _chapterViewPrefab = gameDataStorage.LoadPrefab("ChapterView");
        _menuViewPrefab = gameDataStorage.LoadPrefab("MenuView");
        _backlogViewPrefab = gameDataStorage.LoadPrefab("BacklogView");
        _performanceViewPrefab = gameDataStorage.LoadPrefab("PerformanceView");
        _gameSaveViewPrefab = gameDataStorage.LoadPrefab("GameSaveView");
        _chooseViewPrefab = gameDataStorage.LoadPrefab("ChooseView");
    
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
        this.RegisterEvent<ShowSaveGameSaveViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _gameSaveView = Instantiate(_gameSaveViewPrefab, ui);
            _gameSaveView.GetComponent<GameSaveViewController>().viewType = GameSaveViewController.GameSaveViewType.Save;
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<ShowLoadGameSaveViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _gameSaveView = Instantiate(_gameSaveViewPrefab, ui);
            _gameSaveView.GetComponent<GameSaveViewController>().viewType = GameSaveViewController.GameSaveViewType.Load;
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<ShowChooseViewEvent>(_ =>
        {
            Transform ui = GameObject.Find("UI").transform;
            _chooseView = Instantiate(_chooseViewPrefab, ui);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.RegisterEvent<HideChapterViewEvent>(_ => Destroy(_chapterView)).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideConfigViewEvent>(_ => Destroy(_configView)).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideMenuViewEvent>(_ => Destroy(_menuView)).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideBacklogViewEvent>(_ => Destroy(_backlogView)).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HidePerformanceViewEvent>(_ => Destroy(_performanceView)).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideGameSaveViewEvent>(_ => Destroy(_gameSaveView)).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<HideChooseViewEvent>(_ => Destroy(_chooseView)).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    public IArchitecture GetArchitecture()
    {
        return VNFrameworkProj.Interface;
    }
}