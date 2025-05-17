using UnityEngine;
using UnityEngine.EventSystems;

public class BtnPlayLevel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        MapEditorManager mapEditor = GetComponentInParent<MapEditorManager>();
        mapEditor.transform.parent.GetComponentInChildren<InGameMenu>().Show();

        Player.Play(FindAnyObjectByType<GameGrid>().PlayerPrefab);
        Player.StopPlaying += OnStopPlaying;
    }
    private void OnStopPlaying()
    {
        Player.StopPlaying -= OnStopPlaying;
        GetComponentInParent<MapEditorManager>()
            .Show();
    }
}
