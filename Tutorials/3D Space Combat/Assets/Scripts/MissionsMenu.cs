using UnityEngine;
using System.Collections;

public class MissionsMenu : MonoBehaviour {

    public GameObject missionsMenuCanvas;

    private bool isOpen;

    void Update()
    {
        if (isOpen)
        {
            missionsMenuCanvas.SetActive(true);
            //mainCanvas.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            missionsMenuCanvas.SetActive(false);
            //mainCanvas.SetActive(true);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            isOpen = !isOpen;
        }
    }

    //public void UpdateMissionProgress()
}
