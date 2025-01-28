using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
     void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("dead"))
        {
            Die(); 
        }
    }

      void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("dead"))
        {
            Die(); 
        }
    }

    void Die()
    {        
        Destroy(gameObject);
    }
}
