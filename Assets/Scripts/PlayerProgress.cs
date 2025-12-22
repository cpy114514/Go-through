using UnityEngine;
using System.Collections.Generic;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance;

    // 已通关的关卡名
    private HashSet<string> clearedLevels = new HashSet<string>();

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

    // ===== 通关 =====
    public void ClearLevel(string levelName)
    {
        if (clearedLevels.Contains(levelName))
            return;

        clearedLevels.Add(levelName);
        Save();

        Debug.Log("Level cleared: " + levelName);
    }

    // ===== 查询 =====
    public bool IsLevelCleared(string levelName)
    {
        return clearedLevels.Contains(levelName);
    }

    // ===== 存档 =====
    void Save()
    {
        PlayerPrefs.SetString("CLEARED_LEVELS",
            string.Join(",", clearedLevels));
        PlayerPrefs.Save();
    }

    void Load()
    {
        clearedLevels.Clear();

        string data = PlayerPrefs.GetString("CLEARED_LEVELS", "");
        if (string.IsNullOrEmpty(data)) return;

        foreach (var level in data.Split(','))
            clearedLevels.Add(level);
    }
}
