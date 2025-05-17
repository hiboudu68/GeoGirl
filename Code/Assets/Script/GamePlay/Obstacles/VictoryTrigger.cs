using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>() == Player.Instance)
        {
            PlayerStats.SetLevelStats(new()
            {
                LevelId = FindAnyObjectByType<GameGrid>().CurrentLevel.Id,
                Progression = 100f,
                CollectedCoins = Statistics.CurrentLevelCoinsCount,
                JumpCount = Statistics.CurrentLevelJumpCount,
                TryCount = Statistics.CurrentLevelTryCount
            });

            Player.Win();
        }
    }
}
