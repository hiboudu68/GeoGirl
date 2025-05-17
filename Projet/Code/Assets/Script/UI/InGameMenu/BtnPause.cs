using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnPause : MonoBehaviour, IPointerClickHandler
{
    private PauseMenu pauseMenu;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite resumeSprite;

    void Start()
    {
        pauseMenu = transform.parent.parent.GetComponentInChildren<PauseMenu>();

        UIToggler pauseToggler = pauseMenu.GetComponent<UIToggler>();
        pauseToggler.IsVisible = false;
        pauseToggler.OnVisibilityChanged += OnPauseMenuVisibilityChanged;

        Player.StartPlaying += OnStartPlaying;
        Player.StopPlaying += OnStopPlaying;
    }
    private void OnStopPlaying()
    {
        GetComponent<Slidable>().Hide();
    }
    private void OnStartPlaying()
    {
        OnPauseMenuVisibilityChanged(false);
        GetComponent<Slidable>().Show();
    }
    private void OnPauseMenuVisibilityChanged(bool isVisible)
    {
        if (isVisible)
        {
            GetComponent<Image>().sprite = resumeSprite;
        }
        else
        {
            GetComponent<Image>().sprite = pauseSprite;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (pauseMenu.GetComponent<UIToggler>().IsVisible)
        {
            pauseMenu.Hide();
            MusicLoader.Instance.ResumeMusic();
        }
        else
        {
            MusicLoader.Instance.PauseMusic();
            pauseMenu.Show();
        }

        UIAudio.Instance.PlayTick();
    }
}
