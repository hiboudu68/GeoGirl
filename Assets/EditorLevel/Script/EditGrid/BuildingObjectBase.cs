using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PlaceType
{
    Single,
    Rectangle
}

[CreateAssetMenu (fileName ="Buildable", menuName ="BuildingObjects/Create Buildable")]
public class BuildingObjectsBase : ScriptableObject
{
    [SerializeField] TileBase tileBase;
    [SerializeField] PlaceType placeType;

    public TileBase TileBase {
        get { return tileBase; }
    }

    public PlaceType PlaceType
    {
        get { return placeType; }
    }
}
