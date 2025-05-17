using System.Collections.Generic;
using UnityEngine;

public class GameModesManager : MonoBehaviour
{
    public static GameModesManager Instance { get; private set; }

    [SerializeField] private GameModeCollection gameModeSkinRegistry;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public List<PlayMode> GetModes() => gameModeSkinRegistry.playModes;
    public List<SkinData> GetSkinsForGameMode(string gameMode)
    {
        foreach (PlayMode pm in gameModeSkinRegistry.playModes)
        {
            if (pm.ModeName.Equals(gameMode, System.StringComparison.OrdinalIgnoreCase))
                return pm.Skins ?? new List<SkinData>();
        }

        return new List<SkinData>();
    }
}
