using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        // 保证场景切换时不销毁
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // 每次场景切换时调用
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        // 尝试加载位于 Resources/Audio 目录下的同名音乐
        AudioClip clipToPlay = Resources.Load<AudioClip>("Audio/" + sceneName + "Music");

        if (clipToPlay != null && audioSource.clip != clipToPlay)
        {
            audioSource.clip = clipToPlay;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
