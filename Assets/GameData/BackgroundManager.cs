using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    public List<Sprite> Tiles = new();

    void Start()
    {
        Instance = this;
    }
    void Awake()
    {
        Instance = this;
    }
}
