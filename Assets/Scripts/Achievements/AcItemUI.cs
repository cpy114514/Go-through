using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementItemUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image lockIcon;

    public void Setup(AchievementData data)
    {
        titleText.text = data.title;
        descriptionText.text = data.description;

        if (icon && data.icon)
            icon.sprite = data.icon;

        if (data.unlocked)
        {
            lockIcon.gameObject.SetActive(false);
            SetAlpha(1f);
        }
        else
        {
            lockIcon.gameObject.SetActive(true);
            SetAlpha(0.4f);
        }
    }

    void SetAlpha(float a)
    {
        titleText.alpha = a;
        descriptionText.alpha = a;

        if (icon)
        {
            Color c = icon.color;
            c.a = a;
            icon.color = c;
        }
    }
}
