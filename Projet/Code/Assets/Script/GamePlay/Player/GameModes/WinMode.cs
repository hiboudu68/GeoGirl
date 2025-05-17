using UnityEngine;

public class WinMode : PlayerController
{
    private float speed = 0;
    private float targetSpeed = 5f;

    protected override void InitControl()
    {
        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        Player.Instance.TimeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        speed = Mathf.Lerp(speed, targetSpeed, Time.deltaTime);

        VictoryTrigger victory = FindAnyObjectByType<VictoryTrigger>();
        if (victory != null)
        {
            transform.position = Vector3.Lerp(transform.position, victory.transform.position + Vector3.right * 5f, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, -90f), Time.deltaTime);
        }
        else Destroy(this);
    }
}
