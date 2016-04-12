using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

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
}
