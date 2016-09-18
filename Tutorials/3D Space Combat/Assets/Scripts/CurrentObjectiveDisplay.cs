using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CurrentObjectiveDisplay : MonoBehaviour {

    private Text objectiveText;

    void Start()
    {
        objectiveText = GetComponent<Text>();
        objectiveText.text = GameManager.questManager.ActiveObjective;
    }

    void Update ()
    {
        if(GameManager.questManager.ActiveObjective == null)
        {
            objectiveText.text = "";
        }
        else if (!objectiveText.text.Equals(GameManager.questManager.ActiveObjective))
        {
            objectiveText.text = GameManager.questManager.ActiveObjective;
        }
	}
}
