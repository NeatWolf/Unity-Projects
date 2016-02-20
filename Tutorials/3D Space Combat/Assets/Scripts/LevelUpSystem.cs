using UnityEngine;
using System.Collections;

public class LevelUpSystem : MonoBehaviour {

    private int currentXP = 0;
    private int nextLevelUp = 50;
    private int level = 1;

    public void GainExperience(int amount)
    {
        currentXP += amount;
        print(string.Format("Current XP: {0}", currentXP));
        if (currentXP >= nextLevelUp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        print("Level Up!");
        nextLevelUp += 20;
        level += 1;
    }
}
