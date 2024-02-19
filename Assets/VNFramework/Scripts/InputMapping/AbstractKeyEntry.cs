using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class AbstractKeyEntry : MonoBehaviour, ICanGetModel, ICanRegisterEvent
    {
        public TMP_Text label;

        public Image background;
        public Color selectedColor;
        public Color defaultColor;

        private InputMappingController controller;
        private AbstractKey key;

        private bool selected => controller != null && key == controller.CurrentAbstractKey;

        private void UpdateText()
        {
            label.text = this.GetModel<I18nModel>().__($"config.key.{Enum.GetName(typeof(AbstractKey),key)}");
        }

        public void Refresh()
        {
            background.color = selected ? selectedColor : defaultColor;
        }

        public void Init(InputMappingController controller, AbstractKey key)
        {
            this.controller = controller;
            this.key = key;
            UpdateText();
            Refresh();
        }

        private void OnEnable()
        {
            UpdateText();
            this.RegisterEvent<LanguageChangedEvent>(_ => UpdateText()).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        public void Select()
        {
            controller.CurrentAbstractKey = key;
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
