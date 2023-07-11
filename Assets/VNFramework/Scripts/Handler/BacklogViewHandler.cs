using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class BacklogViewHandler : MonoBehaviour
    {
        public RectTransform backlogViewPos;
        public TMP_Text backlogTextBox;
        public Image backlogBgp;
        public ScrollRect scrollRect;

        private StringBuilder _currentHistorialDialogue = new();

        private void Awake()
        {
            backlogViewPos = gameObject.GetComponent<RectTransform>();
            backlogBgp = transform.Find("BacklogBgp").GetComponent<Image>();
            backlogTextBox = transform.Find("ScrollView/Text").GetComponent<TMP_Text>();
            scrollRect = transform.Find("ScrollView").GetComponent<ScrollRect>();

            GameState.UIChanged += OnUIChanged;
            GameState.DialogueChanged += OnDialogueChanged;
        }

        private void OnDestory()
        {
            GameState.UIChanged += OnUIChanged;
            GameState.DialogueChanged -= OnDialogueChanged;
        }

        private void OnUIChanged(Hashtable hashtable)
        {
            if ((string)hashtable["object"] != "backlog") return;

            var action = (string)hashtable["action"];

            if (action == "show") ShowBacklogView();
            else if (action == "hide") HideBacklogView();
        }

        private void ShowBacklogView()
        {
            backlogViewPos.anchoredPosition = new Vector2(0, 0);
            backlogTextBox.text = _currentHistorialDialogue.ToString();
            GameState.UIChanged(VNutils.Hash(
                "object", "dialogue",
                "action", "hide"
            ));
        }

        private void HideBacklogView()
        {
            backlogViewPos.anchoredPosition = new Vector3(0,1100,0);
            GameState.UIChanged(VNutils.Hash(
                "object", "dialogue",
                "action", "show"
            ));
        }

        private void OnDialogueChanged(Hashtable hashtable)
        {
            var action = (string)hashtable["action"];
            if ( action == "append")
            {
                _currentHistorialDialogue.Append(hashtable["dialogue"]);
            }
            else if (action == "newline")
            {
                _currentHistorialDialogue.Append("<br>");
            }
            else if (action == "clear")
            {
                _currentHistorialDialogue.Append("<br><br>");
            }
        }
    }
}