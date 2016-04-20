using UnityEngine;
using System.Collections;

public class ReticleController : MonoBehaviour {

    public bool menuScene = false;
    public Texture2D reticleTexture;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 reticleHotSpot = Vector2.zero;
    public Vector2 cursorHotSpot = Vector2.zero;

    private bool isPaused = false;

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
            bool gamePaused = GameManager.instance.isMenuOpen;
            if (gamePaused != isPaused)
            {
                isPaused = gamePaused;
                if (gamePaused)
                {
                    Cursor.SetCursor(cursorTexture, cursorHotSpot, cursorMode);
                }
                else
                {
                    Cursor.SetCursor(reticleTexture, reticleHotSpot, cursorMode);
                }
            }
            if (!GameManager.instance.isCursorVisible && !gamePaused)
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
