using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class PlayerStats
{
    public static void DeleteLevelStats(Level level)
    {
        List<LevelStatistics> stats = new List<LevelStatistics>(GetStatistics());
        stats.RemoveAll(x => x.LevelId == level.Id);
        Save(stats.ToArray());
    }
    public static LevelStatistics[] GetStatistics()
    {
        string path = FilePath.GetPlayerStats();
        if (!File.Exists(path))
            return Array.Empty<LevelStatistics>();

        string fileContent = File.ReadAllText(path);
        return JsonUtility.FromJson<LevelStatisticCollection>(fileContent).Stats;
    }
    public static void SetLevelStats(LevelStatistics level)
    {
        LevelStatistics existing = GetLevelStats(level.LevelId);
        if (existing == null || existing.Progression <= level.Progression)
        {
            List<LevelStatistics> stats = new List<LevelStatistics>(GetStatistics());
            stats.RemoveAll(x => x.LevelId == level.LevelId);
            stats.Add(level);

            Save(stats.ToArray());
        }
    }
    private static void Save(LevelStatistics[] stats)
    {
        string json = JsonUtility.ToJson(new LevelStatisticCollection() { Stats = stats });
        File.WriteAllText(FilePath.GetPlayerStats(), json);
    }
    public static LevelStatistics GetLevelStats(string id)
        => GetStatistics().FirstOrDefault(x => x.LevelId == id);
}
