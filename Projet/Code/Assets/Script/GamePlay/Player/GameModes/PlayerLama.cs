using UnityEngine;

public class PlayerLama : PlayerController
{
    private Rigidbody2D rb;
    private float spitTimer = 0f;
    public float speed = 5f;
    public float spitCooldown = 0.2f;
    public float spitForce = 5f;
    public Vector2 spitOffset;
    public GameObject spitPrefab;

    protected override void InitControl()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new(GameSettings.MoveSpeed, rb.linearVelocity.y);

        if (spitTimer > 0f)
        {
            spitTimer -= Time.deltaTime;
            return;
        }

        bool isPressing = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
        if (isPressing)
        {
            Debug.Log("SPIT");
            spitTimer = spitCooldown;
            GameObject spit = GameObject.Instantiate(spitPrefab);
            spit.transform.position = new Vector3(transform.position.x + spitOffset.x, transform.position.y + spitOffset.y, 0f);

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector2 directionToCursor = (mousePosition - spit.transform.position).normalized;
            float angleToCursor = Vector2.SignedAngle(Vector2.right, directionToCursor);
            float clampedAngle = Mathf.Clamp(angleToCursor, -65f, 65f);
            Vector2 finalDirection = Quaternion.Euler(0, 0, clampedAngle) * Vector2.right;

            spit.GetComponent<Rigidbody2D>().AddForce(finalDirection * spitForce, ForceMode2D.Impulse);
        }
    }
}
