using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIProgressBarController : MonoBehaviour {

    [SerializeField]
    private Image barFullImage;
    [SerializeField]
    private Image barEmptyImage;

    public float fillAmount
    {
        get
        {
            return barFullImage.fillAmount;
        }
        set
        {
            barFullImage.fillAmount = value;
        }
    }

    public Vector2 anchoredPosition
    {
        get
        {
            return barEmptyImage.rectTransform.anchoredPosition;
        }
        set
        {
            barEmptyImage.rectTransform.anchoredPosition = value;
            barFullImage.rectTransform.anchoredPosition = value;
        }
    }

    public void SetAlpha(float value)
    {
        barEmptyImage.canvasRenderer.SetAlpha(value);
        barFullImage.canvasRenderer.SetAlpha(value);
    }

    public void FadeIn(float duration)
    {
        barEmptyImage.CrossFadeAlpha(1, duration, false);
        barFullImage.CrossFadeAlpha(1, duration, false);
    }

    public void FadeOut(float duration)
    {
        barEmptyImage.CrossFadeAlpha(0, duration, false);
        barFullImage.CrossFadeAlpha(0, duration, false);
    }
}
