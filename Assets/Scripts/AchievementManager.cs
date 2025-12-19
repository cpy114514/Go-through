using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [Header("All Achievements")]
    public AchievementData[] achievements;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // ⭐ 解锁成就（对外接口）
    // =========================
    public void Unlock(string id)
    {
        Debug.Log("Unlock called: [" + id + "]");

        AchievementData data = GetAchievementDataById(id);
        if (data == null)
        {
            Debug.LogError("Achievement data not found: " + id);
            return;
        }

        // 🔒 已解锁就不再触发（非常重要）
        if (data.unlocked)
        {
            Debug.Log("Achievement already unlocked: " + id);
            return;
        }

        // 标记解锁
        data.unlocked = true;
        Save();

        Debug.Log("ID MATCHED & UNLOCKED!");

        // ⭐⭐⭐ 真正显示弹窗的地方 ⭐⭐⭐
        if (AchievementPopup.Instance != null)
        {
            AchievementPopup.Instance.Show(data);
        }
        else
        {
            Debug.LogError("AchievementPopup.Instance is NULL");
        }
    }

    // =========================
    // 🔍 根据 ID 查找成就
    // =========================
    AchievementData GetAchievementDataById(string id)
    {
        foreach (var a in achievements)
        {
            if (a.id == id)
                return a;
        }
        return null;
    }

    // =========================
    // 💾 存档 / 读档
    // =========================
    void Save()
    {
        foreach (var a in achievements)
        {
            PlayerPrefs.SetInt("ACH_" + a.id, a.unlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    void Load()
    {
        foreach (var a in achievements)
        {
            a.unlocked = PlayerPrefs.GetInt("ACH_" + a.id, 0) == 1;
        }
    }

    public bool IsUnlocked(string id)
    {
        foreach (var a in achievements)
        {
            if (a.id == id)
                return a.unlocked;
        }
        return false;
    }
}
