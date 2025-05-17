using UnityEngine;
using UnityEngine.EventSystems;

public class BtnRotateObject : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        MapEditorManager mapEditor = GetComponentInParent<MapEditorManager>();
        BaseObject obj = mapEditor.SelectedObject;
        if (mapEditor.SelectedObject != null)
            mapEditor.SelectedObject.Rotate();
        else mapEditor.Rotate = (byte)((mapEditor.Rotate + 1) % 4);
    }
}
