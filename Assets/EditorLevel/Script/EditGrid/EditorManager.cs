using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using SimpleFileBrowser;
using System.IO;
using System.Linq;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.SceneManagement;

public class EditorManager : MonoBehaviour
{
    PlayerInput playerInput;
    Vector3 origin;
    Vector3 dif;
    Camera mainCamera;
    bool isDragging;
    Dictionary<string, Tilemap> tilemaps = new();
    string pathFileTemp;
    public List<TileBase> tileBases = new();

    private void Awake() {
        playerInput = new PlayerInput();
        mainCamera = Camera.main;
        pathFileTemp = FileHandler.GetPath("tilemapTemp.json");
    }

    private void Start()
    {
        InitTilemaps();

        if (ModifEditorHandler.IsModif) {
            LoadFile(pathFileTemp);
        }
    }

    private void InitTilemaps()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();

        foreach (Tilemap map in maps) {
            tilemaps.Add(map.name, map);
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Editor.DragCamera.started += OnDrag;
        playerInput.Editor.DragCamera.performed += OnDrag;
        playerInput.Editor.DragCamera.canceled += OnDrag;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Editor.DragCamera.started -= OnDrag;
        playerInput.Editor.DragCamera.performed -= OnDrag;
        playerInput.Editor.DragCamera.canceled -= OnDrag;
    }

    public void OnDrag(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            origin = GetMousePosition;
        }

        isDragging = ctx.started || ctx.performed;
    }

    private void LateUpdate() {
        if (!isDragging) return;
        dif = GetMousePosition - mainCamera.transform.position;
        Vector3 newPosition = origin - dif;
        if (newPosition.x > -200 && newPosition.y < 225 && newPosition.x < 200 && newPosition.y > -225) {
            mainCamera.transform.position = newPosition;
        }
    }

    private Vector3 GetMousePosition => mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

    private void Update() {
        if (mainCamera.orthographic) {
            var zoomValue = Input.GetAxis("Mouse ScrollWheel") * 10;
            if (mainCamera.orthographicSize - zoomValue < 21 && mainCamera.orthographicSize - zoomValue > 3){
                mainCamera.orthographicSize -= zoomValue;
            }        
        }
    }

    public void ImportTxt() {
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "*", ".txt" ) );
		FileBrowser.ShowLoadDialog( OnSuccess, () => { Debug.Log( "Canceled" ); },
								   FileBrowser.PickMode.Files, false, null, null, "Select Folder", "Select" );
    }

    private void OnSuccess(string[] paths) {
        List<TilemapData> data = new();
        TilemapData mapData = new()
        {
            key = "Default"
        };

        List<string> lines = File.ReadAllLines(paths[0]).ToList();
        int numLine = 0;
        foreach(string line in lines) {
            for(int i = 0; i < line.Length; i++){
                TileBase tile = null;
                switch(line[i]) {
                    case '#':
                        tile = tileBases[0];
                        break;
                    case '<':
                        tile = tileBases[1];
                        break;
                    case '>':
                        tile = tileBases[1];
                        break;
                    case '^':
                        tile = tileBases[1];
                        break;
                    case 'v':
                        tile = tileBases[1];
                        break;
                    case 'S':
                        tile = tileBases[2];
                        break;
                    case 'E':
                        tile = tileBases[3];
                        break;
                    default:
                        break; 
                }


                if (tile != null) {
                    if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(tile, out string guid, out long localId))
                    {
                        TileInfo ti = new(tile, new(i, numLine, 0), guid);
                        mapData.tiles.Add(ti);
                    }
                    else {
                        Debug.Log("Could not get guid for tile :" + tile.name);
                    }
                }
            }
            numLine--;
        }
        data.Add(mapData);

        FileHandler.SaveToJSON<TilemapData>(data, pathFileTemp);
        
        SceneManager.LoadScene("EditorPlay");
    }

    public void TestLevelEditor()
    {
        SaveFile(pathFileTemp);
        SceneManager.LoadScene("EditorPlay");
    }

    public void OnSave()
    {
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "*", ".json" ) );
        FileBrowser.ShowSaveDialog( ( paths ) => { SaveFile(paths[0]); }, 
            () => { Debug.Log( "Canceled" ); }, 
            FileBrowser.PickMode.Files, 
            false, null, "GeoGirlLevel.json", "Save As", "Save" );
    }

    public void SaveFile(string path)
    {
        List<TilemapData> data = new();

        foreach (var map in tilemaps)
        {
            TilemapData mapData = new()
            {
                key = map.Key
            };

            BoundsInt boundsMap = map.Value.cellBounds;
            for (int x = boundsMap.xMin; x < boundsMap.xMax; x++) {
                for (int y = boundsMap.yMin; y < boundsMap.yMax; y++) {
                    Vector3Int pos = new(x, y, 0);
                    TileBase tile = map.Value.GetTile(pos);

                    if (tile != null) {
                        if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(tile, out string guid, out long localId))
                        {
                            TileInfo ti = new(tile, pos, guid);
                            mapData.tiles.Add(ti);
                        }
                        else {
                            Debug.Log("Could not get guid for tile :" + tile.name);
                        }
                    }
                }
            } 

            data.Add(mapData);
        }

        FileHandler.SaveToJSON<TilemapData>(data, path);
    }

    public void OnLoad()
    {
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "*", ".json" ) );
        FileBrowser.ShowLoadDialog( ( paths ) => { LoadFile(paths[0]); }, 
            () => { Debug.Log( "Canceled" ); }, 
            FileBrowser.PickMode.Files, false, null, null, "Select Folder", "Select" );
    }

    public void LoadFile(string path)
    {
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(path);

        foreach (var mapData in data) {
            if (!tilemaps.ContainsKey(mapData.key)) {
                Debug.Log("This \"" + mapData.key + "\" TileMap doesn't exist");
                continue;
            }

            var map = tilemaps[mapData.key];
            map.ClearAllTiles();
            
            if (mapData.tiles != null && mapData.tiles.Count != 0)
            {
                foreach (TileInfo tile in mapData.tiles) {
                    TileBase tileBase = tile.tileBase;
                    if (tileBase == null)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(tile.guidFromAssetDB);
                        tileBase = AssetDatabase.LoadAssetAtPath<TileBase>(assetPath);
                        
                        if (tileBase == null) {
                            Debug.Log("Tile not found in AssetDatabase");
                            continue;
                        }
                    }

                    map.SetTile(tile.position, tileBase);
                }
            }
        }

    }
}

[Serializable]
public class TilemapData
{
    public string key;
    public List<TileInfo> tiles = new();
}

[Serializable]
public class TileInfo
{
    public TileBase tileBase;
    public string guidFromAssetDB;
    public Vector3Int position;

    public TileInfo(TileBase tile, Vector3Int pos, string guid) { 
        this.tileBase = tile;
        this.guidFromAssetDB = guid;
        this.position = pos;
    }

}
