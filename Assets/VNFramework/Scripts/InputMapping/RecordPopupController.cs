using UnityEngine;

namespace VNFramework
{
    public class RecordPopupController
    {
        [SerializeField] private RecordPopupLabel label;

        public InputMappingEntry entry
        {
            set => label.entry = value;
        }
    }
}
