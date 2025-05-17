using UnityEngine;
using UnityEngine.EventSystems;

public class BtnConfirmPopup : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ConfirmPopup.Instance.Confirm();
        UIAudio.Instance.PlayTick();
    }
}
