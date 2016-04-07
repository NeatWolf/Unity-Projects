using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject container;

	void Update ()
    {
        if (GameManager.instance.isMenuOpen && GameManager.instance.pauseType == GameManager.PauseType.pauseMenu)
        {
            container.SetActive(true);
            Time.timeScale = 0f;
        }
        else if(!GameManager.instance.isMenuOpen)
        {
            container.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.isMenuOpen && GameManager.instance.pauseType == GameManager.PauseType.pauseMenu)
            {
                GameManager.instance.isMenuOpen = false;
                GameManager.instance.pauseType = GameManager.PauseType.none;
            }
            else if(!GameManager.instance.isMenuOpen)
            {
                GameManager.instance.isMenuOpen = true;
                GameManager.instance.pauseType = GameManager.PauseType.pauseMenu;
            }
        }
	}

    public void Resume()
    {
        GameManager.instance.isMenuOpen = false;
    }

    public void ExitToMainMenu()
    {
        FadeInOut.instance.LoadLevel("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
