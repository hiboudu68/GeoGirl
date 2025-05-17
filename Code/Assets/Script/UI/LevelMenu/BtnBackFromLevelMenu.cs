using UnityEngine;
using UnityEngine.EventSystems;

public class BtnBack : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        LevelMenu levelMenu = GetComponentInParent<LevelMenu>();
        MainMenuController mainMenu = levelMenu.transform.parent.GetComponentInChildren<MainMenuController>();

        levelMenu.Hide();
        mainMenu.Show();
        UIAudio.Instance.PlayTick();
    }
}
