namespace VNFramework
{
    struct ShowTitleViewEvent { }
    struct HideTitleViewEvent { }
    struct ShowLoadGameSaveViewEvent { }
    struct ShowSaveGameSaveViewEvent { }
    struct HideGameSaveViewEvent { }
    
    struct ShowBacklogViewEvent { }
    struct HideBacklogViewEvent { }

    struct ShowChapterViewEvent { }
    struct HideChapterViewEvent { }

    struct ShowConfigViewEvent { }
    struct HideConfigViewEvent { }

    struct ChangeNameEvent { }
    struct AppendDialogEvent { }
    struct ClearDialogEvent { }
    struct AppendNewLineToDialogEvent { }
    struct StopDialogueAnimEvent { }
    struct OpenFullDialogViewEvent { }
    struct OpenNormDialogViewEvent { }

    struct ShowDialogPanelEvent { }
    struct HideDialogPanelEvent { }
    struct ToggleDialogPanelEvent { }
    struct ShowPerformanceViewEvent { }
    struct HidePerformanceViewEvent { }

    struct ShowMenuViewEvent { }
    struct HideMenuViewEvent { }

    struct ShowChooseViewEvent { }
    struct HideChooseViewEvent { }

    struct InitViewControllerEvent {}
}