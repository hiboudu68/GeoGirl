using UnityEngine;
using UnityEngine.EventSystems;

public class BtnRestart : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<PauseMenu>()
            .Hide();

        Player.Instance.Restart();
        UIAudio.Instance.PlayTick();
    }
}
