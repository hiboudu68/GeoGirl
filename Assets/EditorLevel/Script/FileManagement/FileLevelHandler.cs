using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class FileLevelHandler : MonoBehaviour
{
    string pathFileTemp;
    //[SerializeField] Dictionary<TileBase, GameObject> linkTilePrefab = new();
    public List<TileBase> tilemaps = new();
    public List<GameObject> prefabs = new();
    public GameObject objectsContainer;
    public GameObject playerContainer;
    PlayerInput playerInput;
    public GameObject pause;
    
    private void Awake() {
        playerInput = new PlayerInput();
        pathFileTemp = FileHandler.GetPath("tilemapTemp.json");
    }

    private void Start()
    {
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(pathFileTemp);
        Tilemap map = FindObjectOfType<Tilemap>();

        //with the position put the good object in this position
        foreach (var mapData in data)
        {
            if (mapData.tiles == null || mapData.tiles.Count == 0)
            {
                continue;
            }

            foreach (TileInfo tile in mapData.tiles)
            {
                TileBase tileBase = tile.tileBase;
                if (tileBase == null)
                {
                    string path = AssetDatabase.GUIDToAssetPath(tile.guidFromAssetDB);
                    tileBase = AssetDatabase.LoadAssetAtPath<TileBase>(path);

                    if (tileBase == null)
                    {
                        Debug.Log("Tile not found in AssetDatabase");
                        continue;
                    }
                }

                int index = tilemaps.FindIndex(tm => tm.name == tileBase.name);
                if (index > -1 && index < tilemaps.Count) //tiles.TryGetValue(tileBase, out GameObject val)
                {
                    Vector3 vector = map.CellToWorld(tile.position);
                    GameObject newGameObj = Instantiate(prefabs[index], vector, Quaternion.identity);
                    
                    if (index == 3) {
                        newGameObj.transform.parent = playerContainer.transform;
                        playerContainer.transform.position = vector;
                    }
                    else{
                        newGameObj.transform.parent = objectsContainer.transform;
                    }
                }
                else
                {
                    Debug.Log("TileBase: " + tileBase.name + " ,is not in the dictionnary");
                    continue;
                }
            }
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Editor.Esc.performed += OnEscClick;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Editor.Esc.performed -= OnEscClick;
    }
    
    private void OnEscClick(InputAction.CallbackContext ctx) {
        Time.timeScale = 0;
        pause.SetActive(true);
    }

    public void Continue() {
        Time.timeScale = 1;
        pause.SetActive(false);
    }

}
