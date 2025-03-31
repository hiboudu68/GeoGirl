using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FileLevelHandler : MonoBehaviour
{
    [SerializeField] string filename = "tilemapTest.json";
    //[SerializeField] Dictionary<TileBase, GameObject> linkTilePrefab = new();
    [SerializeField] List<TileBase> tilemaps = new();
    [SerializeField] List<GameObject> prefabs = new();

    private void Start()
    {
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(filename);
        Tilemap map = FindObjectOfType<Tilemap>();

        //with the position put the good object in this position
        foreach (var mapData in data)
        {
            if (mapData.tiles == null || mapData.tiles.Count == 0)
            {
                continue;
            }

            foreach (TileInfo tile in mapData.tiles)
            {
                TileBase tileBase = tile.tileBase;
                if (tileBase == null)
                {
                    string path = AssetDatabase.GUIDToAssetPath(tile.guidFromAssetDB);
                    tileBase = AssetDatabase.LoadAssetAtPath<TileBase>(path);

                    if (tileBase == null)
                    {
                        Debug.Log("Tile not found in AssetDatabase");
                        continue;
                    }
                }

                int index = tilemaps.FindIndex(tm => tm.name == tileBase.name);
                if (index > -1 && index < tilemaps.Count) //tiles.TryGetValue(tileBase, out GameObject val)
                {
                    Vector3 vector = map.CellToWorld(tile.position);
                    GameObject newGameObj = Instantiate(prefabs[index], vector, Quaternion.identity);

                    //this.gameObject.AddComponent(newGameObj.GetType());
                    //GetComponent<Canvas>().AddComponent<Image>();
                }
                else
                {
                    Debug.Log("TileBase: " + tileBase.name + " ,is not in the dictionnary");
                    continue;
                }
            }
        }
    }

}
