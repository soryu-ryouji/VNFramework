using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VNFramework
{
    public class ButtonSound : MonoBehaviour, IPointerEnterHandler, ICanSendCommand
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            this.SendCommand(new PlayAudioCommand("click-button", AsmObj.gms));
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
