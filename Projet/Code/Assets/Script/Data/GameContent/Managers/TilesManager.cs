using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public static TilesManager Instance { get; private set; }

    [SerializeField] private TileCollection tilesCollection;
    public Material SpriteMaterial;

    void Start()
    {
        Instance = this;
    }
    void Awake()
    {
        Instance = this;
    }
    public TileData GetTile(int id) => tilesCollection.TileList.FirstOrDefault(x => x.Id == id);
    public List<TileData> GetTiles() => tilesCollection.TileList;
}
