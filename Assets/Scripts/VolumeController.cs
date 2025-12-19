using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider; // 拖入滑块

    void Start()
    {
        // 初始化滑块值
        if (PlayerPrefs.HasKey("volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = savedVolume;
            AudioListener.volume = savedVolume;
        }
        else
        {
            volumeSlider.value = 1f; // 默认最大音量
            AudioListener.volume = 1f;
        }

        // 添加监听事件
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);
    }

    void OnVolumeChange(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value); // 保存音量设置
    }
}
