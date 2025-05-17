using UnityEngine;
using UnityEngine.EventSystems;

public class BtnCancelPopup : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ConfirmPopup.Instance.Cancel();
        UIAudio.Instance.PlayTick();
    }
}
