using UnityEngine;
using UnityEngine.UI;

public class LevelComponent : MonoBehaviour
{
    public Vector3 TargetPosition;
    public Vector3 StartPosition;
    public Level LevelInfos;

    [SerializeField] private Text levelName;
    [SerializeField] private Text progressText;
    [SerializeField] private Text bonusText;
    [SerializeField] private Image progressImage;


    public void SetInfos(Level levelInfos)
    {
        LevelInfos = levelInfos;
        levelName.text = levelInfos.Name;
        LevelStatistics levelStats = PlayerStats.GetLevelStats(levelInfos.Id);

        if (levelStats != null)
        {
            bonusText.text = $"{levelStats.CollectedCoins}/{levelInfos.TotalBonusCount}";
            progressText.text = levelStats.Progression.ToString() + "%";
            progressImage.fillAmount = levelStats.Progression / 100;
        }
        else
        {
            bonusText.text = $"0/{levelInfos.TotalBonusCount}";
            progressText.text = "0%";
            progressImage.fillAmount = 0;
        }
    }
    public void SetInfos(string name, int collectedCoins, int totalCollectableCoinsCount, float progression)
    {
        levelName.text = name;
        bonusText.text = $"{collectedCoins}/{totalCollectableCoinsCount}";
        progressText.text = progression.ToString() + "%";
        progressImage.fillAmount = progression / 100;
    }
}
