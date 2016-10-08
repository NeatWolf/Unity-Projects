using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Camera cam;
    public GameObject controlsGameObject;
    public GameObject menuGameObject;

    private Animator camAnim;

    void Start()
    {
        controlsGameObject.SetActive(false);
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
    }
}
