using UnityEngine;
using UnityEngine.EventSystems;

public class BtnEditLevel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EditableLevelListMenu editableLevelListMenu = GetComponentInParent<EditableLevelListMenu>();
        LevelSlider levelSlider = editableLevelListMenu.GetComponentInChildren<LevelSlider>();
        editableLevelListMenu.StartEditLevel(levelSlider.GetLevel());
    }
}
