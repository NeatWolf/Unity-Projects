using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour {

    public Image box, healthBar;

    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    public bool healthBarVisible
    {
        get
        {
            return healthBar.enabled;
        }
        set
        {
            healthBar.enabled = value;
        }
    }

    public Vector2 boxSize
    {
        get
        {
            return box.rectTransform.sizeDelta;
        }
        set
        {
            box.rectTransform.sizeDelta = value;
        }
    }

    public Vector3 anchoredPosition
    {
        get
        {
            return rt.anchoredPosition;
        }
        set
        {
            rt.anchoredPosition = value;
        }
    }
}
