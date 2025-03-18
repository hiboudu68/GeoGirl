using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : PlayerController
{
    private float currentLift = 0f;
    public float lift = 8f;
    public float gravity = 300f;
    public float speed = 5f; 

    private Rigidbody2D rb;


    protected override void InitControl()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
        rb.freezeRotation = true;

        //rb.gravityScale = 0;
    }
    void Update()
    {
        bool isPressing = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        if (IsActive && isPressing)
        {
            currentLift = Mathf.Lerp(currentLift, lift, Time.deltaTime * 5f);
            //rb.velocity = new Vector2(rb.velocity.x, currentLift);
        }
        else currentLift = Mathf.Lerp(currentLift, 0, Time.deltaTime * 50f);
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * currentLift, ForceMode2D.Force);
        rb.velocity = new(speed, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("dead")) 
        {
            Debug.Log("Game Over !");
            Destroy(gameObject); 
        }
    }
}
