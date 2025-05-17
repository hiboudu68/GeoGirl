using UnityEngine;

public class PlayerBall : PlayerController
{
    [SerializeField] private float rotationSpeed = 100f;
    private float reverseTimer = 0f;
    private Rigidbody2D rb;

    // Start is called before the first frame update

    protected override void InitControl()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(GameSettings.MoveSpeed, rb.linearVelocity.y);
        rb.freezeRotation = true;
        rb.angularDamping = 0f;
        rb.gravityScale = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -(rotationSpeed * rb.gravityScale) * Time.deltaTime);
        Reverse(false);
    }
    public void Reverse(bool ignoreInputs = true)
    {
        if (reverseTimer > 0f)
        {
            reverseTimer -= Time.deltaTime;
            return;
        }

        bool isPressing = ignoreInputs || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        if (isPressing)
        {
            rb.gravityScale = -rb.gravityScale;
            reverseTimer = 0.1f;
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = new(GameSettings.MoveSpeed, rb.linearVelocity.y);
    }

}
