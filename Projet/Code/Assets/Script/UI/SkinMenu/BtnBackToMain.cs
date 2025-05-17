using UnityEngine;
using UnityEngine.EventSystems;

public class BtnBackToMain : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<UIToggler>().IsVisible = false;
        MainMenuController mainMenu = transform.parent.parent.GetComponentInChildren<MainMenuController>();
        mainMenu.Show();
        mainMenu.GetComponentInChildren<BtnPlay>()
            .GetComponent<Slidable>()
            .Show();
        UIAudio.Instance.PlayTick();
    }
}
