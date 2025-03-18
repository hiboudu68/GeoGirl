using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public static TilesManager Instance;

    public List<TileData> Tiles = new();

    void Start()
    {
        Instance = this;
    }
    void Awake()
    {
        Instance = this;
    }
}
