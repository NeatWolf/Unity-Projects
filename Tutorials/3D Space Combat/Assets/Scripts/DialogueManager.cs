using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text subtitleUIText;
    public List<float> subtitleTimings = new List<float>();
    public List<string> subtitleText = new List<string>();
    public List<float> triggerTimings = new List<float>();
    public List<string> triggerObjectNames = new List<string>();
    public List<string> triggerMethodNames = new List<string>();

    private AudioClip dialogueAudio;

    private string[] fileLines;

    private List<string> subtitleLines = new List<string>();
    private List<string> subtitleTimingStrings = new List<string>();
    private int nextSubtitle = 0;
    private string displaySubtitle;

    private List<string> triggerLines = new List<string>();
    private List<string> triggerTimingStrings = new List<string>();
    private List<string> triggers = new List<string>();
    private int nextTrigger = 0;

    private const float RATE = 44100.0f;

    public static DialogueManager instance;

    private AudioSource audioSource;

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
        audioSource = gameObject.AddComponent<AudioSource>();
    }

	void Update ()
    {
        if (!GameManager.instance.isMenuOpen)
        {
            if ((dialogueAudio != null) && (audioSource.clip != null) && audioSource.clip.name == dialogueAudio.name)
            {
                audioSource.UnPause();

                // Check for <break/> or negative nextSubtitle
                if (nextSubtitle > 0 && !subtitleText[nextSubtitle - 1].Contains("<break/>"))
                {
                    subtitleUIText.text = displaySubtitle;
                }
                else
                {
                    subtitleUIText.text = "";
                }

                if (nextSubtitle < subtitleText.Count)
                {
                    if (audioSource.timeSamples / RATE > subtitleTimings[nextSubtitle])
                    {
                        displaySubtitle = subtitleText[nextSubtitle];
                        nextSubtitle++;
                    }
                }

                if (nextTrigger < triggers.Count)
                {
                    if (audioSource.timeSamples / RATE > triggerTimings[nextTrigger])
                    {
                        // Perform trigger actions
                    }
                }
            }
        }
        else
        {
            if ((dialogueAudio != null) && (audioSource.clip != null) && audioSource.clip.name == dialogueAudio.name)
            {
                audioSource.Pause();
            }
            subtitleUIText.text = "";
        }
	}

    public void BeginDialogue(AudioClip passedClip)
    {
        dialogueAudio = passedClip;

        // Reset everything
        subtitleLines = new List<string>();
        subtitleTimingStrings = new List<string>();
        subtitleTimings = new List<float>();
        subtitleText = new List<string>();

        triggerLines = new List<string>();
        triggerTimingStrings = new List<string>();
        triggerTimings = new List<float>();
        triggers = new List<string>();
        triggerObjectNames = new List<string>();
        triggerMethodNames = new List<string>();

        nextSubtitle = 0;
        nextTrigger = 0;

        // Get everything from text file
        TextAsset temp = Resources.Load("Dialogues/" + "dialogue1") as TextAsset;
        fileLines = temp.text.Split('\n');

        // Split subtitles and triggers into different lists
        foreach(string line in fileLines)
        {
            if (line.Contains("<trigger/>"))
            {
                triggerLines.Add(line);
            }
            else
            {
                subtitleLines.Add(line);
            }
        }

        // Split out time
        for(int i = 0; i < subtitleLines.Count; i++)
        {
            string[] splitTemp = subtitleLines[i].Split('|');

            subtitleTimingStrings.Add(splitTemp[0]);
            subtitleTimings.Add(float.Parse(CleanTimeString(subtitleTimingStrings[i])));

            subtitleText.Add(splitTemp[1]);
        }

        // Split out trigger
        for (int i = 0; i < triggerLines.Count; i++)
        {
            string[] splitTemp = triggerLines[i].Split('|');

            triggerTimingStrings.Add(splitTemp[0]);
            triggerTimings.Add(float.Parse(CleanTimeString(triggerTimingStrings[i])));

            triggers.Add(splitTemp[1]);
            string[] splitTriggerTemp = triggers[i].Split('-');
            splitTriggerTemp[0] = splitTriggerTemp[0].Replace("<trigger/>", "");
            triggerObjectNames.Add(splitTriggerTemp[0]);
            triggerMethodNames.Add(splitTriggerTemp[1]);
        }

        // Set initial subtitle text
        if(subtitleText[0] != null)
        {
            displaySubtitle = subtitleText[0];
        }

        // Set and play audio clip
        if(dialogueAudio != null)
        {
            audioSource.clip = dialogueAudio;
            audioSource.Play();
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
