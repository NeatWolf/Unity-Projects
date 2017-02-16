using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject controlsGameObject;
    [SerializeField]
    private GameObject menuGameObject;

    private Animator _camAnim;

    void Start()
    {
        controlsGameObject.SetActive(false);
        _camAnim = cam.GetComponent<Animator>();
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
        _camAnim.SetTrigger("EnterControls");
        controlsGameObject.SetActive(true);
    }
}
