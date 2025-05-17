using UnityEngine;
using UnityEngine.EventSystems;

public class BtnSkins : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        FindAnyObjectByType<GameGrid>().HideStartSprite();
        GetComponentInParent<MainMenuController>().Hide();
        transform.parent.parent.GetComponentInChildren<EditableLevelListMenu>().HideBack(false);
        transform.parent.parent.GetComponentInChildren<LevelMenu>().HideBack(false);

        transform.parent.GetComponentInChildren<BtnPlay>()
            .GetComponent<Slidable>().Hide();

        transform.parent.parent.GetComponentInChildren<EditableLevelListMenu>()
            .GetComponent<UIToggler>().IsVisible = false;

        transform.parent.parent.GetComponentInChildren<SkinsMenu>()
            .GetComponent<UIToggler>().IsVisible = true;
    }
}
