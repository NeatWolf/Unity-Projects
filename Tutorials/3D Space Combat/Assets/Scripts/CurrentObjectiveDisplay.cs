using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CurrentObjectiveDisplay : MonoBehaviour {

    private Text _objectiveText;

    void Start()
    {
        _objectiveText = GetComponent<Text>();
        _objectiveText.text = GameManager.questManager.ActiveObjective;
    }

    void Update ()
    {
        if(GameManager.questManager.ActiveObjective == null)
        {
            _objectiveText.text = "";
        }
        else if (!_objectiveText.text.Equals(GameManager.questManager.ActiveObjective))
        {
            _objectiveText.text = GameManager.questManager.ActiveObjective;
        }
	}
}
