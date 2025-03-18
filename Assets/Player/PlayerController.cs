using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    public bool IsActive = true;
    
    void Start(){
        GetComponentInParent<Rigidbody2D>().gravityScale = 3;

        InitControl();
    }

    protected abstract void InitControl();
}
