using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlayerSkinPreferences
{
    private static bool volume;

    public static bool Volume { get => volume; set => SetVolume(value); }
    private static Dictionary<int, SkinData> skinsByMode = new();
    private static Dictionary<int, Color> primaryColorsByMode = new();
    private static Dictionary<int, Color> secondaryColorsByMode = new();

    static PlayerSkinPreferences()
    {
        volume = PlayerPrefs.GetInt("volume") > 0;
        foreach (PlayMode mode in GameModesManager.Instance.GetModes())
        {
            LoadSkin(mode);
            LoadColor(mode);
        }
    }
    private static void LoadSkin(PlayMode mode)
    {
        if (PlayerPrefs.HasKey($"skinMode[{mode.Id}]"))
        {
            int id = PlayerPrefs.GetInt($"skinMode[{mode.Id}]");
            SkinData skin = mode.Skins.FirstOrDefault(x => x.Id == id);
            skinsByMode[mode.Id] = skin ?? mode.Skins.First(x => x.Id == mode.DefaultSkin);
        }
        else
        {
            skinsByMode[mode.Id] = mode.Skins.First(x => x.Id == mode.DefaultSkin);
        }
    }
    private static void LoadColor(PlayMode mode)
    {
        if (PlayerPrefs.HasKey($"colorModeA[{mode.Id}]"))
        {
            int i = PlayerPrefs.GetInt($"colorModeA[{mode.Id}]");
            primaryColorsByMode[mode.Id] = IntToColor(i);
        }
        else primaryColorsByMode[mode.Id] = mode.DefaultPrimaryColor;

        if (PlayerPrefs.HasKey($"colorModeB[{mode.Id}]"))
        {
            int i = PlayerPrefs.GetInt($"colorModeB[{mode.Id}]");
            secondaryColorsByMode[mode.Id] = IntToColor(i);
        }
        else secondaryColorsByMode[mode.Id] = mode.DefaultSecondaryColor;
    }
    private static Color IntToColor(int i)
        => new Color((i >> 16 & 0xff) / 255f, (i >> 8 & 0xff) / 255f, (i & 0xff) / 255f);
    private static int ColorToInt(Color c)
        => ((int)(c.r * 255) & 0xff) << 16 | ((int)(c.g * 255) & 0xff) << 8 | ((int)(c.b * 255) & 0xff);


    private static void SetVolume(bool value)
    {
        if (value == volume)
            return;

        volume = value;
        PlayerPrefs.SetInt("volume", value ? 1 : 0);
    }

    public static Color GetPrimaryColor(int modeId) => primaryColorsByMode[modeId];
    public static Color GetSecondaryColor(int modeId) => secondaryColorsByMode[modeId];
    public static SkinData GetSkin(int modeId) => skinsByMode[modeId];
    public static void SetPrimaryColor(int modeId, Color color)
    {
        primaryColorsByMode[modeId] = color;
        PlayerPrefs.SetInt($"colorModeA[{modeId}]", ColorToInt(color));
    }
    public static void SetSecondaryColor(int modeId, Color color)
    {
        secondaryColorsByMode[modeId] = color;
        PlayerPrefs.SetInt($"colorModeB[{modeId}]", ColorToInt(color));
    }
    public static void SetSkin(int modeId, SkinData skin)
    {
        skinsByMode[modeId] = skin;
        PlayerPrefs.SetInt($"skinMode[{modeId}]", skin.Id);
    }
}
