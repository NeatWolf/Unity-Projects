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

    public void Display()
    {
        Time.timeScale = 0.5f;
        gameObject.SetActive(true);
        GameManager.instance.isMenuOpen = true;
    }

    public void Retry()
    {
        CloseMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenu()
    {
        CloseMenu();
        SceneManager.LoadScene("Main Menu");
    }

    private void CloseMenu()
    {
        anim.SetTrigger("FadeOutTrigger");
        Time.timeScale = 1f;
        GameManager.instance.isMenuOpen = false;
    }
}
