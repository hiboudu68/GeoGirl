using UnityEngine;
using UnityEngine.EventSystems;

public class BtnEdit : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.parent.GetComponentInChildren<EditableLevelListMenu>().Show();
        transform.parent.parent.GetComponentInChildren<LevelMenu>().HideBack(false);
        GetComponentInParent<MainMenuController>().Hide();
        UIAudio.Instance.PlayTick();
    }
}
