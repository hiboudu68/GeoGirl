using UnityEngine;
using UnityEngine.EventSystems;

public class BtnRestartVictory : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<VictoryMenu>()
            .Hide();

        Player.Play(FindAnyObjectByType<GameGrid>().PlayerPrefab);
        UIAudio.Instance.PlayTick();
    }
}
