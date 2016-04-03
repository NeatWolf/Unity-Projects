using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {

	void Start ()
    {
        gameObject.SetActive(false);
	}
	
	void Update ()
    {
	
	}

    public void Display()
    {
        Time.timeScale = 0.5f;
        gameObject.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
