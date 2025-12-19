using UnityEngine;

public class AchievementUnlocker : MonoBehaviour
{
    [Header("Achievement")]
    public string achievementId;

    [Tooltip("是否只解锁一次")]
    public bool unlockOnce = true;

    private bool unlocked = false;

    public void Unlock()
    {
        if (unlockOnce && unlocked) return;

        if (string.IsNullOrEmpty(achievementId)) return;

        AchievementManager.Instance.Unlock(achievementId);
        unlocked = true;
    }
}
