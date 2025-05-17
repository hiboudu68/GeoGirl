using UnityEngine;
using UnityEngine.EventSystems;

public class BtnCloseVictory : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<VictoryMenu>().Hide();
        Player.Instance.Destroy();
    }
}
