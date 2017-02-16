using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class GameOverScreen : MonoBehaviour {

    private Animator _anim;

	void Start ()
    {
        _anim = GetComponent<Animator>();
        gameObject.SetActive(false);
	}

    public void CloseMenu()
    {
        //anim.SetTrigger("FadeOutTrigger");
        Time.timeScale = 1f;
        GameManager.instance.IsMenuOpen = false;
        GameManager.instance.PauseType = GameManager.PauseTypeEnum.none;
        GameManager.instance.CloseWinScreen();
        gameObject.SetActive(false);
    }

    public void Display()
    {
        Time.timeScale = 0.5f;
        gameObject.SetActive(true);
        GameManager.instance.IsMenuOpen = true;
        GameManager.instance.PauseType = GameManager.PauseTypeEnum.gameOver;
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
