using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VNFramework
{
    [RequireComponent(typeof(TMP_Text))]
    public class RecordPopupLabel : MonoBehaviour
    {
        public InputMappingEntry entry;

        private TMP_Text label;

        private void Awake()
        {
            label = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            label.text = entry.key.ToString();
        }
    }
}
