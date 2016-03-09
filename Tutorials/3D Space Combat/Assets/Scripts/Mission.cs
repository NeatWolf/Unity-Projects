using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Mission : MonoBehaviour
{
    public string description;
    public Dictionary<string, bool> startingObjectives;

    private Text nameText;

    public delegate void MissionDetailsHandler(string missionName, string description, Dictionary<string, bool> objectives);

    public string MissionName
    {
        get
        {
            if(nameText != null)
            {
                return nameText.text;
            }
            else
            {
                return "Unknown";
            }
        }
        set
        {
            nameText.text = value;
        }
    }

    void Awake()
    {
        nameText = GetComponentInChildren<Text>();
    }

    public void SetOnClickMissionDetails(MissionDetailsHandler method)
    {
        Button button = gameObject.GetComponentInChildren<Button>() as Button;
        button.onClick.AddListener(() => method(MissionName, description, startingObjectives));
    }

    public void SetParent(VerticalLayoutGroup group)
    {
        transform.SetParent(group.transform, false);
    }
}
