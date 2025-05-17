using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    public PlayerController deadPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        DeadZoneProperty props = other.gameObject.GetComponent<DeadZoneProperty>();
        if (props != null && Player.Instance != null)
        {
            float velocity = GetComponentInParent<Rigidbody2D>().linearVelocity.y;
            if (Mathf.Abs(velocity) > 200f || Mathf.Abs(velocity) < 0.005f)
                velocity = 0f;

            VelocityDirection dir = props.VelocityDirection;
            if (Mathf.Abs(other.gameObject.transform.rotation.eulerAngles.z) > 5)
            {
                if (dir == VelocityDirection.UpToDown)
                {
                    dir = VelocityDirection.DownToUp;
                }
                else if (dir == VelocityDirection.DownToUp)
                {
                    if (transform.position.y < other.gameObject.transform.position.y)
                    {
                        dir = VelocityDirection.UpToDown;
                    }
                }
            }

            switch (dir)
            {
                case VelocityDirection.UpToDown:
                    if (velocity < -2f)
                    {
                        Die();
                    }
                    break;

                case VelocityDirection.DownToUp:
                    if (velocity > 2f)
                    {
                        Die();
                    }
                    break;

                case VelocityDirection.Flat:
                    if (Mathf.Abs(velocity) < 0.1f)
                    {
                        Die();
                    }
                    break;

                case VelocityDirection.Any:
                    if (!Player.Instance.IsGrounded || props.DieIfGrounded)
                        Die();
                    break;
            }
        }
    }

    void Die()
    {
        if (Player.Instance.currentMode is WinMode)
        {
            VictoryMenu.Instance.Show();
            Player.Win();
        }
        else
        {
            Player.Instance.SetControlMode(deadPrefab);
            Statistics.CurrentLevelJumpCount = 0;
            Statistics.CurrentLevelTryCount++;

            GameGrid gameGrid = FindAnyObjectByType<GameGrid>();
            PlayerStats.SetLevelStats(new()
            {
                LevelId = gameGrid.CurrentLevel.Id,
                Progression = gameGrid.GetLevelProgression(Player.Instance.transform.position.x),
                CollectedCoins = Statistics.CurrentLevelCoinsCount,
                JumpCount = Statistics.CurrentLevelJumpCount,
                TryCount = Statistics.CurrentLevelTryCount
            });
            Player.Die();
        }
    }
}
