using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text subtitleUIText;
    public Image backgroundImg;
    public List<float> subtitleTimings = new List<float>();
    public List<string> subtitleText = new List<string>();
    public List<float> triggerTimings = new List<float>();
    public List<string> triggerObjectNames = new List<string>();
    public List<string> triggerMethodNames = new List<string>();

    private AudioClip dialogueAudio;

    private string[] fileLines;

    private List<string> _subtitleLines = new List<string>();
    private List<string> _subtitleTimingStrings = new List<string>();
    private int _nextSubtitle = 0;
    private string _displaySubtitle;

    private List<string> _triggerLines = new List<string>();
    private List<string> _triggerTimingStrings = new List<string>();
    private List<string> _triggers = new List<string>();
    private int _nextTrigger = 0;

    private const float RATE = 44100.0f;

    public static DialogueManager instance;

    private AudioSource _audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

	void Update ()
    {
        if (!GameManager.instance.IsMenuOpen)
        {
            if ((dialogueAudio != null) && (_audioSource.clip != null) && _audioSource.clip.name == dialogueAudio.name)
            {
                _audioSource.UnPause();

                // Check for <break/> or negative nextSubtitle
                if (_nextSubtitle > 0 && !subtitleText[_nextSubtitle - 1].Contains("<break/>"))
                {
                    backgroundImg.enabled = true;
                    if (subtitleUIText.text != _displaySubtitle) subtitleUIText.text = _displaySubtitle;
                }
                else
                {
                    backgroundImg.enabled = false;
                    if (!string.IsNullOrEmpty(subtitleUIText.text)) subtitleUIText.text = "";
                }

                if (_nextSubtitle < subtitleText.Count)
                {
                    if (_audioSource.timeSamples / RATE > subtitleTimings[_nextSubtitle])
                    {
                        _displaySubtitle = subtitleText[_nextSubtitle];
                        _nextSubtitle++;
                    }
                }

                if (_nextTrigger < _triggers.Count)
                {
                    if (_audioSource.timeSamples / RATE > triggerTimings[_nextTrigger])
                    {
                        // Perform trigger actions
                    }
                }
            }
        }
        else
        {
            if ((dialogueAudio != null) && (_audioSource.clip != null) && _audioSource.clip.name == dialogueAudio.name)
            {
                _audioSource.Pause();
            }
            if (!string.IsNullOrEmpty(subtitleUIText.text)) subtitleUIText.text = "";
        }
	}

    public void BeginDialogue(AudioClip passedClip, string textFileName)
    {
        dialogueAudio = passedClip;

        // Reset everything
        _subtitleLines = new List<string>();
        _subtitleTimingStrings = new List<string>();
        subtitleTimings = new List<float>();
        subtitleText = new List<string>();

        _triggerLines = new List<string>();
        _triggerTimingStrings = new List<string>();
        triggerTimings = new List<float>();
        _triggers = new List<string>();
        triggerObjectNames = new List<string>();
        triggerMethodNames = new List<string>();

        _nextSubtitle = 0;
        _nextTrigger = 0;

        // Get everything from text file
        TextAsset temp = Resources.Load("Dialogues/" + textFileName) as TextAsset;
        fileLines = temp.text.Split('\n');

        // Split subtitles and triggers into different lists
        foreach(string line in fileLines)
        {
            if (line.Contains("<trigger/>"))
            {
                _triggerLines.Add(line);
            }
            else
            {
                _subtitleLines.Add(line);
            }
        }

        // Split out time
        for(int i = 0; i < _subtitleLines.Count; i++)
        {
            string[] splitTemp = _subtitleLines[i].Split('|');

            _subtitleTimingStrings.Add(splitTemp[0]);
            subtitleTimings.Add(float.Parse(CleanTimeString(_subtitleTimingStrings[i])));

            subtitleText.Add(splitTemp[1]);
        }

        // Split out trigger
        for (int i = 0; i < _triggerLines.Count; i++)
        {
            string[] splitTemp = _triggerLines[i].Split('|');

            _triggerTimingStrings.Add(splitTemp[0]);
            triggerTimings.Add(float.Parse(CleanTimeString(_triggerTimingStrings[i])));

            _triggers.Add(splitTemp[1]);
            string[] splitTriggerTemp = _triggers[i].Split('-');
            splitTriggerTemp[0] = splitTriggerTemp[0].Replace("<trigger/>", "");
            triggerObjectNames.Add(splitTriggerTemp[0]);
            triggerMethodNames.Add(splitTriggerTemp[1]);
        }

        // Set initial subtitle text
        if(subtitleText[0] != null)
        {
            _displaySubtitle = subtitleText[0];
        }

        // Set and play audio clip
        if(dialogueAudio != null)
        {
            _audioSource.clip = dialogueAudio;
            _audioSource.Play();
        }
    }

    /// <summary>
    /// Remove all characters that are not part of the timing float
    /// </summary>
    /// <param name="timeString"></param>
    /// <returns></returns>
    private string CleanTimeString(string timeString)
    {
        Regex digitsOnly = new Regex(@"[^\d+(\.\d+)*S]");
        return digitsOnly.Replace(timeString, "");
    }
}
