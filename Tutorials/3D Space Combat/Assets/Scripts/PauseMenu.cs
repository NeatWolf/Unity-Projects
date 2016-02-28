using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenuCanvas;
    public GameObject mainCanvas;

    private bool isPaused;
	
	void Update ()
    {
        if (isPaused)
        {
            pauseMenuCanvas.SetActive(true);
            //mainCanvas.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuCanvas.SetActive(false);
            //mainCanvas.SetActive(true);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
	}

    public void Resume()
    {
        isPaused = false;
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
