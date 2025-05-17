using UnityEngine;

public class PlayerShip : PlayerController
{
    private float currentLift = 0f;
    public float lift = 65f;
    public float gravity = 300f;
    public float speed = 5f;

    private Rigidbody2D rb;


    protected override void InitControl()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(GameSettings.MoveSpeed, 0);
        rb.freezeRotation = true;
    }
    void Update()
    {
        rb.gravityScale = GameSettings.ShipGravity;
        bool isPressing = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        transform.rotation = Quaternion.Euler(0, 0, rb.linearVelocity.y * 2);
        if (IsActive && isPressing)
            currentLift = Mathf.Lerp(currentLift, GameSettings.Lift, Time.deltaTime * 5f);
        else currentLift = Mathf.Lerp(currentLift, 0, Time.deltaTime * 50f);
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * currentLift, ForceMode2D.Force);
        rb.linearVelocity = new(GameSettings.MoveSpeed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("dead"))
            Player.Die();
    }
}
