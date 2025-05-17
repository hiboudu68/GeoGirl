using UnityEngine;

public class Coin : MonoBehaviour
{
    void Start()
    {
        Player.StartPlaying += OnStartPlaying;
        Player.StopPlaying += OnStartPlaying;
        FindAnyObjectByType<GameGrid>().Cleared += OnGridCleared;
    }
    private void OnGridCleared()
    {
        FindAnyObjectByType<GameGrid>().Cleared -= OnGridCleared;
        Player.StartPlaying -= OnStartPlaying;
        Player.StopPlaying -= OnStartPlaying;
    }
    private void OnStartPlaying()
    {
        gameObject.transform.parent.gameObject.SetActive(true);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Statistics.CurrentLevelCoinsCount++;
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
