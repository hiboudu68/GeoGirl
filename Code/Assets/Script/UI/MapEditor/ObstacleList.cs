using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObstacleList : MonoBehaviour, IPointerClickHandler
{
    private GridLayoutGroup gridGroup;

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<MapEditorManager>().SelectedTile = null;
    }

    void Start()
    {
        gridGroup = GetComponent<GridLayoutGroup>();

        foreach (TileData tile in TilesManager.Instance.GetTiles())
        {
            GameObject tileDemo = new GameObject(tile.name);
            tileDemo.AddComponent<RectTransform>().pivot = Vector2.zero;
            tileDemo.transform.localScale = Vector3.one * 0.5f;

            Image img = tileDemo.AddComponent<Image>();
            img.sprite = tile.sprite;

            tileDemo.AddComponent<ObjectSelector>().TileData = tile;
            tileDemo.transform.parent = gridGroup.transform;
        }
    }
}
