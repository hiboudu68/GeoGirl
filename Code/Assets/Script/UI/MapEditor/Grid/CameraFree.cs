using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFree : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    private bool isDown = false;
    private Vector2 startDownPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isDown = true;
            GetComponentInParent<MapEditorManager>().DisableRaycast();
            startDownPosition = eventData.position;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (isDown)
        {
            Vector3 diff = eventData.position - startDownPosition;
            float worldPerPx = 2f * Camera.main.orthographicSize / Screen.height;
            Camera.main.transform.position -= diff * worldPerPx;
            startDownPosition = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isDown = false;
            GetComponentInParent<MapEditorManager>().EnableRaycast();
        }
    }
}
