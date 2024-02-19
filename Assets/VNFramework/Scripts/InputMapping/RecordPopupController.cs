using UnityEngine;

namespace VNFramework
{
    public class RecordPopupController : MonoBehaviour
    {
        [SerializeField] private RecordPopupLabel label;

        public InputMappingEntry entry
        {
            set => label.entry = value;
        }
    }
}
