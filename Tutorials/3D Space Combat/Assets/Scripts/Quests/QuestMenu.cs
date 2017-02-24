using UnityEngine;
using System.Collections;

public class QuestMenu : MonoBehaviour {

    public GameObject questMenuCanvas;

    private UiElementHider _uiElementHider;

    void Awake()
    {
        _uiElementHider = new UiElementHider("InGameUI");
    }

    void Update()
    {
        if (GameManager.instance.IsMenuOpen && GameManager.instance.PauseType == GameManager.PauseTypeEnum.questMenu)
        {
            questMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (!GameManager.instance.IsMenuOpen)
        {
            questMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            // If game is already paused because of another menu, don't do anything
            if (GameManager.instance.IsMenuOpen && GameManager.instance.PauseType != GameManager.PauseTypeEnum.questMenu)
            {
                return;
            }
            if (GameManager.instance.IsMenuOpen)
            {
                GameManager.instance.PauseType = GameManager.PauseTypeEnum.none;
                _uiElementHider.Show();
            }
            else
            {
                GameManager.instance.PauseType = GameManager.PauseTypeEnum.questMenu;
                _uiElementHider.Hide();
            }
            GameManager.instance.IsMenuOpen = !GameManager.instance.IsMenuOpen;
        }
    }
}
