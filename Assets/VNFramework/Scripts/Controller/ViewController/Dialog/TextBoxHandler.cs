using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class TextBoxHandler : MonoBehaviour
    {
        public TextBox textBox;
        public Image img;

        private void Awake()
        {
            textBox = transform.Find("TextBox").GetComponent<TextBox>();
            img = transform.Find("Bgp").GetComponent<Image>();

            if (textBox == null) Debug.LogError("TextBox is null");
            if (img == null) Debug.LogError("Image is null");
        }

        public void Hide()
        {
            textBox.gameObject.SetActive(false);
            img.gameObject.SetActive(false);
        }

        public void Show()
        {
            textBox.gameObject.SetActive(true);
            img.gameObject.SetActive(true);
        }
    }
}