using UnityEngine;
using UnityEngine.EventSystems;

public class BtnDeleteSelection : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        MapEditorManager mapEditor = GetComponentInParent<MapEditorManager>();
        if (mapEditor.SelectedObject == null)
            return;

        mapEditor.Grid.RemoveObject(mapEditor.SelectedObject);
        mapEditor.SelectedObject = null;
    }
}
