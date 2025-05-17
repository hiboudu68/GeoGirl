using UnityEngine;
using UnityEngine.EventSystems;

public class BtnCloseStats : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<StatsMenu>().Hide();
        UIAudio.Instance.PlayTick();
    }
}
