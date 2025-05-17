using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayMode
{
    private static int LastId = 0;

    public int Id = LastId++;
    public string ModeName;
    public int DefaultSkin;
    public Color DefaultPrimaryColor;
    public Color DefaultSecondaryColor;
    public GameObject PlayModePrefab;
    public List<SkinData> Skins;
}
