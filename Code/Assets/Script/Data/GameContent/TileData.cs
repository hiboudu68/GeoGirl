using UnityEngine;

[System.Serializable]
public class TileData
{
    private static int LastId = 0;

    public int Id = ++LastId;
    public Sprite sprite;
    public GameObject Prefab;
    public string name;
    public int xSize;
    public int ySize;
}
