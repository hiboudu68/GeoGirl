using UnityEngine;
using UnityEngine.EventSystems;

public class BtnCancelAskFormName : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AskForName.Instance.Cancel();
        UIAudio.Instance.PlayTick();
    }
}
