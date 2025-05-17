using UnityEngine;

[RequireComponent(typeof(UIToggler))]
public class ConfirmPopup : BaseMenu
{
    public delegate void ConfirmResultCallback(bool confirm);
    public static ConfirmPopup Instance;
    private ConfirmResultCallback callback;

    private UIToggler toggler;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        toggler = GetComponent<UIToggler>();
    }

    public void GetValue(ConfirmResultCallback callback)
    {
        this.callback = callback;
        Show();
    }
    public void Confirm()
    {
        this.callback?.Invoke(true);
        Hide();
    }
    public void Cancel()
    {
        this.callback?.Invoke(false);
        Hide();
    }
    public override void Hide()
    {
        toggler.IsVisible = false;
    }
    public override void Show()
    {
        toggler.IsVisible = true;
    }
}
