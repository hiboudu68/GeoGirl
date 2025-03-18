using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpHandler : PlayerController
{

    // Vitesse horizontale constante (à ajuster selon vos besoins)
    public float moveSpeed = 5f;
    // Force appliquée lors du saut (à ajuster selon vos besoins)
    public float jumpForce = 10f;
    public bool autoAdjustJumpForce = false;
    public int jumpHeightMultiplier = 3;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private BoxCollider2D collider;

    protected override void InitControl()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        if(rb == null)
        {
            Debug.LogError("Le composant Rigidbody2D est requis sur cet objet !");
        }

        collider = GetComponent<BoxCollider2D>();
        if(collider == null)
            return;

        float playerHeight = collider.bounds.size.y;
        float gravityMagnitude = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float requiredJumpForce = Mathf.Sqrt(2 * gravityMagnitude * (jumpHeightMultiplier * playerHeight));
        
        if(autoAdjustJumpForce)
            jumpForce = requiredJumpForce;
    }

    void Update()
    {
        if(!IsActive)
            return;

        // Si on appuie sur la barre espace ou on clique et que l'objet est au sol, on effectue un saut
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
        {
            // On réinitialise la composante verticale de la vélocité pour un saut précis
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        if(!IsActive)
            return;
            
        // Applique une vitesse horizontale constante
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.GetComponent<Bumper>() != null){
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
    }

}
