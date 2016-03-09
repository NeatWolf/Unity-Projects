using UnityEngine;
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
}
