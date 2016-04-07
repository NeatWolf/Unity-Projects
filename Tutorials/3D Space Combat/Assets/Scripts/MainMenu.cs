using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void NewGame()
    {
        FadeInOut.instance.LoadLevel("Main");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
