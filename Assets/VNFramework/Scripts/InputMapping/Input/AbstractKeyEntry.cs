using System;
using TMPro;
using UnityEngine;

namespace VNFramework
{
    public class AbstractKeyEntry : MonoBehaviour, ICanGetModel, ICanRegisterEvent
    {
        public TMP_Text label;

        private InputMappingController controller;
        private AbstractKey key;

        // private bool selected => controller != null && key == controller.currentAbstractKey;

        private void UpdateText()
        {
            label.text = this.GetModel<I18nModel>().__($"config.key.{Enum.GetName(typeof(AbstractKey),key)}");
        }

        public void Refresh()
        {
            // background.color = selected ? selectedColor : defaultColor;
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
            // controller.currentAbstractKey = key;
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
