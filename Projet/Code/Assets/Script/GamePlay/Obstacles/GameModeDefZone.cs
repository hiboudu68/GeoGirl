using UnityEngine;

public class GameModeDefZone : MonoBehaviour
{
    public PlayerController planeMode;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponentInParent<Player>() != null)
            Player.Instance.SetControlMode(planeMode);

    }
}
