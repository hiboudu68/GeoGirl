using UnityEngine;
using UnityEngine.EventSystems;

public class BtnPlayAI : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        MapEditorManager mapEditor = GetComponentInParent<MapEditorManager>();
        mapEditor.transform.parent.GetComponentInChildren<InGameMenu>().Show();

        Player.Play(FindAnyObjectByType<GameGrid>().AIPrefab);
        Player.StopPlaying += OnStopPlaying;
    }
    private void OnStopPlaying()
    {
        Debug.Log("Stop Playing");
        Player.StopPlaying -= OnStopPlaying;
        GetComponentInParent<MapEditorManager>()
            .Show();
    }
}
