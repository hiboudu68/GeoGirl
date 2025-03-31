using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEditor;
using UnityEngine.UI;

public class SaveHandler : MonoBehaviour
{
    Dictionary<string, Tilemap> tilemaps = new();
    [SerializeField] string filename = "tilemapTest.json";

    private void Start()
    {
        InitTilemaps();
    }

    private void InitTilemaps()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();

        foreach (Tilemap map in maps) {
            tilemaps.Add(map.name, map);
        }
    }

    public void OnSave()
    {
        List<TilemapData> data = new();

        foreach (var map in tilemaps)
        {
            TilemapData mapData = new()
            {
                key = map.Key
            };

            BoundsInt boundsMap = map.Value.cellBounds;
            for (int x = boundsMap.xMin; x < boundsMap.xMax; x++) {
                for (int y = boundsMap.yMin; y < boundsMap.yMax; y++) {
                    Vector3Int pos = new(x, y, 0);
                    TileBase tile = map.Value.GetTile(pos);

                    if (tile != null) {
                        if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(tile, out string guid, out long localId))
                        {
                            TileInfo ti = new(tile, pos, guid);
                            mapData.tiles.Add(ti);
                        }
                        else {
                            Debug.Log("Could not get guid for tile :" + tile.name);
                        }
                    }
                }
            } 

            data.Add(mapData);
        }

        FileHandler.SaveToJSON<TilemapData>(data, filename);
    }

    public void OnLoad()
    {
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(filename);

        foreach (var mapData in data) {
            if (!tilemaps.ContainsKey(mapData.key)) {
                Debug.Log("This \"" + mapData.key + "\" TileMap doesn't exist");
                continue;
            }

            var map = tilemaps[mapData.key];
            map.ClearAllTiles();
            
            if (mapData.tiles != null && mapData.tiles.Count != 0)
            {
                foreach (TileInfo tile in mapData.tiles) {
                    TileBase tileBase = tile.tileBase;
                    if (tileBase == null)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(tile.guidFromAssetDB);
                        tileBase = AssetDatabase.LoadAssetAtPath<TileBase>(path);
                        
                        if (tileBase == null) {
                            Debug.Log("Tile not found in AssetDatabase");
                            continue;
                        }
                    }

                    map.SetTile(tile.position, tileBase);
                }
            }
        }
    }
}

[Serializable]
public class TilemapData
{
    public string key;
    public List<TileInfo> tiles = new();
}

[Serializable]
public class TileInfo
{
    public TileBase tileBase;
    public string guidFromAssetDB;
    public Vector3Int position;

    public TileInfo(TileBase tile, Vector3Int pos, string guid) { 
        this.tileBase = tile;
        this.guidFromAssetDB = guid;
        this.position = pos;
    }

}
