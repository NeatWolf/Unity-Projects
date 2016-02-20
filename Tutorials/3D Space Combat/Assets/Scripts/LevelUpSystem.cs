using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LevelUpSystem : MonoBehaviour {

    public Text addXPText;
    public Text levelUpText;
    public int nextLevelUp;

    private int currentXP = 0;
    private int level = 1;

    void Start()
    {
        addXPText.enabled = false;
        levelUpText.enabled = false;
    }

    public void GainExperience(int amount)
    {
        currentXP += amount;
        StartCoroutine(ShowXPPopup(amount));
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
        nextLevelUp += nextLevelUp / 2;
        level += 1;
        StartCoroutine(ShowLevelUpPopup(level));
    }

    private IEnumerator ShowXPPopup(int amount)
    {
        addXPText.text = string.Format("+{0}", amount.ToString());
        addXPText.enabled = true;
        yield return new WaitForSeconds(1.5f);
        addXPText.enabled = false;
    }

    private IEnumerator ShowLevelUpPopup(int level)
    {
        levelUpText.text = string.Format("You reached level {0}", level.ToString());
        levelUpText.enabled = true;
        yield return new WaitForSeconds(5f);
        levelUpText.enabled = false;
    }
}
