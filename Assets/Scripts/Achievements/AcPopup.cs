using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AchievementPopup : MonoBehaviour
{
    public static AchievementPopup Instance;

    [Header("UI")]
    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image iconImage;

    [Header("Settings")]
    public float showTime = 2.5f;

    private Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;
        Debug.Log("AchievementPopup Awake");
        panel.SetActive(false);
    }



    public void Show(AchievementData data)
    {
        Debug.Log("AchievementPopup.Show() CALLED");

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        titleText.text = data.title;
        descriptionText.text = data.description;

        if (iconImage != null && data.icon != null)
        {
            iconImage.sprite = data.icon;
            iconImage.gameObject.SetActive(true);
        }
        else if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
        }

        panel.SetActive(true);
        currentRoutine = StartCoroutine(HideAfterDelay());
    }


    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showTime);
        panel.SetActive(false);
    }
}
