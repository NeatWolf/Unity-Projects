using UnityEngine;
using System.Collections;

public class GUICrosshair : MonoBehaviour {

    public Texture2D crosshairImage;
    public int crosshairWidth;
    public int crosshairHeight;

    void Start()
    {

    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(crosshairImage, new Vector2(crosshairWidth / 2, crosshairHeight / 2), CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
