using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModeCollection", menuName = "Game/Game Mode Collection")]
public class GameModeCollection : ScriptableObject
{
    public List<PlayMode> playModes;
}
