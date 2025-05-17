using UnityEngine;
using UnityEngine.EventSystems;

public class BtnExit : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<PauseMenu>().Hide();

        transform.parent.parent.GetComponentInChildren<BtnSound>()
            .GetComponent<Slidable>().Show();
        transform.parent.parent.GetComponentInChildren<BtnPause>()
            .GetComponent<Slidable>().Hide();

        UIAudio.Instance.PlayTick();
        MusicLoader.Instance.StopMusic();
        Time.timeScale = Player.Instance.TimeScale;
        Player.Instance.TimeScale = 1;
        Player.Instance.Destroy();
        Time.timeScale = 1;
        //transform.parent.parent.GetComponentInChildren<BtnPlay>().GetComponent<Slidable>().Show();
        //transform.parent.parent.GetComponentInChildren<MainMenuController>().Show();
    }
}
