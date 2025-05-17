using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public static UIAudio Instance;

    private AudioSource src;
    [SerializeField] private AudioClip Tick;

    void Awake()
    {
        Instance = this;
        src = GetComponent<AudioSource>();
    }
    public void PlayTick()
    {
        src.PlayOneShot(Tick);
    }
}
