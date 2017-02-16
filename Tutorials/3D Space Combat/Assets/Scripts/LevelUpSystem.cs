using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Linq;

public class LevelUpSystem : MonoBehaviour {

    [SerializeField]
    private GameObject addXPTextPrefab;
    [SerializeField]
    private GameObject levelUpTextPrefab;
    [SerializeField]
    private RectTransform mainCanvas;
    [SerializeField]
    private UIProgressBarController experienceBar;
    [SerializeField]
    private int nextLevelUp;

    private int _currentXP = 0;
    private int _level = 1;

    void Start()
    {
        experienceBar.SetAlpha(0f);
    }

    public void GainExperience(int amount)
    {
        _currentXP += amount;
        ShowXPPopup(amount);
        StartCoroutine(ShowExperienceBarIncrease());
        print(string.Format("Current XP: {0}", _currentXP));
        if (_currentXP >= nextLevelUp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        Debug.Log("Level Up!");
        _currentXP = 0;
        experienceBar.fillAmount = 0;
        nextLevelUp += nextLevelUp / 2;
        _level += 1;
        ShowLevelUpPopup(_level);
    }

    private void ShowXPPopup(int amount)
    {
        GameObject addXP = Instantiate(addXPTextPrefab);
        Text addXPText = addXP.GetComponentInChildren<Text>();
        if (addXPText != null)
        {
            addXPText.text = string.Format("+{0}", amount.ToString());
        }
        addXP.transform.SetParent(mainCanvas);
        var xpTransform = addXP.GetComponent<RectTransform>();
        xpTransform.anchoredPosition = new Vector2(0f, 0f);
        xpTransform.anchorMax = new Vector2(0.5f, 0.5f);
        xpTransform.anchorMin = new Vector2(0.5f, 0.5f);
    }

    private IEnumerator ShowExperienceBarIncrease()
    {
        experienceBar.FadeIn(0.5f);

        // Increase bar fill smoothly
        experienceBar.fillAmount = (float) _currentXP / (float) nextLevelUp;

        // Wait
        yield return new WaitForSeconds(3);

        experienceBar.FadeOut(0.5f);
    }

    private void ShowLevelUpPopup(int level)
    {
        var levelUp = Instantiate(levelUpTextPrefab);
        var text = levelUp.GetComponentsInChildren<Text>().Where(t => t.name.Equals("LevelNumber"));
        text.FirstOrDefault().text = string.Format("LEVEL {0}", level.ToString());
        levelUp.transform.SetParent(mainCanvas);
        var rectTransform = levelUp.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
    }
}
