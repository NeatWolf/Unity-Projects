using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestDetailsScreen : MonoBehaviour {

    public Text questName, description;
    public Transform objectivesListTransform;
    public ObjectiveDisplay objectiveDisplayPrefab;

    void Start ()
    {
        GameManager.questManager.Display();
        QuestDisplay.OnClick += QuestDisplay_OnClick;
        QuestDisplay.OnPointerEnter += QuestDisplay_OnPointerEnter;
        questName.text = "";
        description.text = "";
        // Destroy all children
        for (int i = 0; i < objectivesListTransform.childCount; i++)
        {
            Destroy(objectivesListTransform.GetChild(i).gameObject);
        }
    }

    void OnDestroy()
    {
        QuestDisplay.OnClick -= QuestDisplay_OnClick;
        QuestDisplay.OnPointerEnter -= QuestDisplay_OnPointerEnter;
    }

    void Update ()
    {
	
	}

    private void QuestDisplay_OnClick(Quest sender)
    {
        Debug.Log(string.Format("Click handler for {0}", sender.questName));
    }

    private void QuestDisplay_OnPointerEnter(Quest sender)
    {
        Debug.Log(string.Format("Hover handler for {0}", sender.questName));
        Initialize(sender);
    }

    public void Initialize(Quest quest)
    {
        if(questName != null)
        {
            questName.text = quest.questName.ToUpper();
        }
        if(description != null)
        {
            description.text = quest.description;
        }

        // Destroy all children
        for (int i = 0; i < objectivesListTransform.childCount; i++)
        {
            Destroy(objectivesListTransform.GetChild(i).gameObject);
        }

        // Add objectives to list
        foreach (Objective obj in quest.objectives)
        {
            ObjectiveDisplay display = Instantiate(objectiveDisplayPrefab) as ObjectiveDisplay;
            display.transform.SetParent(objectivesListTransform, false);
            display.Initialize(obj);
        }
    }
}
