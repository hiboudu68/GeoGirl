using System;
using UnityEngine;
using UnityEngine.UI;

public class EditableLevelListMenu : BaseMenu
{
    public GameObject PlayerPrefab;
    [SerializeField] private Button btnBack;
    private LevelSlider levelSlider;

    public Slidable BtnBack { get => btnBack.GetComponent<Slidable>(); }
    public bool IsVisible { get => levelSlider.IsVisible; }

    void Start()
    {
        levelSlider = GetComponent<LevelSlider>();
        transform.parent.GetComponentInChildren<MainMenuController>()
            .GetComponentInChildren<BtnSkins>()
            .GetComponent<Slidable>()
            .OnAnimationEnds += OnBtnBackAnimationEnds;
        btnBack.gameObject.SetActive(false);

        LevelsManager.LevelListChanged += OnLevelList;
    }
    public Level GetSelectedLevel() => levelSlider.Level;
    private void OnLevelList(Level[] levels)
    {
        levelSlider.SetLevels(levels);
    }
    private void OnBtnBackAnimationEnds(Slidable slidable)
    {
        if (slidable.IsVisible)
            btnBack.gameObject.SetActive(false);
    }
    public void StartEditLevel(Level lvl)
    {
        levelSlider.HideButtons();
        transform.parent.GetComponentInChildren<MainMenuController>()
            .GetComponentInChildren<BtnPlay>()
            .GetComponent<Slidable>().Hide();

        GetComponent<UIToggler>().IsVisible = false;
        transform.parent.GetComponentInChildren<MapEditorManager>()
            .Edit(lvl)
            .Show();
    }
    public override void Hide()
    {
        levelSlider.HideButtons();
        levelSlider.SetLevels(Array.Empty<Level>());

        BtnBack.Show();
        foreach (Slidable slidable in GetComponentsInChildren<Slidable>())
        {
            if (slidable != BtnBack)
            {
                slidable.gameObject.SetActive(true);
                slidable.Show();
            }
        }
    }
    public override void Show()
    {
        UIToggler toggler = GetComponent<UIToggler>();
        if (toggler.IsVisible == false)
        {
            toggler.IsVisible = true;
            transform.parent.GetComponentInChildren<MainMenuController>()
                .GetComponentInChildren<BtnPlay>()
                .GetComponent<Slidable>().Show();
        }
        levelSlider.SetLevels(LevelsManager.GetEditableLevels());
        levelSlider.ShowButtons();
        btnBack.gameObject.SetActive(true);
        BtnBack.Hide();
        foreach (Slidable slidable in GetComponentsInChildren<Slidable>())
        {
            if (slidable != BtnBack)
                slidable.Hide();
        }
    }
    public void HideBack(bool capturePosition = true)
    {
        if (capturePosition)
        {
            btnBack.GetComponent<Slidable>()
                .CaptureStartPosition(Vector3.down * -250f + Vector3.left * -60f);
        }
        else btnBack.gameObject.SetActive(false);
    }
    public void ShowBack()
    {
        btnBack.gameObject.SetActive(true);
        //btnBack.GetComponent<Slidable>().CaptureStartPosition(Vector3.zero);
        btnBack.GetComponent<Slidable>().Hide();
    }
}
