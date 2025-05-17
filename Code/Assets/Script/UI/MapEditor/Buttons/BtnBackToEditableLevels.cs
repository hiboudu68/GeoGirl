using UnityEngine;
using UnityEngine.EventSystems;

public class BtnBackToEditableLevels : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        MapEditorManager mapEditor = GetComponentInParent<MapEditorManager>();
        mapEditor.Hide();

        mapEditor.transform.parent.GetComponentInChildren<EditableLevelListMenu>(true).Show();
        UIAudio.Instance.PlayTick();
    }
}
