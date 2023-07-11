using System.Collections;
using UnityEngine;
using VNFramework;

public class DialoguePanelViewHandler : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject nameBox;

    private bool _dialoguePanelActive = true;

    public bool DialoguePanelActive
    {
        get { return _dialoguePanelActive; }
    }

    private void Awake()
    {
        dialogueBox = transform.Find("DialogueBox").gameObject;
        nameBox = transform.Find("NameBox").gameObject;
        GameState.UIChanged += OnUIChanged;
    }

    private void OnDestroy()
    {
        GameState.UIChanged -= OnUIChanged;
    }

    private void OnUIChanged(Hashtable hash)
    {
        var obj = (string)hash["object"];
        if (obj != "dialogue") return;

        var action = (string)hash["action"];

        if (action == "toggle") ToggleDialoguePanel();
        else if (action == "hide") HideDialoguePanel();
        else if (action == "show") ShowDialoguePanel();
    }

    private void SetDialoguePanelActive(bool active)
    {
        dialogueBox.SetActive(active);
        nameBox.SetActive(active);
    }

    private void ToggleDialoguePanel()
    {
        _dialoguePanelActive = !_dialoguePanelActive;

        if (_dialoguePanelActive)
            ShowDialoguePanel();
        else
            HideDialoguePanel();

    }

    private void HideDialoguePanel()
    {
        Debug.Log("Hide Dialogue Panel");
        _dialoguePanelActive = false;

        if (GameState.IsDialogueTyping()) GameState.DialogueStop();

        SetDialoguePanelActive(_dialoguePanelActive);
    }

    private void ShowDialoguePanel()
    {
        _dialoguePanelActive = true;
        Debug.Log("Show Dialogue Panel");
        SetDialoguePanelActive(_dialoguePanelActive);
    }
}
