using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        string levelName = SceneManager.GetActiveScene().name;
        PlayerProgress.Instance.ClearLevel(levelName);

        Debug.Log("WIN: " + levelName);
    }
}
