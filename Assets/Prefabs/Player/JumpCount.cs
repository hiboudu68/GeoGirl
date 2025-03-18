using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpCount : MonoBehaviour
{
    public static JumpCount Instance;

    private Text text;
    private int count = 0;

    void Start()
    {
        Instance = this;
        text = GetComponent<Text>();
    }
    public void Reset()
    {
        count = 0;
        UpdateText();
    }
    public void Add()
    {
        count++;
        UpdateText();
    }
    private void UpdateText()
    {
        text.text = "Nombre de sauts : " + count;
    }
}
