using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
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
