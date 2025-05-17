using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameModeList : MonoBehaviour
{
    public delegate void PlayModeEvent(PlayMode mode);
    public event PlayModeEvent ModeChanged;

    private Dictionary<PlayMode, BtnGameMode> buttons = new();
    private List<Image> iconObjs = new();
    private PlayMode choosedMode;

    [SerializeField] private Sprite background;
    [SerializeField] private Color choosedModeBackgroundColor;

    public PlayMode CurrentMode { get => choosedMode; }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        Material spriteMaterial = transform.parent.GetComponentInChildren<SkinsList>().SpriteMaterial;
        SkinsMenu skinMenu = GetComponentInParent<SkinsMenu>();
        skinMenu.PrimaryChanged += OnPrimaryChanged;
        skinMenu.SecondaryChanged += OnSecondaryChanged;

        foreach (PlayMode mode in GameModesManager.Instance.GetModes())
        {
            GameObject bgObj = new();

            bgObj.AddComponent<RectTransform>();
            bgObj.transform.localScale = Vector3.one / 1.5f;

            Image bgImg = bgObj.AddComponent<Image>();
            bgImg.sprite = background;

            GameObject iconObj = new();
            SkinData skin = mode.Skins.First(x => x.Id == mode.DefaultSkin);
            Image img = iconObj.AddComponent<Image>();
            img.sprite = skin.Icon;
            img.material = new Material(spriteMaterial);
            img.material.SetColor("_ColorA", PlayerSkinPreferences.GetPrimaryColor(mode.Id));
            img.material.SetColor("_ColorB", PlayerSkinPreferences.GetSecondaryColor(mode.Id));

            iconObjs.Add(img);

            iconObj.transform.localScale = Vector3.one / 3;

            iconObj.transform.parent = bgObj.transform;
            bgObj.transform.parent = transform;

            BtnGameMode btnGameMode = bgObj.AddComponent<BtnGameMode>();
            btnGameMode.Mode = mode;
            buttons.Add(mode, btnGameMode);
            if (choosedMode == null)
                ChooseMode(mode);
            else btnGameMode.BackgroundColor = new Color(.8f, .8f, .8f);
        }
    }
    private void OnPrimaryChanged(Color c)
    {
        Image img = buttons[choosedMode].GetComponentsInChildren<Image>()[1];
        img.material.SetColor("_ColorA", c);
        PlayerSkinPreferences.SetPrimaryColor(choosedMode.Id, c);
    }
    private void OnSecondaryChanged(Color c)
    {
        Image img = buttons[choosedMode].GetComponentsInChildren<Image>()[1];
        img.material.SetColor("_ColorB", c);
        PlayerSkinPreferences.SetSecondaryColor(choosedMode.Id, c);
    }
    public void ChooseMode(PlayMode mode)
    {
        if (!buttons.ContainsKey(mode) || choosedMode == mode)
            return;

        if (choosedMode != null)
            buttons[choosedMode].BackgroundColor = new Color(.8f, .8f, .8f, 1f);

        choosedMode = mode;
        buttons[choosedMode].BackgroundColor = choosedModeBackgroundColor;
        ModeChanged?.Invoke(choosedMode);
        SkinsMenu skinMenu = GetComponentInParent<SkinsMenu>();
        skinMenu.PrimaryColorPicker.color = PlayerSkinPreferences.GetPrimaryColor(choosedMode.Id);
        skinMenu.SecondaryColorPicker.color = PlayerSkinPreferences.GetSecondaryColor(choosedMode.Id);
    }
}
