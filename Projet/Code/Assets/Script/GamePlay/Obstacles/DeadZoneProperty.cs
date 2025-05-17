using UnityEngine;

public class DeadZoneProperty : MonoBehaviour
{
    public VelocityDirection VelocityDirection;
    public bool DieIfGrounded = false;
}
public enum VelocityDirection
{
    UpToDown, DownToUp, Flat, Any
}
