using UnityEngine;

public class AchievementListUI : MonoBehaviour
{
    public Transform content;
    public GameObject achievementItemPrefab;

    void OnEnable()
    {
        Refresh(); // ⭐ 打开页面时自动刷新
    }

    public void Refresh()
    {
        // 清空旧内容
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        var manager = AchievementManager.Instance;
        if (manager == null) return;

        foreach (var data in manager.achievements)
        {
            GameObject item = Instantiate(achievementItemPrefab, content);
            item.GetComponent<AchievementItemUI>().Setup(data);
        }
    }

    // 防止你之前按钮绑了 Rebuild
    public void Rebuild()
    {
        Refresh();
    }
}
