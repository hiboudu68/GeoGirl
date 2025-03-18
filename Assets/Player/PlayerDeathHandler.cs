using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    private Player player;
    public PlayerController deadPrefab;
      public GameManager gameManager; 

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
    }
    
      void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("dead"))
        {
            GameManager.IncrementTries();
            Die(); 
        }
    }

    void Die()
    {
        Debug.Log("DEAD");
        player.SetControlMode(deadPrefab);
    }
}
