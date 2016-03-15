﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionObjective : MonoBehaviour
{
    public Toggle toggle;
    public Text description;

    public void SetDescription(string desc)
    {
        description.text = desc;
    }

    public bool IsCompleted
    {
        get
        {
            return toggle.isOn;
        }
        set
        {
            toggle.isOn = value;
        }
    }
}
