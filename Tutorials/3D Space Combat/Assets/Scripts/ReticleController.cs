using UnityEngine;
using System.Collections;

public class ReticleController : MonoBehaviour {

    public Texture2D reticleTexture;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 reticleHotSpot = Vector2.zero;
    public Vector2 cursorHotSpot = Vector2.zero;

    private bool isPaused = false;

    void Start()
    {
        Cursor.SetCursor(reticleTexture, reticleHotSpot, cursorMode);
    }

    void Update()
    {
        bool gamePaused = GameManager.instance.isPaused;
        if (gamePaused != isPaused)
        {
            isPaused = gamePaused;
            if(gamePaused)
            {
                Cursor.SetCursor(cursorTexture, cursorHotSpot, cursorMode);
            }
            else
            {
                Cursor.SetCursor(reticleTexture, reticleHotSpot, cursorMode);
            }
        }
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(reticleTexture, reticleHotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
