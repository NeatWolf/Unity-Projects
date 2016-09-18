using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class GameOverScreen : MonoBehaviour {

    private Animator anim;

	void Start ()
    {
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
	}

    public void CloseMenu()
    {
        //anim.SetTrigger("FadeOutTrigger");
        Time.timeScale = 1f;
        GameManager.instance.isMenuOpen = false;
        GameManager.instance.pauseType = GameManager.PauseType.none;
        GameManager.instance.CloseWinScreen();
        gameObject.SetActive(false);
    }

    public void Display()
    {
        Time.timeScale = 0.5f;
        gameObject.SetActive(true);
        GameManager.instance.isMenuOpen = true;
        GameManager.instance.pauseType = GameManager.PauseType.gameOver;
    }

    public void Retry()
    {
        CloseMenu();
        FadeInOut.instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenu()
    {
        CloseMenu();
        FadeInOut.instance.LoadLevel("Main Menu");
    }
}
