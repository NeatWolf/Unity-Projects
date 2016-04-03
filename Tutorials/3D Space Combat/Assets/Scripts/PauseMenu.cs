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
            if (GameManager.instance.isMenuOpen)
            {
                GameManager.instance.pauseType = GameManager.PauseType.none;
            }
            else
            {
                GameManager.instance.pauseType = GameManager.PauseType.pauseMenu;
            }
            GameManager.instance.isMenuOpen = !GameManager.instance.isMenuOpen;
        }
	}

    public void Resume()
    {
        GameManager.instance.isMenuOpen = false;
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
