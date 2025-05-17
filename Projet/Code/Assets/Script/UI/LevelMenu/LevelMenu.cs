using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : BaseMenu
{
    [SerializeField] private Button btnBack;
    private LevelSlider levelSlider;
    private CanvasRenderer btnBackRenderer;

    public Slidable BtnBack { get => btnBack.GetComponent<Slidable>(); }
    public bool IsVisible { get => levelSlider.IsVisible; }

    void Start()
    {
        levelSlider = GetComponent<LevelSlider>();
        btnBackRenderer = btnBack.GetComponent<CanvasRenderer>();
        transform.parent.GetComponentInChildren<MainMenuController>()
            .GetComponentInChildren<BtnSkins>()
            .GetComponent<Slidable>().OnAnimationEnds += OnBtnBackAnimationEnds;

        btnBack.gameObject.SetActive(false);

        levelSlider.SetLevels(LevelsManager.GetLevels());
    }
    private void OnBtnBackAnimationEnds(Slidable slidable)
    {
        if (slidable.IsVisible)
            btnBack.gameObject.SetActive(false);
    }
    public override void Hide()
    {
        levelSlider.HideButtons();
        btnBack.GetComponent<Slidable>().Show();
    }
    public override void Show()
    {
        levelSlider.ShowButtons();
        GetComponent<UIToggler>().IsVisible = true;
        btnBack.gameObject.SetActive(true);
        btnBack.GetComponent<Slidable>().Hide();
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
