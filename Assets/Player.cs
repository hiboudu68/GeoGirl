using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 3;
    public float JumpForce=10;
    public float GroundCheckRadius= 0.5f;
    public LayerMask GroundLayer;
    private Rigidbody2D rb;
    private bool Jumping= false;
    private bool Grounded=false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position+= new Vector3(MoveSpeed*Time.fixedDeltaTime,0);
        
    }
    void Update()
    {
        Grounded=Physics2D.OverlapCircle(transform.position,GroundCheckRadius,GroundLayer);
        if(Input.GetKey(KeyCode.Space) && Grounded==true)
        {
            Jump();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,GroundCheckRadius);
    }

    public void Jump()
    {
        rb.velocity=new Vector2(rb.velocity.x,JumpForce);
        Jumping=true;
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        Bumper bump = collider2D.GetComponent<Bumper>();

        if(bump!=null)
        {
            Jump();
        }
    }
}
