using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnSound : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Sprite soundOnSprite;

    void Start()
    {
        image = GetComponent<Image>();
        AudioListener.pause = !PlayerSkinPreferences.Volume;
        Player.StartPlaying += OnStartPlaying;
        Player.StopPlaying += OnStopPlaying;
        UpdatePause();
    }
    private void OnStartPlaying()
    {
        GetComponent<Slidable>().Hide();
    }
    private void OnStopPlaying()
    {
        GetComponent<Slidable>().Show();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioListener.pause = !AudioListener.pause;
        PlayerSkinPreferences.Volume = !AudioListener.pause;
        UpdatePause();
    }
    private void UpdatePause()
    {
        image.sprite = AudioListener.pause ? soundOffSprite : soundOnSprite;
        UIAudio.Instance.PlayTick();
    }
}
