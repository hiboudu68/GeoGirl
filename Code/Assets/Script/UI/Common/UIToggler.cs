using UnityEngine;

public class UIToggler : MonoBehaviour
{
    public delegate void UIToggleEvent(bool isVisible);
    public event UIToggleEvent OnVisibilityChanged;

    [SerializeField] private bool isVisible = true;


    public bool IsVisible
    {
        get => isVisible;
        set
        {
            if (isVisible == value)
                return;

            isVisible = value;
            UpdateVisibility();
            OnVisibilityChanged?.Invoke(isVisible);
        }
    }

    public void Toggle()
    {
        LevelsManager.GetLevels();
        IsVisible = !isVisible;
    }
    private void UpdateVisibility()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(isVisible);
    }
    private void OnValidate()
    {
        UpdateVisibility();
    }
}
