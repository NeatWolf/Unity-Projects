using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenuCanvas;
	
	void Update ()
    {
        if (GameManager.instance.isPaused && GameManager.instance.pauseType == GameManager.PauseType.pauseMenu)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            GameManager.instance.isPaused = true;
        }
        else if(!GameManager.instance.isPaused)
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
            GameManager.instance.isPaused = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.isPaused)
            {
                GameManager.instance.pauseType = GameManager.PauseType.none;
            }
            else
            {
                GameManager.instance.pauseType = GameManager.PauseType.pauseMenu;
            }
            GameManager.instance.isPaused = !GameManager.instance.isPaused;
        }
	}

    public void Resume()
    {
        GameManager.instance.isPaused = false;
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
