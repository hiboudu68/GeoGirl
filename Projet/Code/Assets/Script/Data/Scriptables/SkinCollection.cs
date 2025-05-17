using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinsCollection", menuName = "Game/Skin Collection")]
public class SkinCollection : ScriptableObject
{
    public List<SkinData> Skins;
}
