using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkinsList : MonoBehaviour
{
    private GameModeList modeList;
    private List<SkinContainer> skins = new();
    private SkinContainer currentSkin = null;
    private Image skinImage;

    public Material SpriteMaterial;
    [SerializeField] private Color choosedSkinBackgroundColor;
    [SerializeField] private Sprite background;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Debug.Log("Starting");
        yield return null;

        skinImage = transform.parent.GetComponentsInChildren<Image>().First(x => x.CompareTag("Player"));
        skinImage.material = new Material(SpriteMaterial);

        modeList = transform.parent.GetComponentInChildren<GameModeList>();
        modeList.ModeChanged += OnModeChanged;
        if (modeList.CurrentMode != null)
            OnModeChanged(modeList.CurrentMode);

        SkinsMenu skinMenu = GetComponentInParent<SkinsMenu>();
        skinMenu.PrimaryChanged += OnPrimaryChanged;
        skinMenu.SecondaryChanged += OnSecondaryChanged;

        OnPrimaryChanged(skinMenu.PrimaryColorPicker.color);
        OnSecondaryChanged(skinMenu.SecondaryColorPicker.color);
    }
    private void OnPrimaryChanged(Color c)
    {
        skinImage.material.SetColor("_ColorA", c);
    }
    private void OnSecondaryChanged(Color c)
    {
        skinImage.material.SetColor("_ColorB", c);
    }

    private void OnModeChanged(PlayMode mode)
    {
        foreach (SkinContainer skin in skins)
            Destroy(skin.transform.parent.gameObject);

        skins.Clear();

        currentSkin = null;
        SkinData choosenSkin = PlayerSkinPreferences.GetSkin(mode.Id);
        foreach (SkinData skinData in mode.Skins)
        {
            GameObject bgObj = new GameObject();
            bgObj.AddComponent<Image>().sprite = background;
            bgObj.transform.localScale = Vector3.one / 1.5f;

            GameObject obj = new GameObject();
            obj.AddComponent<Image>().sprite = skinData.Icon;
            obj.transform.localScale = Vector3.one / 3f;
            SkinContainer skinContainer = obj.AddComponent<SkinContainer>();
            skinContainer.skin = skinData;

            skins.Add(skinContainer);

            obj.transform.parent = bgObj.transform;
            bgObj.transform.parent = transform;

            if (choosenSkin.Id == skinData.Id)
            {
                ChooseSkin(skinContainer);
            }
            else skinContainer.BackgroundColor = new Color(.8f, .8f, .8f);
        }
    }
    public void ChooseSkin(SkinContainer skinContainer)
    {
        if (currentSkin == skinContainer)
            return;
        if (currentSkin != null)
            currentSkin.BackgroundColor = new Color(.8f, .8f, .8f);

        currentSkin = skinContainer;
        currentSkin.BackgroundColor = choosedSkinBackgroundColor;
        PlayerSkinPreferences.SetSkin(modeList.CurrentMode.Id, currentSkin.skin);

        skinImage.sprite = skinContainer.skin.Icon;
    }
}
