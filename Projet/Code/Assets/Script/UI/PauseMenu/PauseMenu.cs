using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    private UIToggler toggler;
    private bool isOpen = false;
    [SerializeField] private Slidable BtnBack;
    [SerializeField] private Slidable BtnReplay;
    [SerializeField] private Text TimeText;
    [SerializeField] private Text TryCountText;
    [SerializeField] private Text JumpCountText;

    void Start()
    {
        isOpen = false;
        BtnBack.OnAnimationEnds += OnSlidableAnimationEnds;
        toggler = GetComponent<UIToggler>();

        foreach (Slidable slidable in GetComponentsInChildren<Slidable>())
            slidable.Hide();

        Player.StartPlaying += OnStartPlaying;
        Player.StopPlaying += OnStartPlaying;
    }
    private void OnStartPlaying()
    {
        toggler.IsVisible = false;
    }
    void Update()
    {
        float targetTime = toggler.IsVisible || Player.Instance == null ? 0f : Player.Instance.TimeScale;

        if (Time.timeScale != targetTime)
            Time.timeScale = targetTime;
    }
    public override void Show()
    {
        //if (toggler.IsVisible || isOpen)
        //return;
        GameGrid grid = FindAnyObjectByType<GameGrid>();
        IEnumerable<BaseObject> coins = grid.Objects.Where(x => x.GetComponentInChildren<Coin>(true) != null);
        int collectedCoinsCount = coins.Count(x => !x.gameObject.activeInHierarchy);
        int notCollectedCoinsCount = coins.Count(x => x.gameObject.activeInHierarchy);

        TryCountText.text = Statistics.CurrentLevelTryCount.ToString() + " essais";
        JumpCountText.text = Statistics.CurrentLevelJumpCount.ToString() + " sauts";

        LevelComponent levelComponent = GetComponentInChildren<LevelComponent>(true);
        levelComponent.SetInfos(grid.CurrentLevel.Name, collectedCoinsCount, collectedCoinsCount + notCollectedCoinsCount, grid.GetLevelProgression(Player.Instance.transform.position.x));

        TimeSpan time = TimeSpan.FromSeconds(Player.Instance.LifeTime);
        TimeText.text = time.Minutes + " min " + time.Seconds + "s";
        toggler.IsVisible = true;
        TimeText.gameObject.SetActive(true);
        TryCountText.gameObject.SetActive(true);
        JumpCountText.gameObject.SetActive(true);
        foreach (Slidable slidable in GetComponentsInChildren<Slidable>(true))
            slidable.Show();

    }
    public override void Hide()
    {
        foreach (Slidable slidable in GetComponentsInChildren<Slidable>(true))
            slidable.Hide();
    }
    private void OnSlidableAnimationEnds(Slidable s)
    {
        if (isOpen)
        {
            toggler.Toggle();
            isOpen = false;
        }
        else isOpen = true;
    }
}
