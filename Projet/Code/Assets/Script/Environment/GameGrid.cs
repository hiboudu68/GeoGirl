using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public delegate void BaseObjectEvent(BaseObject subject);
    public delegate void EmptyEvent();
    public event BaseObjectEvent NewObject;
    public event BaseObjectEvent ObjectChanged;
    public event BaseObjectEvent ObjectRemoved;
    public event EmptyEvent Cleared;

    private Level currentLevel;
    public bool SaveEnabled = true;
    private bool _isPlaying = false;
    private GameObject startSprite;
    private List<BaseObject> objects = new();
    private List<LevelObject> pendingObjects = new();
    public GameObject PlayerPrefab;
    public GameObject AIPrefab;
    public GameObject AITrainerPrefab;
    public float cellSize = 50f;
    [HideInInspector]
    public byte[] musicBytes;
    [HideInInspector]
    public Color PrimaryColor = Color.red;
    [HideInInspector]
    public Color SecondaryColor = Color.white;
    public bool IsPlaying { get => _isPlaying; }
    public Level CurrentLevel { get => currentLevel; }

    public BaseObject[] Objects { get => objects.ToArray(); }

    void Awake()
    {
        Application.runInBackground = true;
        FilePath.SetBasePath(Application.persistentDataPath);
        Player.StartPlaying += OnStartPlaying;
        Player.StopPlaying += OnStopPlaying;
    }
    void RunForAI()
    {
        FindAnyObjectByType<MainMenuController>().Hide();

        Level[] levels = LevelsManager.GetEditableLevels();
        LoadLevel(levels[4]);
        Player.Play(AITrainerPrefab);
    }
    void Start()
    {
        startSprite = new GameObject("StartupSprite");
        startSprite.transform.localScale = Vector3.one * 0.8f;
        SpriteRenderer sprite = startSprite.AddComponent<SpriteRenderer>();
        SkinData skin = PlayerSkinPreferences.GetSkin(1);
        sprite.sprite = skin.Icon;
        sprite.material = new Material(TilesManager.Instance.SpriteMaterial);
        sprite.material.SetColor("_ColorA", PlayerSkinPreferences.GetPrimaryColor(1));
        sprite.material.SetColor("_ColorB", PlayerSkinPreferences.GetSecondaryColor(1));

        GameObject floorDeadzone = new GameObject("FloorDeadzone");
        floorDeadzone.transform.position = Vector3.down * 10f + Vector3.right * 5000f;
        BoxCollider2D collider = floorDeadzone.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(10000f, 2f);

        floorDeadzone.AddComponent<DeadZoneProperty>().VelocityDirection = VelocityDirection.Any;

        //RunForAI();
    }
    void Update()
    {
        if (pendingObjects.Count > 0)
        {
            SaveEnabled = false;
            for (int i = 0; i < pendingObjects.Count && i < 50; i++)
            {
                AddObject(pendingObjects[i]);
            }
            pendingObjects.RemoveRange(0, Math.Min(50, pendingObjects.Count));
            SaveEnabled = true;
        }
    }
    public void HideStartSprite()
    {
        startSprite.SetActive(false);
    }
    public void ShowStartSprite()
    {
        SpriteRenderer sprite = startSprite.GetComponent<SpriteRenderer>();
        SkinData skin = PlayerSkinPreferences.GetSkin(1);
        sprite.sprite = skin.Icon;
        sprite.material = new Material(TilesManager.Instance.SpriteMaterial);
        sprite.material.SetColor("_ColorA", PlayerSkinPreferences.GetPrimaryColor(1));
        sprite.material.SetColor("_ColorB", PlayerSkinPreferences.GetSecondaryColor(1));
        startSprite.SetActive(true);
    }
    public float GetLevelProgression(float posX)
    {
        VictoryTrigger victoryTrigger = FindAnyObjectByType<VictoryTrigger>();
        if (victoryTrigger == null)
            return 0f;

        return Math.Min(100, Mathf.Round(100f * posX / victoryTrigger.transform.position.x));
    }
    public void LoadLevel(Level levelData)
    {
        if (Player.Instance != null)
            Player.Instance.Destroy();

        if (currentLevel != null && currentLevel.Id == levelData.Id)
            return;

        currentLevel = levelData;
        Clear();
        ThreadPool.QueueUserWorkItem((object s) =>
        {
            string path = FilePath.GetLevelPath(levelData);
            if (File.Exists(path))
            {
                LevelReader reader = new LevelReader(path);
                pendingObjects = reader.objects;
                if (reader.MusicBytes != null && reader.MusicBytes.Length > 0)
                    musicBytes = reader.MusicBytes;
                else musicBytes = null;
            }

            LevelStatistics stats = PlayerStats.GetLevelStats(levelData.Id);
            Statistics.CurrentLevelTryCount = stats == null ? 0 : stats.TryCount;
        });
    }
    public void Clear()
    {
        foreach (Player player in FindObjectsByType<Player>(FindObjectsSortMode.None))
            Destroy(player.gameObject);

        if (objects.Count == 0)
            return;

        bool wasSaving = SaveEnabled;
        Camera.main.transform.position = Vector3.zero;

        SaveEnabled = false;
        if (MusicLoader.Instance != null)
            MusicLoader.Instance.StopMusic();

        musicBytes = null;
        while (objects.Count > 0)
            RemoveObject(objects[0]);
        SaveEnabled = wasSaving;

        if (Player.Instance != null)
            Player.Instance.Destroy();

        Cleared?.Invoke();
    }
    public void Close()
    {
        Clear();
        currentLevel = null;
    }
    public void OnStartPlaying()
    {
        if (MusicLoader.Instance == null)
            return;

        _isPlaying = true;
        Camera.main.transform.position = Vector2.zero;
        startSprite.SetActive(false);

        Debug.Log("StartPlaying...");
        if (musicBytes != null)
        {
            File.WriteAllBytes(FilePath.GetMusicTempPath(), musicBytes);
            Debug.Log("Reading music " + FilePath.GetMusicTempPath() + " (" + musicBytes.Length + " bytes)");
            MusicLoader.Instance.LoadAudioFromFile(FilePath.GetMusicTempPath());
        }
        else MusicLoader.Instance.StopDefaultMusic();
    }
    public void OnStopPlaying()
    {
        _isPlaying = false;
        Camera.main.transform.position = Vector2.zero;
        MusicLoader.Instance.StopMusic();
    }
    public void UpdateObject(BaseObject obj)
    {
        if (!objects.Contains(obj))
            return;

        obj.Render();

        if (SaveEnabled)
            ObjectChanged?.Invoke(obj);
    }
    public void AddObject(LevelObject levelObj)
    {
        if (objects.Find(x => x.LevelObjectInfos.X == levelObj.X && x.LevelObjectInfos.Y == levelObj.Y) != null)
            return;

        TileData tile = TilesManager.Instance.GetTile(levelObj.TileID);
        if (tile == null)
            return;

        GameObject obj = new();
        BaseObject baseObj = obj.AddComponent<BaseObject>();
        baseObj.TileInfos = tile;
        baseObj.LevelObjectInfos = levelObj;
        objects.Add(baseObj);
    }
    public void RemoveTileAt(Vector3Int worldPosition)
    {
        objects.FindAll(x => x.LevelObjectInfos.X == worldPosition.x && x.LevelObjectInfos.Y == worldPosition.y)
            .ForEach(obj =>
            {
                objects.Remove(obj);
                if (SaveEnabled)
                    ObjectRemoved?.Invoke(obj);

                Destroy(obj.gameObject);
            });
    }
    public void RemoveObject(BaseObject obj)
    {
        objects.Remove(obj);
        if (SaveEnabled)
            ObjectRemoved?.Invoke(obj);

        Destroy(obj.gameObject);
    }
    public void AddTileAt(TileData tileData, Vector3Int worldPosition, byte rotation)
    {
        if (TryGetTileAt(worldPosition.x, worldPosition.y, out BaseObject tileObj))
        {
            bool shouldRender = false;
            if (tileObj.TileInfos != tileData)
            {
                shouldRender = true;
                tileObj.TileInfos = tileData;
            }
            if (tileObj.LevelObjectInfos.Rotation != rotation)
            {
                shouldRender = true;
                tileObj.LevelObjectInfos.Rotation = rotation;
            }
            if (tileObj.LevelObjectInfos.PrimaryColor != PrimaryColor)
            {
                shouldRender = true;
                tileObj.LevelObjectInfos.PrimaryColor = PrimaryColor;
            }
            if (tileObj.LevelObjectInfos.SecondaryColor != SecondaryColor)
            {
                shouldRender = true;
                tileObj.LevelObjectInfos.SecondaryColor = SecondaryColor;
            }

            if (shouldRender)
                tileObj.Render();

            if (SaveEnabled)
                ObjectChanged?.Invoke(tileObj);
        }
        else
        {
            GameObject obj = new();
            BaseObject gameObj = obj.AddComponent<BaseObject>();
            gameObj.TileInfos = tileData;
            gameObj.LevelObjectInfos = new LevelObject()
            {
                X = worldPosition.x,
                Y = worldPosition.y,
                Z = worldPosition.z,
                Rotation = rotation,
                PrimaryColor = PrimaryColor,
                SecondaryColor = SecondaryColor,
                TileID = tileData.Id
            };

            objects.Add(gameObj);
            if (SaveEnabled)
                NewObject?.Invoke(gameObj);
        }
    }
    public bool TryGetTileAt(int x, int y, out BaseObject tileObj)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].LevelObjectInfos.X == x && objects[i].LevelObjectInfos.Y == y)
            {
                tileObj = objects[i];
                return true;
            }
        }

        tileObj = null;
        return false;
    }
}
