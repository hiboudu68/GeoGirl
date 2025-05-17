using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float topLimit = 7f;
    [SerializeField] private float bottomLimit = -7f;
    [SerializeField] private float verticalPadding = 1f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    public Player Player { get; set; }

    void Start()
    {
        startPosition = transform.position;
    }
    public void ResetPosition()
    {
        transform.position = startPosition;
    }
    void FixedUpdate()
    {
        if (Player != null && Player.Instance.currentMode is not WinMode)
        {
            Transform t = Player.transform;
            float cameraY = transform.position.y;
            float _y = t.position.y;
            float relativeY = _y - cameraY;
            int tryCount = 0;
            while (relativeY > topLimit && tryCount++ < 50)
            {
                cameraY += verticalPadding;
                relativeY = _y - cameraY;
            }
            while (relativeY < bottomLimit && tryCount++ < 50)
            {
                cameraY -= verticalPadding;
                relativeY = _y - cameraY;
            }

            targetPosition = new(t.position.x, cameraY, t.position.z - 1);
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.fixedDeltaTime * 2.5f);
        }
    }
}
