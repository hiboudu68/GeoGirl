using System.IO;
using UnityEngine;

public static class FilePath
{
    private static string basePath;

    public static void SetBasePath(string path)
    {
        basePath = path;
    }
    public static string GetMusicTempPath() => Path.Join(basePath, "music.mp3");
    public static string GetPlayerStats() => Path.Join(basePath, "player_stats.json");
    public static string GetPlayerLevels() => Path.Join(basePath, "players_levels.json");
    public static string GetLevelPath(Level level) => Path.Join(basePath, level.Id + ".geomap");
}
