using UnityEngine;

[System.Serializable]
public class AchievementData
{
    public string id;           // 唯一ID
    public string title;        // 成就名
    [TextArea]
    public string description;  // 成就描述

    public Sprite icon;         // 图标（以后用）

    [HideInInspector]
    public bool unlocked;       // 是否已解锁
}
