using UnityEngine;
using UnityEngine.EventSystems;

public class BtnLevelSwap : MonoBehaviour, IPointerClickHandler
{
    private Vector3 visiblePosition;
    private Vector3 invisiblePosition;
    private Vector3 targetPosition;
    [SerializeField] private Direction direction;

    void Start()
    {
        visiblePosition = transform.localPosition;
        invisiblePosition = visiblePosition + (direction == Direction.LeftToRight ? Screen.width / 2 : -Screen.width / 2) * Vector3.right;
        targetPosition = invisiblePosition;
        transform.localPosition = targetPosition;
    }
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.unscaledDeltaTime * 10f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (direction == Direction.RightToLeft)
            GetComponentInParent<LevelSlider>().Previous();
        else GetComponentInParent<LevelSlider>().Next();
        UIAudio.Instance.PlayTick();
    }

    public void Show()
    {
        targetPosition = visiblePosition;
    }
    public void Hide()
    {
        targetPosition = invisiblePosition;
    }
}
public enum Direction
{
    RightToLeft = -1,
    LeftToRight = 1
}
