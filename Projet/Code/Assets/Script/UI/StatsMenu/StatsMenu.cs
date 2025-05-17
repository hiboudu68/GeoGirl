using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : BaseMenu
{
    [SerializeField] private Text levelCount;
    [SerializeField] private Text coinsCount;
    [SerializeField] private Text progressionAverage;
    [SerializeField] private Text totalTryCount;

    private void LoadStats()
    {
        LevelStatistics[] stats = PlayerStats.GetStatistics();
        if (stats.Length == 0)
        {
            levelCount.text = "Vous devez jouer au moins une fois...";
            coinsCount.text = string.Empty;
            progressionAverage.text = string.Empty;
            totalTryCount.text = string.Empty;
        }
        else
        {
            levelCount.text = "Nombre de niveaux complétés : " + stats.Count(x => x.Progression == 100);
            coinsCount.text = "Nombre de pièces collectées : " + stats.Sum(x => x.CollectedCoins);
            progressionAverage.text = "Progression moyenne : " + stats.Average(x => x.Progression) + "%";
            totalTryCount.text = "Nombre total d'essais : " + stats.Sum(x => x.TryCount);
        }
    }
    public override void Show()
    {
        //LoadStats();
        transform.parent.GetComponentInChildren<MainMenuController>().Hide();
        transform.parent.GetComponentInChildren<LevelSlider>().HideButtons();

        transform.parent.GetComponentInChildren<BtnPlay>()
                    .GetComponent<Slidable>().Hide();

        GetComponentInChildren<Slidable>().Hide(); //Hide to up panel

        LoadStats();
    }
    public override void Hide()
    {
        transform.parent.GetComponentInChildren<MainMenuController>().Show();
        transform.parent.GetComponentInChildren<BtnPlay>()
           .GetComponent<Slidable>().Show();

        GetComponentInChildren<Slidable>().Show(); //Show to down panel
    }
}
