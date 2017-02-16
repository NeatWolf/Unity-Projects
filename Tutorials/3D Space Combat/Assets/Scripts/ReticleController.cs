using UnityEngine;
using System.Collections;

public class ReticleController : MonoBehaviour {

    [SerializeField]
    private bool menuScene = false;
    [SerializeField]
    private Texture2D reticleTexture;
    [SerializeField]
    private Texture2D cursorTexture;
    [SerializeField]
    private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField]
    private Vector2 reticleHotSpot = Vector2.zero;
    [SerializeField]
    private Vector2 cursorHotSpot = Vector2.zero;

    private bool _isPaused = false;

    void Start()
    {
        if (!menuScene)
        {
            Cursor.SetCursor(reticleTexture, reticleHotSpot, cursorMode);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Cursor.SetCursor(cursorTexture, cursorHotSpot, cursorMode);
        }
    }

    void Update()
    {
        if (GameManager.instance != null)
        {
            bool gamePaused = GameManager.instance.IsMenuOpen;
            if (gamePaused != _isPaused)
            {
                _isPaused = gamePaused;
                if (gamePaused)
                {
                    Cursor.SetCursor(cursorTexture, cursorHotSpot, cursorMode);
                }
                else
                {
                    Cursor.SetCursor(reticleTexture, reticleHotSpot, cursorMode);
                }
            }
            if (!GameManager.instance.IsCursorVisible && !gamePaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    //void OnMouseEnter()
    //{
    //    Cursor.SetCursor(reticleTexture, reticleHotSpot, cursorMode);
    //}

    //void OnMouseExit()
    //{
    //    Cursor.SetCursor(null, Vector2.zero, cursorMode);
    //}
}
