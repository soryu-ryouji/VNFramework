using UnityEngine;
using UnityEngine.EventSystems;

namespace VNFramework
{
    public class ButtonSound : MonoBehaviour,IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            GameState.GmsChanged(VNutils.Hash(
                "object", AudioPlayer.Gms,
                "action", AudioAction.Play,
                "audio_name","click-button"
            ));
        }
    }
}
