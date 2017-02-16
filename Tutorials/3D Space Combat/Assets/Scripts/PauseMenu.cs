using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    private GameObject container;
    [SerializeField]
    private GameObject headerText;
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
        headerText.SetActive(false);
        menuButtons.SetActive(false);

        // Controls Panel
        _controlsPanelGO = Instantiate(controlsPanelPrefab);
        _controlsPanelGO.transform.SetParent(menuActualSize);
        RectTransform controlsPanelRect = _controlsPanelGO.GetComponent<RectTransform>();
        controlsPanelRect.anchorMin = new Vector2(0, 0.5f);
        controlsPanelRect.anchorMax = new Vector2(1, 0.5f);
        controlsPanelRect.pivot = new Vector2(0.5f, 0.5f);
        controlsPanelRect.anchoredPosition = new Vector2(0f, controlsPanelRect.rect.height / 2f);
        controlsPanelRect.offsetMin = new Vector2(10f, controlsPanelRect.offsetMin.y);
        controlsPanelRect.offsetMax = new Vector2(-10f, controlsPanelRect.offsetMax.y);

        // Back Button
        _menuButtonGO = Instantiate(menuButtonPrefab);
        _menuButtonGO.transform.SetParent(transform);
        RectTransform menuButtonRect = _menuButtonGO.GetComponent<RectTransform>();
        menuButtonRect.anchorMin = new Vector2(0.5f, 0.5f);
        menuButtonRect.anchorMax = new Vector2(0.5f, 0.5f);
        menuButtonRect.pivot = new Vector2(0.5f, 0.5f);
        menuButtonRect.anchoredPosition = new Vector2(0f, -menuActualSize.rect.height / 4f);
        menuButtonRect.sizeDelta = new Vector2(350f, menuButtonRect.sizeDelta.y);
        menuButtonRect.GetComponentInChildren<Text>().text = "BACK";
        _menuButtonGO.GetComponent<Button>().onClick.AddListener(() => Back());
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
        headerText.SetActive(true);
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
        Time.timeScale = 1f;
        GameManager.instance.IsMenuOpen = false;
        GameManager.instance.PauseType = GameManager.PauseTypeEnum.none;
        _uiElementHider.Show();
    }
}
