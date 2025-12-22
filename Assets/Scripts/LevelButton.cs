using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public string levelName;
    public GameObject lockIcon;
    public Button button;

    void Start()
    {
        bool unlocked = PlayerProgress.Instance.IsLevelCleared(levelName);

        button.interactable = unlocked;
        lockIcon.SetActive(!unlocked);
    }
}
