using UnityEngine;
using System.Collections;

public class QuestMenu : MonoBehaviour {

    public GameObject questMenuCanvas;

    void Update()
    {
        if (GameManager.instance.isPaused && GameManager.instance.pauseType == GameManager.PauseType.questMenu)
        {
            questMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            GameManager.instance.isPaused = true;
        }
        else if (!GameManager.instance.isPaused)
        {
            questMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
            GameManager.instance.isPaused = false;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            // If game is already paused because of another menu, don't do anything
            if (GameManager.instance.isPaused && GameManager.instance.pauseType != GameManager.PauseType.questMenu)
            {
                return;
            }
            if (GameManager.instance.isPaused)
            {
                GameManager.instance.pauseType = GameManager.PauseType.none;
            }
            else
            {
                GameManager.instance.pauseType = GameManager.PauseType.questMenu;
            }
            GameManager.instance.isPaused = !GameManager.instance.isPaused;
        }
    }
}
