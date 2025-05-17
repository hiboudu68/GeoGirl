using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void PlayDelegate();
    public static event PlayDelegate StartPlaying;
    public static event PlayDelegate StopPlaying;

    public static event PlayDelegate OnDie;
    public static event PlayDelegate OnWin;

    public static Player Instance;

    public float TimeScale = 1.25f;
    private Rigidbody2D rb;
    private ParticleSystem sparks;
    private float spawnTime;

    private PlayerController currentPrefabMode;
    public PlayerController defaultMode;
    public PlayerController currentMode;
    public LayerMask GroundMask;

    public float LifeTime { get => Time.time - spawnTime; }

    public static void Play(GameObject prefab)
    {
        Statistics.CurrentLevelCoinsCount = 0;
        Statistics.CurrentLevelJumpCount = 0;

        GameObject player = Instantiate(prefab);
        player.transform.position = Vector3.zero;
        player.transform.localScale = Vector3.one * 0.8f;

        Camera.main.GetComponent<CameraController>().Player = player.GetComponent<Player>();
        Camera.main.GetComponent<CameraController>().Player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        StartPlaying?.Invoke();
    }
    public static void Die()
    {
        OnDie?.Invoke();
    }
    public static void Win()
    {
        Debug.Log("Win !");
        OnWin?.Invoke();
    }
    void Awake()
    {
        Instance = this;
        spawnTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        sparks = GetComponentInChildren<ParticleSystem>();
        sparks.transform.parent = null;
        sparks.Stop();

        OnWin += OnVictory;

        Restart();
    }
    private void OnVictory()
    {
        if (currentMode != null)
        {
            currentMode.enabled = false;
            Destroy(currentMode);
        }

        if (gameObject != null)
            currentMode = gameObject.AddComponent<WinMode>();

        OnWin -= OnVictory;
    }
    void Update()
    {
        if (sparks.isPlaying)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * (rb.gravityScale < 0 ? 1 : -1f), 1f, GroundMask);
            if (hit.collider != null)
                sparks.transform.position = hit.point + (rb.gravityScale < 0 ? Vector2.up : Vector2.down) * -0.25f;
        }
    }
    public bool IsGrounded => Physics2D.Raycast(transform.position, Vector2.up * (rb.gravityScale < 0 ? 1 : -1f), 1f, GroundMask).collider != null;
    public void Destroy()
    {
        Debug.Log("Destroying...");
        if (Player.Instance == null)
            return;

        Player.Instance = null;
        currentPrefabMode = null;

        Destroy(gameObject);
        OnWin -= OnVictory;
        StopPlaying?.Invoke();
    }
    public void Restart()
    {
        if (currentMode is WinMode)
            return;
        Debug.Log("Restarted");
        Statistics.CurrentLevelCoinsCount = 0;
        Statistics.CurrentLevelJumpCount = 0;
        spawnTime = Time.time;

        transform.SetPositionAndRotation(Vector2.zero, Quaternion.identity);
        transform.localScale = Vector3.one * 0.8f;
        rb.linearVelocity = Vector2.zero;
        spawnTime = Time.time;

        currentPrefabMode = null;
        Camera.main.GetComponent<CameraController>().ResetPosition();
        if (MusicLoader.Instance != null)
            MusicLoader.Instance.RestartMusic();

        SetControlMode(defaultMode);

        foreach (Delegate del in OnWin.GetInvocationList())
        {
            if (del.Method.Name == "OnVictory" && del.Target == this)
                return;
        }

        OnWin += OnVictory;
    }

    public void SetControlMode(PlayerController prefabMode)
    {
        if (currentPrefabMode == prefabMode)
            return;

        currentPrefabMode = prefabMode;
        if (currentMode != null)
            Destroy(currentMode.gameObject);

        if (prefabMode == null) return;

        currentMode = GameObject.Instantiate(prefabMode.gameObject).GetComponent<PlayerController>();
        currentMode.transform.SetParent(transform);
        currentMode.transform.localPosition = Vector3.zero;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        sparks.Play();
    }
    void OnCollisionExit2D(Collision2D collider)
    {
        sparks.Stop();
    }
}
