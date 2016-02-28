using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LevelUpSystem : MonoBehaviour {

    public Text addXPText;
    public Text levelUpText;
    public UIProgressBarController experienceBar;
    public int nextLevelUp;

    private int currentXP = 0;
    private int level = 1;

    void Start()
    {
        addXPText.canvasRenderer.SetAlpha(0f);
        levelUpText.canvasRenderer.SetAlpha(0f);
        experienceBar.barEmptyImage.canvasRenderer.SetAlpha(0f);
        experienceBar.barFullImage.canvasRenderer.SetAlpha(0f);
    }

    public void GainExperience(int amount)
    {
        currentXP += amount;
        StartCoroutine(ShowXPPopup(amount));
        StartCoroutine(ShowExperienceBarIncrease());
        print(string.Format("Current XP: {0}", currentXP));
        if (currentXP >= nextLevelUp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        print("Level Up!");
        currentXP = 0;
        experienceBar.fillAmount = 0;
        nextLevelUp += nextLevelUp / 2;
        level += 1;
        StartCoroutine(ShowLevelUpPopup(level));
    }

    private IEnumerator ShowXPPopup(int amount)
    {
        addXPText.text = string.Format("+{0}", amount.ToString());
        addXPText.CrossFadeAlpha(1, 0.25f, false);
        yield return new WaitForSeconds(1.5f);
        addXPText.CrossFadeAlpha(0, 0.25f, false);
    }

    private IEnumerator ShowExperienceBarIncrease()
    {
        // Fade in
        experienceBar.barEmptyImage.CrossFadeAlpha(1, 0.5f, false);
        experienceBar.barFullImage.CrossFadeAlpha(1, 0.5f, false);

        // Increase bar fill smoothly
        experienceBar.fillAmount = (float) currentXP / (float) nextLevelUp;

        // Wait
        yield return new WaitForSeconds(3);

        // Fade out
        experienceBar.barEmptyImage.CrossFadeAlpha(0, 0.5f, false);
        experienceBar.barFullImage.CrossFadeAlpha(0, 0.5f, false);
    }

    private IEnumerator ShowLevelUpPopup(int level)
    {
        levelUpText.text = string.Format("You reached level {0}", level.ToString());
        levelUpText.CrossFadeAlpha(1, 0.5f, false);
        yield return new WaitForSeconds(5);
        levelUpText.CrossFadeAlpha(0, 0.5f, false);
    }
}
