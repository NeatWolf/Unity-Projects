using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UICountdown : MonoBehaviour {

    [SerializeField]
    private string formattedText;
    [SerializeField]
    private float startTimeLeft;

    private bool _ticking;
    private Text _uiText;

    public delegate void CountdownFinishedDelegate();
    public event CountdownFinishedDelegate Finished;

    public float TimeLeft { get; set; }

	void Start ()
    {
        _uiText = GetComponent<Text>();
        _uiText.canvasRenderer.SetAlpha(0f);
        _ticking = false;
        TimeLeft = startTimeLeft;
	}
	
	void Update ()
    {
        if (!_ticking) return;

        TimeLeft -= Time.deltaTime;
        _uiText.text = string.Format(formattedText, (int)TimeLeft);
        if (TimeLeft <= 0)
        {
            OnFinished();
            Stop();
        }
	}

    private void OnFinished()
    {
        if (Finished != null) Finished();
    }

    public void Play()
    {
        _ticking = true;
        _uiText.canvasRenderer.SetAlpha(0.01f);
        _uiText.CrossFadeAlpha(1f, 0.2f, false);
    }

    public void Pause() { _ticking = false; }

    public void Stop()
    {
        _ticking = false;
        _uiText.CrossFadeAlpha(0f, 0.2f, false);
        TimeLeft = startTimeLeft;
    }
}
