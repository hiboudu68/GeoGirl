using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpHandler : MonoBehaviour
{
    public float jumpForce = 10f;  
    public float rotationSpeed = 15f;  

    private Rigidbody2D rb;        
    private bool isJumping = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
        }

        if (isJumping)
        {
            RotatePlayer();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);  
        isJumping = true; 
    }

    void RotatePlayer()
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))  
        {
            isJumping = false;  
            transform.rotation = Quaternion.identity;  
        }
    }
}
