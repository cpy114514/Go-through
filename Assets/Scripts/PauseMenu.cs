using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;   // 暂停界面
    public GameObject settingsUI;    // 设置界面
    private bool isPaused = false;   // 是否暂停

    void Update()
    {
        // 按 ESC 打开或关闭暂停菜单
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    // ? 恢复游戏
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // ? 暂停游戏
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        settingsUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // ?? 打开设置界面
    public void OpenSettings()
    {
        settingsUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    // ?? 从设置返回暂停菜单
    public void BackToPauseMenu()
    {
        settingsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    // ?? 返回主菜单
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
