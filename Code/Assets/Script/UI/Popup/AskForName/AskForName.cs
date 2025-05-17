using UnityEngine.UI;

public class AskForName : BaseMenu
{
    public delegate void ValueGivenCallback(string value);
    public delegate void CancelledCallback();
    public static AskForName Instance;

    private ValueGivenCallback onValidate;
    private CancelledCallback onCancel;
    private UIToggler toggler;
    private InputField input;

    public string Result { get => input.text; }

    void Start()
    {
        Instance = this;
        input = GetComponentInChildren<InputField>(true);
        toggler = GetComponent<UIToggler>();
    }
    public void GetValue(ValueGivenCallback onValidate, CancelledCallback onCancel = null, string defaultValue = "")
    {
        this.onValidate = onValidate;
        this.onCancel = onCancel;
        input.text = defaultValue;
        Show();
    }
    public void Validate()
    {
        this.onValidate?.Invoke(input.text);
        Hide();
    }
    public void Cancel()
    {
        this.onCancel?.Invoke();
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
