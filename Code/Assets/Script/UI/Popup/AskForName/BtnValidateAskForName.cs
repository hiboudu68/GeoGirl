using UnityEngine;
using UnityEngine.EventSystems;

public class BtnValidateAskForName : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AskForName.Instance.Validate();
        UIAudio.Instance.PlayTick();
    }
}
