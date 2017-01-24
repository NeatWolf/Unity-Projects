using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class Quest : MonoBehaviour
{
    public string questName;
    [Multiline]
    public string description;
    public Objective currentObjective;
    public Text currentObjectiveDescription;
    public Objective.ObjectiveState state;
    public Objective[] objectives;

    public delegate void QuestCompletedDelegate(Quest sender);
    public event QuestCompletedDelegate OnCompleted;

    void Start ()
    {
        // Set the quest and the first objective to active
        currentObjective.state = Objective.ObjectiveState.active;
        //currentObjective.InvokeOnStartedEvent();
        GameObject objectiveParentGameObject = currentObjective.transform.parent.gameObject;
        if (objectiveParentGameObject == null) return;

        objectives = objectiveParentGameObject.GetComponentsInChildren<Objective>();
        if(objectives == null)
        {
            Debug.Log("Failed to find mission objectives");
        }

        Debug.Log("Successfully found all mission objectives");
        objectives.Where(t => t != null).Select(t => { t.ParentScript = this; return t; }).ToList();
	}

    public Objective GetObjectiveAtIndex(int index)
    {
        return objectives.Where(t => t.index == index).FirstOrDefault();
    }

    public void OnObjectivesCompleted()
    {
        print(string.Format("completed quest: {0}", questName));
        state = Objective.ObjectiveState.complete;
        if(OnCompleted != null)
        {
            OnCompleted(this);
        }
    }
}
