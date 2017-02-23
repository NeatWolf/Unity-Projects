using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    private GameObject container;
    [SerializeField]
    private GameObject menuButtons;
    [SerializeField]
    private RectTransform menuActualSize;
    [SerializeField]
    private GameObject controlsPanelPrefab;
    [SerializeField]
    private GameObject menuButtonPrefab;
    [SerializeField]
    private BlurOptimized cameraBlur;
    [Header("Content Pane")]
    [SerializeField]
    private GameObject controls;

    private GameObject _controlsPanelGO;
    private GameObject _menuButtonGO;
    private UiElementHider _uiElementHider;

    void Awake()
    {
        _uiElementHider = new UiElementHider("InGameUI");
    }

    void Start()
    {
        Close();
    }

    void Update ()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (GameManager.instance.PauseType != GameManager.PauseTypeEnum.none)
        {
            Close();
            return;
        }

        Open();
    }

    public void Resume()
    {
        Close();
    }

    public void ShowControls()
    {
        controls.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        GameManager.instance.IsMenuOpen = false;
        FadeInOut.instance.LoadLevel("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        Debug.Log("Back button was pressed");
        Destroy(_controlsPanelGO);
        Destroy(_menuButtonGO);
        menuButtons.SetActive(true);
    }

    private void Open()
    {
        container.SetActive(true);
        cameraBlur.enabled = true;
        Time.timeScale = 0f;
        GameManager.instance.IsMenuOpen = true;
        GameManager.instance.PauseType = GameManager.PauseTypeEnum.pauseMenu;
        _uiElementHider.Hide();
    }

    private void Close()
    {
        container.SetActive(false);
        cameraBlur.enabled = false;
        controls.SetActive(false);
        Time.timeScale = 1f;
        GameManager.instance.IsMenuOpen = false;
        GameManager.instance.PauseType = GameManager.PauseTypeEnum.none;
        _uiElementHider.Show();
    }
}
