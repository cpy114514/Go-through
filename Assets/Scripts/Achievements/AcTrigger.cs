using UnityEngine;

public class AchievementTrigger : MonoBehaviour
{
    private AchievementUnlocker unlocker;

    void Awake()
    {
        unlocker = GetComponent<AchievementUnlocker>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        unlocker.Unlock();
    }
}
