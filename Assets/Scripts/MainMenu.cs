using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject levelSelectPanel;
    public GameObject settingPanel;

    [Header("Main Menu Buttons")]
    public GameObject startButton;
    public GameObject settingButton;
    public GameObject quitButton;

    // ---------------------------
    // 显示关卡选择面板
    public void ShowLevelSelect()
    {
        levelSelectPanel.SetActive(true);
        ToggleMainButtons(false); // 隐藏主菜单按钮
    }

    // 关闭关卡选择面板
    public void HideLevelSelect()
    {
        levelSelectPanel.SetActive(false);
        ToggleMainButtons(true); // 显示主菜单按钮
    }

    // ---------------------------
    // 显示设置面板
    public void ShowSetting()
    {
        settingPanel.SetActive(true);
        ToggleMainButtons(false); // 隐藏主菜单按钮
    }

    // 关闭设置面板
    public void HideSetting()
    {
        settingPanel.SetActive(false);
        ToggleMainButtons(true); // 显示主菜单按钮
    }

    // ---------------------------
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

    // ---------------------------
    // 控制主菜单按钮显示/隐藏
    void ToggleMainButtons(bool state)
    {
        if (startButton != null) startButton.SetActive(state);
        if (settingButton != null) settingButton.SetActive(state);
        if (quitButton != null) quitButton.SetActive(state);
    }
}
