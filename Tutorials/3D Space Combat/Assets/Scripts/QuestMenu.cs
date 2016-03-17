using UnityEngine;
using System.Collections;

public class QuestMenu : MonoBehaviour {

    public GameObject questMenuCanvas;

    private Canvas canvas;
    private bool isPaused;

    void Start ()
    {
        canvas = GetComponent<Canvas>();
	}

    void Update()
    {
        if (isPaused)
        {
            questMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            GameManager.instance.isPaused = true;
        }
        else
        {
            questMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
            GameManager.instance.isPaused = false;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            isPaused = !isPaused;
        }
    }
}
