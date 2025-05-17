using System;


[Serializable]
public class LevelStatisticCollection
{
    public LevelStatistics[] Stats;
}
[Serializable]
public class LevelStatistics
{
    public string LevelId;
    public int JumpCount;
    public int TryCount;
    public int CollectedCoins;
    public float Progression;
}
[Serializable]
public class Statistics
{
    public static int CurrentLevelJumpCount = 0;
    public static int CurrentLevelTryCount = 0;
    public static int CurrentLevelCoinsCount = 0;
}