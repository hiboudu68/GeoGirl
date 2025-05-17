using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileCollection", menuName = "Game/Obstacle Collection")]
public class TileCollection : ScriptableObject
{
    public List<TileData> TileList;
}
