using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void NewGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
