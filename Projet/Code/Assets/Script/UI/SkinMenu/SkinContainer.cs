using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkinContainer : MonoBehaviour, IPointerClickHandler
{
    private SkinsList skinList;
    private Image img;
    private Material spriteMaterial;
    public SkinData skin;
    public Color BackgroundColor;

    IEnumerator Start()
    {
        yield return null;
        img = transform.parent.GetComponent<Image>();
        skinList = transform.parent.GetComponentInParent<SkinsList>();

        SkinsMenu skinMenu = skinList.GetComponentInParent<SkinsMenu>();
        skinMenu.PrimaryChanged += OnPrimaryChanged;
        skinMenu.SecondaryChanged += OnSecondaryChanged;

        spriteMaterial = new Material(skinList.SpriteMaterial);
        GetComponentInChildren<Image>().material = spriteMaterial;
        OnPrimaryChanged(skinMenu.PrimaryColorPicker.color);
        OnSecondaryChanged(skinMenu.SecondaryColorPicker.color);
    }
    private void OnPrimaryChanged(Color c)
    {
        spriteMaterial.SetColor("_ColorA", c);
    }
    private void OnSecondaryChanged(Color c)
    {
        spriteMaterial.SetColor("_ColorB", c);
    }
    void Update()
    {
        if (img == null)
            return;

        img.color = Color.LerpUnclamped(img.color, BackgroundColor, Time.unscaledDeltaTime * 10f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        skinList.ChooseSkin(this);
        UIAudio.Instance.PlayTick();
    }
}
