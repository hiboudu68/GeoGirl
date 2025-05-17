using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectSelector : MonoBehaviour, IPointerClickHandler
{
    public TileData TileData;
    private GameObject border;

    void Start()
    {
        GetComponentInParent<MapEditorManager>().SelectedTileChanged += OnSelectedTileChanged;
    }
    private void OnSelectedTileChanged(TileData tile)
    {
        if (tile == TileData)
        {
            border = new GameObject("SelectionBorder", typeof(RectTransform), typeof(Image));
            border.transform.SetParent(transform, false);

            RectTransform rt = border.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.one * -4;
            rt.offsetMax = Vector2.one * 4;

            Image img = border.GetComponent<Image>();
            img.sprite = GetComponentInParent<MapEditorManager>().SelectionFrameSprite;
            img.type = Image.Type.Sliced;
            img.raycastTarget = false;
        }
        else if (border != null)
        {
            DestroyImmediate(border);
            border = null;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<MapEditorManager>().SelectedTile = TileData;
    }
}
