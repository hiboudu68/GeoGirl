using UnityEngine;

public class PlayerJumpHandler : PlayerController
{

    // Vitesse horizontale constante (à ajuster selon vos besoins)
    public float moveSpeed = 5f;
    // Force appliquée lors du saut (à ajuster selon vos besoins)
    public float jumpForce = 10f;
    public bool autoAdjustJumpForce = false;
    public float angularForce = -270f;
    public LayerMask groundLayer;

    private float jumpCD = 0f;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool jumping = false;

    protected override void InitControl()
    {
        transform.localPosition = Vector2.zero;
        rb = GetComponentInParent<Rigidbody2D>();
        rb.angularVelocity = 0f;
        rb.velocity = Vector2.zero;
    }

    void Update()
    {
        if (!IsActive)
            return;

        isGrounded = IsGrounded();
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && isGrounded)
        {
            jumpCD = 0.1f;
            isGrounded = false;
            rb.freezeRotation = false;
            rb.velocity = new Vector2(moveSpeed, jumpForce);
            rb.angularVelocity = angularForce;
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }

    void FixedUpdate()
    {
        if (!IsActive)
            return;

        if (jumpCD == 0.1f)
        {
            JumpCount.Instance.Add();
            jumpCD = 0.09f;
        }
        jumpCD -= Time.fixedDeltaTime;

        if (isGrounded)
        {
            rb.velocity = new Vector2(moveSpeed, 0);
            rb.angularVelocity = 0f;
            SnapAngle();
            rb.freezeRotation = true;
        }
        else
        {
            rb.freezeRotation = false;
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
    {
        float rayLength = Mathf.Abs(rb.angularVelocity) < 0.35f ? 0.55f : 0.76f; // Longueur du rayon
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);
        return hit.collider != null;
    }
    /*void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Bumper>() != null)
        {
            Debug.Log("Bumper");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        // Vérifier si la collision se fait par le dessous (contact avec le sol)
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }*/

}
