using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class SceneMusicGroup
{
    public AudioClip music;
    public string[] sceneNames;
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music Groups")]
    public List<SceneMusicGroup> musicGroups = new List<SceneMusicGroup>();

    private AudioSource audioSource;
    private AudioClip currentClip;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip clip = GetMusicForScene(scene.name);
        PlayMusic(clip);
    }

    AudioClip GetMusicForScene(string sceneName)
    {
        foreach (var group in musicGroups)
        {
            foreach (var s in group.sceneNames)
            {
                if (s == sceneName)
                    return group.music;
            }
        }

        return null;
    }

    void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (clip == currentClip) return; // ⭐ 同一首歌不重播

        currentClip = clip;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
