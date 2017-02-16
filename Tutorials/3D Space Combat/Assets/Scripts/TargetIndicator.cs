using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour {

    [SerializeField]
    private Image box, healthBar, trajectory;

    private RectTransform _rt;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
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

    public float healthBarFillAmount
    {
        get
        {
            return healthBar.fillAmount;
        }
        set
        {
            healthBar.fillAmount = value;
        }
    }

    public float boxAlpha
    {
        get
        {
            return box.color.a;
        }
        set
        {
            Color c = box.color;
            c.a = value;
            box.color = c;
        }
    }

    public Vector3 anchoredPosition
    {
        get
        {
            return _rt.anchoredPosition;
        }
        set
        {
            _rt.anchoredPosition = value;
        }
    }
}
