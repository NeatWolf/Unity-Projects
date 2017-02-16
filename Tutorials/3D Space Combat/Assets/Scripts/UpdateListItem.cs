using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateListItem : MonoBehaviour
{
    [SerializeField]
    private Color textColor;

    private float _animateTime = 0.5f;
    private RectTransform _rectTransform;
    private Text _textComponent;
    private float _height;
    private Timer _timer;

    public float AnimateTime
    {
        get { return _animateTime; }
        set { _animateTime = value; }
    }

    public string Text
    {
        get
        {
            if(_textComponent != null)
            {
                return _textComponent.text;
            }
            else
            {
                return "Error!";
            }
        }
        set
        {
            if(_textComponent != null)
            {
                _textComponent.text = value;
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
        _rectTransform = GetComponent<RectTransform>();
        _textComponent = GetComponent<Text>();
        _height = _rectTransform.rect.height;
        _timer = GetComponent<Timer>();
	}
	
	void Update ()
    {
        if (_timer != null && _timer.CurrentTime <= 0)
        {
            RemoveFromBottom();
            _timer.ResetTimer();
        }
    }

    public void AddToTop()
    {
        _timer.ResetTimer();
        _timer.StartTimer();
        StartCoroutine(FadeIn(_animateTime));
        StartCoroutine(MoveDownCoroutine(_animateTime));
    }

    public void RemoveFromBottom()
    {
        StartCoroutine(FadeOut(_animateTime));
        StartCoroutine(MoveDownCoroutine(_animateTime));
    }

    public void MoveDown()
    {
        StartCoroutine(MoveDownCoroutine(_animateTime));
    }

    IEnumerator FadeIn(float time)
    {
        _textComponent.color = Color.clear;
        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (_textComponent.color != textColor)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            _textComponent.color = Color.Lerp(Color.clear, textColor, percentageComplete);
            yield return null;
        }
    }

    IEnumerator FadeOut(float time)
    {
        Color startColor = _textComponent.color;
        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;
        
        while (_textComponent.color != Color.clear)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            _textComponent.color = Color.Lerp(startColor, Color.clear, percentageComplete);
            yield return null;
        }
    }

    IEnumerator MoveDownCoroutine(float time)
    {
        Vector2 startPosition = _rectTransform.anchoredPosition;
        Vector2 endPosition = new Vector2(startPosition.x, startPosition.y - _height);
        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (_rectTransform.anchoredPosition.y != (startPosition.y - _height))
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            _rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, percentageComplete);
            yield return null;
        }
    }
}
