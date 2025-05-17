using UnityEngine;

public class PlayerJumpHandler : PlayerController
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float angularForce = -270f;

    private Player player;
    private float jumpCD = 0f;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool jumping = false;

    protected override void InitControl()
    {
        player = GetComponentInParent<Player>();
        transform.localPosition = Vector2.zero;
        rb = GetComponentInParent<Rigidbody2D>();
        rb.angularVelocity = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 3f;
    }

    void Update()
    {
        if (!IsActive)
            return;

        isGrounded = IsGrounded();
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && isGrounded)
        {
            Jump();
        }
        else
        {
            rb.linearVelocity = new Vector2(GameSettings.MoveSpeed, rb.linearVelocity.y);
        }
    }
    void FixedUpdate()
    {
        if (!IsActive)
            return;

        if (jumpCD == 0.1f)
        {
            Statistics.CurrentLevelJumpCount++;
            jumpCD = 0.09f;
        }
        jumpCD -= Time.fixedDeltaTime;

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(GameSettings.MoveSpeed, 0);
            rb.angularVelocity = 0f;
            SnapAngle();
            rb.freezeRotation = true;
        }
        else
        {
            rb.freezeRotation = false;
        }
    }
    public void Jump()
    {

        jumpCD = 0.1f;
        isGrounded = false;
        if (rb != null)
        {
            rb.freezeRotation = false;
            rb.linearVelocity = new Vector2(GameSettings.MoveSpeed, GameSettings.JumpForce);
            rb.angularVelocity = GameSettings.AngularForce;
        }
    }
    private void SnapAngle()
    {
        float currentAngle;
        Vector3 rotationAxis;
        transform.rotation.ToAngleAxis(out currentAngle, out rotationAxis);

        if (rotationAxis.z < 0)
            currentAngle = -currentAngle;

        float targetAngle = Mathf.Round(currentAngle / 90f) * 90f;
        transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
    }
    bool IsGrounded()
        => Physics2D.Raycast(transform.position, Vector2.down, GameSettings.CubeHitboxSize, player.GroundMask).collider != null;
}
