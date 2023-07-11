using UnityEngine;
using UnityEngine.EventSystems;

namespace VNFramework
{
    public class ButtonSound : MonoBehaviour,IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            GameState.AudioChanged(VNutils.Hash(
                "object","gms",
                "action","play",
                "audio_name","click-button"
            ));
        }
    }
}
