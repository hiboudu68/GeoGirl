using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : BaseMenu
{
    [SerializeField] private Slidable btnSkins;
    [SerializeField] private Slidable btnEditor;
    [SerializeField] private Slidable btnStats;
    [SerializeField] private Slidable logo;
    [SerializeField] private Slidable btnExit;

    public override void Hide()
    {
        btnSkins.Hide();
        btnEditor.Hide();
        btnStats.Hide();
        btnExit.Hide();
        logo.Hide();
    }
    public override void Show()
    {
        btnSkins.Show();
        btnEditor.Show();
        btnStats.Show();
        btnExit.Show();
        logo.Show();

        GameGrid grid = FindAnyObjectByType<GameGrid>();
        grid.Close();
        grid.ShowStartSprite();

    }
}
