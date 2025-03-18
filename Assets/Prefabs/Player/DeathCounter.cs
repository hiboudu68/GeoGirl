using UnityEngine;
using UnityEngine.UI;

public class DeathCounter : MonoBehaviour
{
    public static DeathCounter Instance;

    private int Count;
    private Text text;


    public void Add()
    {
        Count++;
        text.text = "Nombre d'essais : " + Count;
    }
    void Start()
    {
        Instance = this;
        text = GetComponent<Text>();
    }
}
