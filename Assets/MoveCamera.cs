using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Player player;
    public Transform target;
    public Vector3 offset = new Vector3(5f, 0f, -10f);
    public float smoothSpeed = 5f;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    void FixedUpdate()
    {
        if (player != null && player.currentMode != null && player.currentMode.IsActive)
        {
            transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y + offset.y,
                -10
            );
        }
    }

}
