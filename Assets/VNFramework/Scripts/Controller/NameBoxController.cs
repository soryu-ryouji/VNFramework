using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VNFramework
{
    public class NameBoxController : MonoBehaviour
    {
        public TMP_Text nameTextBox;
        private string _currentName;

        private void Awake()
        {
            GameState.NameChanged += OnNameChanged;
        }

        private void OnDestroy()
        {
            GameState.NameChanged -= OnNameChanged;
        }

        private void OnNameChanged(Hashtable hash)
        {
            var action = (string)hash["action"];
            if (action == "append")
            {
                _currentName = (string)hash["name"];
            }
            else if (action == "clear")
            {
                _currentName = "";
            }

            ChangedDisplay();
        }

        private void ChangedDisplay()
        {
            if (_currentName == "")
            {
               gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                nameTextBox.text = _currentName;
            }
        }
    }
}
