using UnityEngine;
using UnityEngine.EventSystems;

public class GridObjectsPlacementCursor : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private bool isDown = false;
    private BaseObject cursorObj;
    private MapEditorManager mapEditor;
    private Vector2 onDownPosition;
    private float onDownTime;
    private GameObject border;

    void Start()
    {
        mapEditor = GetComponentInParent<MapEditorManager>(true);
        mapEditor.SelectedTileChanged += OnSelectedTileChanged;
        mapEditor.SelectedObjectChanged += OnSelectedObjectChanged;
        mapEditor.Grid.ObjectChanged += OnSelectedObjectChanged;

        cursorObj = new GameObject().AddComponent<BaseObject>();
        cursorObj.LevelObjectInfos = new();

        border = new GameObject("SelectionBorder");
        SpriteRenderer sprite = border.AddComponent<SpriteRenderer>();
        sprite.sprite = mapEditor.SelectionFrameSprite;
        border.SetActive(false);

        Player.StartPlaying += HideCursor;
        Player.StopPlaying += HideCursor;
        mapEditor.OnExit += HideCursor;
        mapEditor.OnEdit += ShowCursor;
    }
    private void ShowCursor()
    {
        cursorObj.gameObject.SetActive(true);
        border.SetActive(true);
    }
    private void HideCursor()
    {
        cursorObj.gameObject.SetActive(false);
        border.SetActive(false);
    }
    private void OnSelectedObjectChanged(BaseObject obj)
    {
        if (obj == null)
        {
            border.SetActive(false);
        }
        else
        {
            border.SetActive(true);
            border.transform.position = obj.transform.position;
        }
    }
    private void OnSelectedTileChanged(TileData tile)
    {
        border.SetActive(false);
        cursorObj.TileInfos = tile;
        cursorObj.Render();
    }
    public void OnPointerDown(PointerEventData e)
    {
        if (mapEditor.Grid.IsPlaying)
            return;
        onDownPosition = e.position;
        onDownTime = e.clickTime;
        if (e.button == PointerEventData.InputButton.Left)
        {
            isDown = true;
            mapEditor.DisableRaycast();
            TrySelectObjectAt(e.position);
        }
    }

    public void OnPointerMove(PointerEventData e)
    {
        if (mapEditor.Grid.IsPlaying)
            return;

        if (isDown && e.button == PointerEventData.InputButton.Left && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            (int x, int y) = PointerToWorldPosition(e.position);
            mapEditor.Grid.RemoveTileAt(new Vector3Int(x, y, 1));
        }
        else
        {
            if (mapEditor.SelectedObject != null && isDown)
            {
                if (mapEditor.GetComponentInChildren<BtnToggleColorObject>().IsVisible == false)
                    MoveObject(e.position);
            }
            else if (mapEditor.SelectedTile != null)
                MoveTile(e.position);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (mapEditor.Grid.IsPlaying)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDown = false;
            mapEditor.EnableRaycast();
        }
    }
    private void MoveTile(Vector2 position)
    {
        (int x, int y) = PointerToWorldPosition(position);
        if (isDown)
            mapEditor.Grid.AddTileAt(mapEditor.SelectedTile, new Vector3Int(x, y, 1), mapEditor.Rotate);
        else
        {
            cursorObj.LevelObjectInfos.Rotation = mapEditor.Rotate;
            cursorObj.LevelObjectInfos.X = x;
            cursorObj.LevelObjectInfos.Y = y;
            cursorObj.Render(false);
        }
    }
    private void MoveObject(Vector2 position)
    {
        (int x, int y) = PointerToWorldPosition(position);
        LevelObject lvlObj = mapEditor.SelectedObject.LevelObjectInfos;
        if (lvlObj.X == x && lvlObj.Y == y)
            return;

        lvlObj.X = x;
        lvlObj.Y = y;
        mapEditor.Grid.UpdateObject(mapEditor.SelectedObject);
    }


    public void OnPointerClick(PointerEventData e)
    {
        if ((e.position - onDownPosition).magnitude > 5f)
            return;

        if (e.button == PointerEventData.InputButton.Right)
        {
            mapEditor.SelectedTile = null;
            mapEditor.SelectedObject = null;
        }
        else if (e.button == PointerEventData.InputButton.Left)
        {
            if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
            {
                mapEditor.SelectedObject = null;
                border.SetActive(false);

                (int x, int y) = PointerToWorldPosition(e.position);
                mapEditor.Grid.RemoveTileAt(new Vector3Int(x, y, 1));
            }
            else TrySelectObjectAt(e.position);
        }
    }
    private void TrySelectObjectAt(Vector2 position)
    {
        (int x, int y) = PointerToWorldPosition(position);
        if (mapEditor.SelectedTile != null)
        {
            mapEditor.Grid.AddTileAt(mapEditor.SelectedTile, new Vector3Int(x, y, 1), mapEditor.Rotate);
        }
        else if (mapEditor.Grid.TryGetTileAt(x, y, out BaseObject obj))
        {
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                mapEditor.SelectedObject = obj;
                mapEditor.SelectedTile = null;
            }
        }
        else
        {
            mapEditor.GetComponentInChildren<BtnToggleColorObject>().CloseColors();
        }
    }

    private (int, int) PointerToWorldPosition(Vector2 position)
    {
        float distance = -Camera.main.transform.position.z;
        Vector3 world = Camera.main.ScreenToWorldPoint(new(position.x, position.y, distance));

        return (Mathf.RoundToInt(world.x), Mathf.RoundToInt(world.y));
    }
}
