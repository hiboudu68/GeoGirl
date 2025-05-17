using UnityEngine;

public class Slidable : MonoBehaviour
{
    public delegate void SlidableEvent(Slidable slidable);
    public event SlidableEvent OnAnimationEnds;

    private Canvas canvas;
    private bool isHidding = false;
    private bool isShowing = false;
    private Vector3 startPosition, startScale;
    private Vector3 currentOffset = Vector3.zero;
    private Vector3 currentScale = Vector3.zero;
    public Vector3 Offset;
    public Vector3 Scale = Vector3.zero;

    [SerializeField] private bool isHidden = false;
    [SerializeField] private float speed = 1f;

    public bool IsVisible { get => isShowing || (currentOffset - Offset).magnitude < 1f; }

    public void CaptureStartPosition(Vector3 currentOffset)
    {
        startPosition = transform.position - currentOffset;
        isShowing = false;
        isHidding = false;
        this.currentOffset = currentOffset;
    }
    public void Show()
    {
        isShowing = true;
        isHidding = false;
    }
    public void Hide()
    {
        isShowing = false;
        isHidding = true;
    }
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        startPosition = transform.position;
        startScale = transform.localScale;
        if (isHidden)
        {
            transform.position = startPosition + Offset;
            transform.localScale = startScale + Scale;
            currentOffset = Offset;
            currentScale = Scale;
        }
    }
    void Update()
    {
        if (isHidding)
        {
            Vector3 scaledOffset = Offset * canvas.scaleFactor;
            Vector3 scaledScale = Scale * canvas.scaleFactor;
            currentOffset = Vector3.Lerp(currentOffset, scaledOffset, Time.unscaledDeltaTime * speed);
            currentScale = Vector3.Lerp(currentScale, scaledScale, Time.unscaledDeltaTime * speed);

            if ((currentOffset - scaledOffset).magnitude < 1f && (currentScale - scaledScale).magnitude < 1f)
            {
                isHidding = false;
                OnAnimationEnds?.Invoke(this);
            }
        }
        else if (isShowing)
        {
            currentOffset = Vector3.Lerp(currentOffset, Vector3.zero, Time.unscaledDeltaTime * speed);
            currentScale = Vector3.Lerp(currentScale, Vector3.zero, Time.unscaledDeltaTime * speed);
            if (currentOffset.magnitude < 1f && currentScale.magnitude < 1f)
            {
                isShowing = false;
                OnAnimationEnds?.Invoke(this);
            }
        }
        else return;

        transform.position = startPosition + currentOffset;
        transform.localScale = startScale + currentScale;
    }
}
