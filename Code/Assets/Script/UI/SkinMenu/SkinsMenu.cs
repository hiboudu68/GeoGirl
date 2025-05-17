using UnityEngine;

public class SkinsMenu : MonoBehaviour
{
    public delegate void ColorChanged(Color color);
    public event ColorChanged PrimaryChanged;
    public event ColorChanged SecondaryChanged;

    public FlexibleColorPicker PrimaryColorPicker;
    public FlexibleColorPicker SecondaryColorPicker;


    public void OnPrimaryColorChanged(Color color)
    {
        PrimaryChanged?.Invoke(color);
    }
    public void OnSecondaryColorChanged(Color color)
    {
        SecondaryChanged?.Invoke(color);
    }
}
