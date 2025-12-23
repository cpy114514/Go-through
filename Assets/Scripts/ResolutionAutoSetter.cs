using UnityEngine;

public class ResolutionAutoSetter : MonoBehaviour
{
    public bool startFullscreen = true;

    void Start()
    {
        SetBestResolution();
    }

    void SetBestResolution()
    {
        // 获取当前屏幕分辨率
        Resolution r = Screen.currentResolution;

        // 设置分辨率
        Screen.SetResolution(
            r.width,
            r.height,
            startFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed
        );

        Debug.Log($"Resolution set to {r.width}x{r.height}, fullscreen={startFullscreen}");
    }
}
