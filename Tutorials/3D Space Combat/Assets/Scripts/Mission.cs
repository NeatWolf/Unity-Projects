using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Mission : MonoBehaviour
{
    public string description;
    public Dictionary<string, Objective.ObjectiveState> objectives;

    private Text nameText;

    public delegate void MissionDetailsHandler(string missionName, string description, Dictionary<string, Objective.ObjectiveState> objectives);

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
        button.onClick.AddListener(() => method(MissionName, description, objectives));
    }

    public void SetParent(VerticalLayoutGroup group)
    {
        transform.SetParent(group.transform, false);
    }
}
