using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    public bool IsActive = true;

    void Start()
    {
        InitControl();
    }

    protected abstract void InitControl();
}
