using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject levelSelectPanel;
    public GameObject settingPanel;
    public GameObject achievementPanel;

    [Header("Main Menu Buttons")]
    public GameObject startButton;
    public GameObject settingButton;
    public GameObject quitButton;
    public GameObject achievementButton;

    // ===========================
    // 通用：进入任意子界面
    void EnterPanel(GameObject panel)
    {
        CloseAllPanels();
        panel.SetActive(true);
        ToggleMainButtons(false);
    }

    // 通用：返回主菜单
    void ExitPanel(GameObject panel)
    {
        panel.SetActive(false);
        ToggleMainButtons(true);
    }

    // 关闭所有面板（防止叠加）
    void CloseAllPanels()
    {
        if (levelSelectPanel) levelSelectPanel.SetActive(false);
        if (settingPanel) settingPanel.SetActive(false);
        if (achievementPanel) achievementPanel.SetActive(false);
    }

    // ===========================
    // 关卡选择
    public void ShowLevelSelect() => EnterPanel(levelSelectPanel);
    public void HideLevelSelect() => ExitPanel(levelSelectPanel);

    // 设置
    public void ShowSetting() => EnterPanel(settingPanel);
    public void HideSetting() => ExitPanel(settingPanel);

    // 成就
    public void ShowAchievement() => EnterPanel(achievementPanel);
    public void HideAchievement() => ExitPanel(achievementPanel);

    // ===========================
    // 加载关卡
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    // 退出游戏
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    // 成就解锁（按钮 / 调试用）
    public void UnlockAchievement(string id)
    {
        if (AchievementManager.Instance != null)
            AchievementManager.Instance.Unlock(id);
        else
            Debug.LogError("AchievementManager.Instance is NULL");
    }

    // ===========================
    // 主菜单按钮统一显隐
    void ToggleMainButtons(bool state)
    {
        if (startButton) startButton.SetActive(state);
        if (settingButton) settingButton.SetActive(state);
        if (quitButton) quitButton.SetActive(state);
        if (achievementButton) achievementButton.SetActive(state);
    }
}
