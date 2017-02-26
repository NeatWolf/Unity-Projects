using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject controls;
    [SerializeField]
    private GameObject menuGameObject;

    void Start()
    {
        controls.SetActive(false);
    }

    public void NewGame()
    {
        Debug.Log("Load Main scene");
        controls.SetActive(false);
        FadeInOut.instance.LoadLevel("Main");
    }

    public void Quit()
    {
        Debug.Log("Quit application");
        Application.Quit();
    }

    public void Options()
    {
        controls.SetActive(!controls.activeSelf);
    }
}
