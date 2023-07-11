using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VNFramework
{
    public class DialogueButtonHandler : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public RectTransform buttonListPos;

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowButtonList();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideButtonList();
        }

        private void HideButtonList()
        {
            iTween.MoveTo(buttonListPos.gameObject ,iTween.Hash(
                "y",-150,
                "time",0.5f,
                "easetype",iTween.EaseType.easeInCubic
            ));
        }

        private void ShowButtonList()
        {
            iTween.MoveTo(buttonListPos.gameObject ,iTween.Hash(
                "y",60,
                "time",0.5f,
                "easetype",iTween.EaseType.easeInCubic
            ));
        }
    }
}