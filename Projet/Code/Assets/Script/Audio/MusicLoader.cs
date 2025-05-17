using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class MusicLoader : MonoBehaviour
{
    public static MusicLoader Instance;
    public AudioClip DefaultMusic;

    private AudioSource src;

    void Start()
    {
        src = GetComponent<AudioSource>();
        Instance = this;
        src.volume = 0;
        src.clip = DefaultMusic;
        src.Play();

        Player.StartPlaying += OnStartPlaying;
        Player.StopPlaying += OnStopPlaying;
    }
    private void OnStopPlaying()
    {
        src.Stop();
    }
    private void OnStartPlaying()
    {
        src.Play();
    }
    void Update()
    {
        src.volume = Mathf.Lerp(src.volume, 0.6f, Time.unscaledDeltaTime * 3);
    }
    public void LoadAudioFromFile(string path)
    {
        StartCoroutine(LoadAudio(path));
    }
    public void RestartMusic()
    {
        if (src.clip == DefaultMusic)
            return;

        src.Stop();
        src.Play();
    }
    public void PauseMusic()
    {
        if (src.clip == DefaultMusic)
            return;

        src.Pause();
    }
    public void ResumeMusic()
    {
        if (src.clip == DefaultMusic)
            return;

        src.Play();
    }
    public void StopMusic()
    {
        if (src.clip == DefaultMusic)
            return;

        src.enabled = true;
        src.volume = 0;
        src.clip = DefaultMusic;
        src.PlayScheduled(AudioSettings.dspTime + 1.0f);
    }
    public void StopDefaultMusic()
    {
        src.Stop();
        src.clip = null;
    }
    private IEnumerator LoadAudio(string path)
    {
        if (File.Exists(path))
        {
            src.Stop();
            src.volume = 0;
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                        src.clip = clip;
                        src.Play();
                    }
                    catch { }
                }
            }
        }
    }
}
