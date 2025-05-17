using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using static GameGrid;

public class MapEditorManager : BaseMenu
{
    private static string persistentPath;
    public delegate void ActivityEvent();
    public delegate void TileEvent(TileData tileData);
    public event BaseObjectEvent SelectedObjectChanged;
    public event TileEvent SelectedTileChanged;
    public event ActivityEvent OnExit;
    public event ActivityEvent OnEdit;

    private bool displayGrid = true;
    private GameObject gridObj;
    private GameGrid gameGrid;
    private BaseObject _selectedObj;
    private TileData _selectedTile;
    private int saveRequestedCount = 0;

    public Sprite SelectionFrameSprite;

    [HideInInspector]
    public byte Rotate = 0;

    public GameGrid Grid { get => gameGrid; }

    [HideInInspector]
    public BaseObject SelectedObject
    {
        get => _selectedObj;
        set
        {
            if (_selectedObj == value)
                return;

            SelectedTile = null;
            _selectedObj = value;
            if (_selectedObj != null)
                Rotate = _selectedObj.LevelObjectInfos.Rotation;

            SelectedObjectChanged?.Invoke(_selectedObj);
        }
    }
    [HideInInspector]
    public TileData SelectedTile
    {
        get => _selectedTile;
        set
        {
            if (_selectedTile == value)
            {
                return;
            }

            SelectedObject = null;
            _selectedTile = value;
            SelectedTileChanged?.Invoke(_selectedTile);
        }
    }

    void Awake()
    {
        persistentPath = Application.persistentDataPath;
        saveRequestedCount = 0;
        gameGrid = FindAnyObjectByType<GameGrid>();
        gameGrid.NewObject += OnSaveRequest;
        gameGrid.ObjectChanged += OnSaveRequest;
        gameGrid.ObjectRemoved += OnSaveRequest;
        Player.StartPlaying += OnStartPlaying;
    }
    void Start()
    {
        gridObj = GetComponentInChildren<UIGrid>(true).gameObject;
    }
    private void OnStartPlaying()
    {
        GetComponent<UIToggler>().IsVisible = false;
    }
    public MapEditorManager Edit(Level levelData)
    {
        gameGrid.LoadLevel(levelData);
        OnEdit?.Invoke();

        return this;
    }
    public void DefineMusic(string filePath)
    {
        gameGrid.musicBytes = File.ReadAllBytes(filePath);
        OnSaveRequest(null);
    }
    private void OnSaveRequest(BaseObject subject)
    {
        gameGrid.CurrentLevel.TotalBonusCount = gameGrid.Objects.Count(x => x.GetComponentInChildren<Coin>() != null);

        saveRequestedCount++;
        if (saveRequestedCount == 1)
            ThreadPool.QueueUserWorkItem(Save);
    }
    private void Save(object s)
    {
        Thread.Sleep(500);
        try
        {
            PlayerStats.DeleteLevelStats(gameGrid.CurrentLevel);
            string path = Path.Join(persistentPath, gameGrid.CurrentLevel.Id + ".geomap");
            LevelWriter levelWriter = new(gameGrid.CurrentLevel, path);
            levelWriter.WriteObjs(gameGrid.Objects);
            levelWriter.SetMusicData(gameGrid.musicBytes);
            levelWriter.Close();

            LevelsManager.SaveLevel(gameGrid.CurrentLevel);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        Thread.Sleep(500);
        saveRequestedCount--;
        if (saveRequestedCount > 0)
        {
            saveRequestedCount = 1;
            Save(null);
        }
    }
    public void DisableRaycast()
    {
        foreach (RayCastToggler rayCastToggler in GetComponentsInChildren<RayCastToggler>())
            rayCastToggler.SetValue(false);
    }
    public void EnableRaycast()
    {
        foreach (RayCastToggler rayCastToggler in GetComponentsInChildren<RayCastToggler>())
            rayCastToggler.SetValue(true);
    }
    public override void Hide()
    {
        OnExit?.Invoke();
        GetComponent<UIToggler>().IsVisible = false;
    }

    public override void Show()
    {
        GetComponent<UIToggler>().IsVisible = true;
        GetComponentInChildren<BtnToggleColorObject>().CloseColors();
        gameGrid.ShowStartSprite();
        gameGrid.SaveEnabled = true;
        if (Player.Instance)
            Player.Instance.Destroy();
    }
}
