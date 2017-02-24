using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Animator))]
public class GameOverScreen : MonoBehaviour {

    [SerializeField]
    private BlurOptimized cameraBlur;

    private Animator _anim;
    private UiElementHider _uiElementHider;

    void Awake()
    {
        _uiElementHider = new UiElementHider("InGameUI");
    }

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
        cameraBlur.enabled = false;
        _uiElementHider.Show();
    }

    public void Display()
    {
        Time.timeScale = 0.2f;
        gameObject.SetActive(true);
        cameraBlur.enabled = true;
        _uiElementHider.Hide();
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
