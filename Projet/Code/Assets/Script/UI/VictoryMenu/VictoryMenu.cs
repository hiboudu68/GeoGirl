using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VictoryMenu : BaseMenu
{
    public static VictoryMenu Instance;

    [SerializeField] private Text textTime;
    [SerializeField] private Text textJumps;
    [SerializeField] private Text textTryCount;

    private List<GameObject> coinSprites = new();
    public Sprite coinSprite;
    public Sprite emptyCoinSprite;


    void Start()
    {
        Instance = this;
        Player.OnWin += Show;
    }
    public override void Show()
    {
        TimeSpan time = TimeSpan.FromSeconds(Player.Instance.LifeTime);
        int jumps = Statistics.CurrentLevelJumpCount;
        int tries = Statistics.CurrentLevelTryCount + 1;

        textTime.text = "Temps : " + time.Minutes + "min " + time.Seconds + "s";
        textJumps.text = "Nombre de sauts : " + jumps;
        textTryCount.text = "Nombre d'essais : " + tries;
        GetComponent<UIToggler>().IsVisible = true;
        GetComponentInChildren<Slidable>(true).Hide();

        HorizontalLayoutGroup layout = GetComponentInChildren<HorizontalLayoutGroup>();
        GameGrid grid = FindAnyObjectByType<GameGrid>();
        IEnumerable<BaseObject> coins = grid.Objects.Where(x => x.GetComponentInChildren<Coin>(true) != null);

        foreach (GameObject coin in coinSprites)
            Destroy(coin);
        coinSprites.Clear();

        foreach (BaseObject obj in coins)
        {
            GameObject img = new GameObject();
            img.AddComponent<Image>().sprite = obj.gameObject.activeInHierarchy ? emptyCoinSprite : coinSprite;
            img.transform.SetParent(layout.transform);
            coinSprites.Add(img);
        }
    }
    public override void Hide()
    {
        GetComponentInChildren<Slidable>().Show();
        coinSprites.ForEach(Destroy);
        coinSprites.Clear();
    }
}
