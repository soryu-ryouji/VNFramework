using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VNFramework
{
    public class I18nText : MonoBehaviour, ICanGetModel,ICanRegisterEvent
    {
        public string inflateTextKey;

        private Text text;
        private TMP_Text textPro;

        private void Awake()
        {
            text = GetComponent<Text>();
            textPro = GetComponent<TMP_Text>();
        }

        private void UpdateText()
        {
            string str = this.GetModel<I18nModel>().__(inflateTextKey);

            if (textPro != null)
            {
                textPro.text = str;
            }
            else
            {
                text.text = str;
            }
        }

        private void OnEnable()
        {
            UpdateText();
            this.RegisterEvent<LanguageChangedEvent>(_ => UpdateText()).UnRegisterWhenGameObjectDestroyed(this);;
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
