using System;

[Serializable]
public class LevelCollection
{
    public Level[] Levels;
}
[Serializable]
public class Level
{
    public string Id;
    public bool IsMainLevel;
    public string Name;
    public int TotalBonusCount;
}