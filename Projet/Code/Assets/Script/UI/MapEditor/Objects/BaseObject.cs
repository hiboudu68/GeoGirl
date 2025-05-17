using UnityEngine;

public class BaseObject : MonoBehaviour
{
    public TileData TileInfos;
    public LevelObject LevelObjectInfos;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = gameObject.AddComponent<SpriteRenderer>();
        sprite.material = new Material(TilesManager.Instance.SpriteMaterial);

        if (TileInfos != null && LevelObjectInfos != null)
        {
            if (TileInfos.Prefab != null)
            {
                GameObject subObj = Instantiate(TileInfos.Prefab);
                subObj.transform.SetParent(transform);
                subObj.transform.localPosition = Vector3.zero;

                gameObject.tag = TileInfos.Prefab.tag;
            }

            Render();
        }
    }
    public void Render(bool updateColors = true)
    {
        transform.position = new Vector3(LevelObjectInfos.X, LevelObjectInfos.Y, 1f);
        transform.rotation = Quaternion.Euler(0f, 0f, LevelObjectInfos.Rotation * 90);

        if (TileInfos == null)
        {
            sprite.sprite = null;
            return;
        }
        sprite.sprite = TileInfos.sprite;
        if (updateColors)
        {
            sprite.material.SetColor("_ColorA", LevelObjectInfos.PrimaryColor);
            sprite.material.SetColor("_ColorB", LevelObjectInfos.SecondaryColor);
        }
    }
    public void Rotate()
    {
        LevelObjectInfos.Rotation = (byte)((LevelObjectInfos.Rotation + 1) % 4);
        transform.rotation = Quaternion.Euler(0f, 0f, LevelObjectInfos.Rotation * 90);
    }
}
