using UnityEngine;
using UnityEngine.EventSystems;

public class BtnStats : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UIAudio.Instance.PlayTick();
        transform.parent.parent.GetComponentInChildren<StatsMenu>().Show();

        transform.parent.parent.GetComponentInChildren<EditableLevelListMenu>()
            .GetComponent<UIToggler>().IsVisible = false;
        transform.parent.parent.GetComponentInChildren<LevelMenu>()
            .GetComponent<UIToggler>().IsVisible = false;
    }
}
