using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private Slidable btnPause;

    public void Show()
    {
        btnPause.Show();
    }
    public void Hide()
    {
        btnPause.Hide();
    }
}
