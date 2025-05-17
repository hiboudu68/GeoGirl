using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSkinLoader : MonoBehaviour
{
    [SerializeField] private int PlayModeID;
    [SerializeField] private Material skinMaterial;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = new Material(skinMaterial);
        //if (spriteRenderer.transform.localScale == Vector3.one)
        //spriteRenderer.transform.localScale = Vector3.one * 0.8f;

        spriteRenderer.sprite = PlayerSkinPreferences.GetSkin(PlayModeID).Icon;
        spriteRenderer.material.SetColor("_ColorA", PlayerSkinPreferences.GetPrimaryColor(PlayModeID));
        spriteRenderer.material.SetColor("_ColorB", PlayerSkinPreferences.GetSecondaryColor(PlayModeID));
    }
}
