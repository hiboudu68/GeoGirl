using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Tilemaps;

public class BuildingCreator : Singleton<BuildingCreator>
{
    [SerializeField] Tilemap previewMap, defaultMap;
    PlayerInput playerInput;
    TileBase tileBase;
    BuildingObjectsBase selectedObj;
    Camera _camera;
    Vector2 mousePos;
    Vector3Int currentGridPosition;
    Vector3Int lastGridPosition;
    bool holdActive;
    Vector3Int holdStartPosition;
    BoundsInt bounds;

    protected override void Awake()
    {
        base.Awake();
        playerInput = new PlayerInput();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Editor.MousePosition.performed += OnMouseMove;
        playerInput.Editor.MouseLeftClick.performed += OnLeftClick;
        playerInput.Editor.MouseLeftClick.started += OnLeftClick;
        playerInput.Editor.MouseLeftClick.canceled += OnLeftClick;

        playerInput.Editor.MouseRightClick.performed += OnRigthClick;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Editor.MousePosition.performed -= OnMouseMove;
        playerInput.Editor.MouseLeftClick.performed -= OnLeftClick;
        playerInput.Editor.MouseLeftClick.started -= OnLeftClick;
        playerInput.Editor.MouseLeftClick.canceled -= OnLeftClick;

        playerInput.Editor.MouseRightClick.performed -= OnRigthClick;
    }

    private void Update()
    {
        if (selectedObj != null)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(mousePos);
            Vector3Int gridPos = previewMap.WorldToCell(pos);

            if (gridPos != currentGridPosition)
            {
                lastGridPosition = currentGridPosition;
                currentGridPosition = gridPos;

                UpdatePreview();
                if (holdActive) {
                    HandleDrawing();
                }
            }
        }
    }

    private BuildingObjectsBase SelectedObj
    {
        set
        {
            selectedObj = value;
            tileBase = selectedObj != null ? selectedObj.TileBase : null;
            UpdatePreview();
        }
    }

    private void OnMouseMove(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
    }

    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (selectedObj != null && !EventSystem.current.IsPointerOverGameObject())
        {

            if (ctx.phase == InputActionPhase.Started)
            {
                holdActive = true;
                if (ctx.interaction is TapInteraction)
                {
                    holdStartPosition = currentGridPosition;
                }
                HandleDrawing();
            }
            else
            {
                if (ctx.interaction is SlowTapInteraction || ctx.interaction is TapInteraction && ctx.phase == InputActionPhase.Performed)
                {
                    holdActive = false;
                    HandleDrawRelease();
                }
            }
        }
    }

    private void OnRigthClick(InputAction.CallbackContext ctx)
    {
        holdActive = false;
        previewMap.ClearAllTiles();
        SelectedObj = null;
    }

    public void ObjectSelected(BuildingObjectsBase obj)
    {
        SelectedObj = obj;
    }

    private void UpdatePreview()
    {
        previewMap.SetTile(lastGridPosition, null);
        previewMap.SetTile(currentGridPosition, tileBase);
    }

    private void HandleDrawing()
    {
        if (selectedObj != null)
        {
            switch (selectedObj.PlaceType)
            {
                case PlaceType.Single:
                default:
                    DrawItem();
                    break;
                case PlaceType.Rectangle:
                    RectangleRenderer();
                    break;
            }
        }
    }

    private void HandleDrawRelease()
    {
        if (selectedObj != null)
        {
            switch (selectedObj.PlaceType)
            {
                case PlaceType.Rectangle:
                    DrawBounds(defaultMap);
                    previewMap.ClearAllTiles();
                    break;
            }
        }
    }

    private void RectangleRenderer()
    {
        previewMap.ClearAllTiles();

        bounds.xMin = currentGridPosition.x < holdStartPosition.x ? currentGridPosition.x : holdStartPosition.x;
        bounds.xMax = currentGridPosition.x > holdStartPosition.x ? currentGridPosition.x : holdStartPosition.x;
        bounds.yMin = currentGridPosition.y < holdStartPosition.y ? currentGridPosition.y : holdStartPosition.y;
        bounds.yMax = currentGridPosition.y > holdStartPosition.y ? currentGridPosition.y : holdStartPosition.y;

        DrawBounds(previewMap);
    }

    private void DrawBounds(Tilemap tilemap)
    {
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                tilemap.SetTile(new Vector3Int(x,y,0), tileBase);
            }
        }
    }

    private void DrawItem()
    {
        defaultMap.SetTile(currentGridPosition, tileBase);
    }
}
