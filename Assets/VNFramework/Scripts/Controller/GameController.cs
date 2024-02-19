using UnityEngine;

namespace VNFramework
{
    public class GameController : MonoBehaviour
    {
        public InputMapper InputMapper => this.FindComponent<InputMapper>();

        private T FindComponent<T>(string childPath = "") where T : MonoBehaviour
        {
            var go = gameObject;
            if (!string.IsNullOrEmpty(childPath))
            {
                go = transform.Find(childPath).gameObject;
            }

            var component = go.GetComponent<T>();
            return component;
        }
    }
}