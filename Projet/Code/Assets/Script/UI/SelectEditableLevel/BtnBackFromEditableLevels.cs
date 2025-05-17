using UnityEngine;
using UnityEngine.EventSystems;

public class BtnBackFromEditableLevels : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EditableLevelListMenu levelMenu = GetComponentInParent<EditableLevelListMenu>();
        MainMenuController mainMenu = levelMenu.transform.parent.GetComponentInChildren<MainMenuController>();

        levelMenu.Hide();
        mainMenu.Show();
        UIAudio.Instance.PlayTick();
        FindAnyObjectByType<GameGrid>().Clear();
    }
}
