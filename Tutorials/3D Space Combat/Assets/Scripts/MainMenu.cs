using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Camera cam;
    public GameObject controlsGameObject;
    public GameObject menuGameObject;
    public GameObject backButton;

    private Animator camAnim;

    void Start()
    {
        controlsGameObject.SetActive(false);
        backButton.SetActive(false);
        camAnim = cam.GetComponent<Animator>();
    }

    public void NewGame()
    {
        Debug.Log("Load Main scene");
        FadeInOut.instance.LoadLevel("Main");
    }

    public void Quit()
    {
        Debug.Log("Quit application");
        Application.Quit();
    }

    public void Options()
    {
        camAnim.SetTrigger("EnterControls");
        controlsGameObject.SetActive(true);
        menuGameObject.SetActive(false);
        backButton.SetActive(true);
    }

    public void Back()
    {
        camAnim.SetTrigger("Back");
        controlsGameObject.SetActive(false);
        menuGameObject.SetActive(true);
        backButton.SetActive(false);
    }
}
