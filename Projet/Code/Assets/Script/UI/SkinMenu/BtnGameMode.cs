using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnGameMode : MonoBehaviour, IPointerClickHandler
{
    private GameModeList modeList;
    private Image img;
    public PlayMode Mode;
    public Color BackgroundColor;

    public void OnPointerClick(PointerEventData eventData)
    {
        modeList.ChooseMode(Mode);
        UIAudio.Instance.PlayTick();
    }
    void Start()
    {
        modeList = GetComponentInParent<GameModeList>();
        img = GetComponent<Image>();
    }
    void Update()
    {
        img.color = Color.LerpUnclamped(img.color, BackgroundColor, Time.unscaledDeltaTime * 10f);
    }
}
