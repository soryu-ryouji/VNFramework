using System.Collections;
using UnityEngine;

namespace VNFramework
{
    public class MenuViewHandler : MonoBehaviour
    {
        RectTransform menuViewPos;

        private void Awake()
        {
            menuViewPos = gameObject.GetComponent<RectTransform>();
            GameState.UIChanged += OnUIChanged;
        }

        private void OnDestroy()
        {
            GameState.UIChanged -= OnUIChanged;
        }

        private void OnUIChanged(Hashtable hash)
        {
            if ((string)hash["object"] != "menu") return;

            var action = (string)hash["action"];

            if (action == "show") ShowMenuView();
            else if (action == "hide") HideMenuView();
        }


        private void ShowMenuView()
        {
            Debug.Log("Show Menu View");
            menuViewPos.anchoredPosition = new Vector2(0, 0);
            GameState.UIChanged(VNutils.Hash(
                "object", "dialogue",
                "action", "hide"
            ));
        }

        private void HideMenuView()
        {
            Debug.Log("Hide Menu View");
            menuViewPos.anchoredPosition = new Vector2(0, -1100);

            GameState.UIChanged(VNutils.Hash(
                "object", "dialogue",
                "action", "show"
            ));
        }
    }
}

