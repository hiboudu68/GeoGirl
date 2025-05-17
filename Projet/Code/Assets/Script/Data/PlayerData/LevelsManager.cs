using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelsManager
{
    public delegate void LevelListUpdate(Level[] newLevelList);
    public static event LevelListUpdate LevelListChanged;


    public static void SaveLevel(Level level)
    {
        Level[] levels = GetEditableLevels();
        for (int i = 0; i < levels.Length; i++)
            if (levels[i].Id == level.Id)
                levels[i] = level;

        Save(levels);
    }
    public static void CreateLevel(Level editableLevel)
    {
        editableLevel.IsMainLevel = false;
        Level[] levels = GetEditableLevels();
        Array.Resize(ref levels, levels.Length + 1);
        levels[levels.Length - 1] = editableLevel;

        Save(levels);
        LevelListChanged?.Invoke(levels);
    }
    public static void DeleteLevel(Level editableLevel)
    {
        if (editableLevel == null || editableLevel.IsMainLevel)
            return;

        List<Level> levelList = new List<Level>(GetEditableLevels());
        levelList.RemoveAll(x => x.Id == editableLevel.Id);

        Level[] levels = levelList.ToArray();
        Save(levels);
        LevelListChanged?.Invoke(levels);
    }
    private static void Save(Level[] levels)
    {
        string json = JsonUtility.ToJson(new LevelCollection() { Levels = levels });
        File.WriteAllText(FilePath.GetPlayerLevels(), json);
    }
    public static Level[] GetEditableLevels()
    {
        string path = FilePath.GetPlayerLevels();
        if (!File.Exists(path))
            return Array.Empty<Level>();

        string fileContent = File.ReadAllText(path);
        return JsonUtility.FromJson<LevelCollection>(fileContent).Levels;
    }
    public static Level[] GetLevels()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("levels");
        Level[] levels = JsonUtility.FromJson<LevelCollection>(jsonFile.text).Levels;
        foreach (Level level in levels)
        {
            TextAsset binary = Resources.Load<TextAsset>(level.Id);
            File.WriteAllBytes(FilePath.GetLevelPath(level), binary.bytes);
        }

        return levels;
    }
}
