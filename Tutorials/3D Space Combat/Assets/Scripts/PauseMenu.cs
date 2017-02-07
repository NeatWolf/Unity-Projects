using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PauseMenu : MonoBehaviour {

    public GameObject container;
    public GameObject headerText;
    public GameObject menuButtons;
    public RectTransform menuActualSize;
    public GameObject controlsPanelPrefab;
    public GameObject menuButtonPrefab;
    public BlurOptimized cameraBlur;

    private GameObject controlsPanelGO;
    private GameObject menuButtonGO;

    void Start()
    {
        Close();
    }

    void Update ()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (GameManager.instance.pauseType != GameManager.PauseType.none)
        {
            Close();
            return;
        }

        Open();
    }

    public void Open()
    {
        container.SetActive(true);
        cameraBlur.enabled = true;
        Time.timeScale = 0f;
        GameManager.instance.isMenuOpen = true;
        GameManager.instance.pauseType = GameManager.PauseType.pauseMenu;
    }

    public void Close()
    {
        container.SetActive(false);
        cameraBlur.enabled = false;
        Time.timeScale = 1f;
        GameManager.instance.isMenuOpen = false;
        GameManager.instance.pauseType = GameManager.PauseType.none;
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
        controlsPanelGO = Instantiate(controlsPanelPrefab);
        controlsPanelGO.transform.SetParent(menuActualSize);
        RectTransform controlsPanelRect = controlsPanelGO.GetComponent<RectTransform>();
        controlsPanelRect.anchorMin = new Vector2(0, 0.5f);
        controlsPanelRect.anchorMax = new Vector2(1, 0.5f);
        controlsPanelRect.pivot = new Vector2(0.5f, 0.5f);
        controlsPanelRect.anchoredPosition = new Vector2(0f, controlsPanelRect.rect.height / 2f);
        controlsPanelRect.offsetMin = new Vector2(10f, controlsPanelRect.offsetMin.y);
        controlsPanelRect.offsetMax = new Vector2(-10f, controlsPanelRect.offsetMax.y);

        // Back Button
        menuButtonGO = Instantiate(menuButtonPrefab);
        menuButtonGO.transform.SetParent(transform);
        RectTransform menuButtonRect = menuButtonGO.GetComponent<RectTransform>();
        menuButtonRect.anchorMin = new Vector2(0.5f, 0.5f);
        menuButtonRect.anchorMax = new Vector2(0.5f, 0.5f);
        menuButtonRect.pivot = new Vector2(0.5f, 0.5f);
        menuButtonRect.anchoredPosition = new Vector2(0f, -menuActualSize.rect.height / 4f);
        menuButtonRect.sizeDelta = new Vector2(350f, menuButtonRect.sizeDelta.y);
        menuButtonRect.GetComponentInChildren<Text>().text = "BACK";
        menuButtonGO.GetComponent<Button>().onClick.AddListener(() => Back());
    }

    public void ExitToMainMenu()
    {
        GameManager.instance.isMenuOpen = false;
        FadeInOut.instance.LoadLevel("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        Debug.Log("Back button was pressed");
        Destroy(controlsPanelGO);
        Destroy(menuButtonGO);
        headerText.SetActive(true);
        menuButtons.SetActive(true);
    }
}
