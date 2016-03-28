using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateListItem : MonoBehaviour
{
    public Color textColor;

    [HideInInspector]
    public float animateTime = 0.5f;

    private RectTransform rectTransform;
    private Text textComponent;
    private float height;
    private Timer timer;

    public string Text
    {
        get
        {
            if(textComponent != null)
            {
                return textComponent.text;
            }
            else
            {
                return "Error!";
            }
        }
        set
        {
            if(textComponent != null)
            {
                textComponent.text = value;
            }
            else
            {
                Debug.Log("Text component hasn't been set");
            }
        }
    }

    public bool FadeOutInProgress { get; set; }
    public bool FadeInInProgress { get; set; }
    public bool MoveDownInProgress { get; set; }

	void Awake ()
    {
        rectTransform = GetComponent<RectTransform>();
        textComponent = GetComponent<Text>();
        height = rectTransform.rect.height;
        timer = GetComponent<Timer>();
	}
	
	void Update ()
    {
        if (timer != null && timer.currentTime <= 0)
        {
            RemoveFromBottom();
            timer.ResetTimer();
        }
    }

    public void AddToTop()
    {
        timer.ResetTimer();
        timer.StartTimer();
        StartCoroutine(FadeIn(animateTime));
        StartCoroutine(MoveDownCoroutine(animateTime));
    }

    public void RemoveFromBottom()
    {
        StartCoroutine(FadeOut(animateTime));
        StartCoroutine(MoveDownCoroutine(animateTime));
    }

    public void MoveDown()
    {
        StartCoroutine(MoveDownCoroutine(animateTime));
    }

    IEnumerator FadeIn(float time)
    {
        textComponent.color = Color.clear;
        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (textComponent.color != textColor)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            textComponent.color = Color.Lerp(Color.clear, textColor, percentageComplete);
            yield return null;
        }
    }

    IEnumerator FadeOut(float time)
    {
        Color startColor = textComponent.color;
        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;
        
        while (textComponent.color != Color.clear)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            textComponent.color = Color.Lerp(startColor, Color.clear, percentageComplete);
            yield return null;
        }
    }

    IEnumerator MoveDownCoroutine(float time)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = new Vector2(startPosition.x, startPosition.y - height);
        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (rectTransform.anchoredPosition.y != (startPosition.y - height))
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, percentageComplete);
            yield return null;
        }
    }
}
