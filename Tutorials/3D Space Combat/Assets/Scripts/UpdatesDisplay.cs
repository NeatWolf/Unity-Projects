using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UpdatesDisplay : MonoBehaviour {

    public Transform listTransform;
    public Text updateTextPrefab;

	void Start ()
    {
        GameManager.questManager.OnQuestAdd += QuestManager_OnQuestAdd;
    }

    void OnDestroy()
    {
        GameManager.questManager.OnQuestAdd -= QuestManager_OnQuestAdd;
    }

    void Update ()
    {
	
	}

    private void QuestManager_OnQuestAdd(Quest addedQuest)
    {
        addedQuest.OnCompleted += Quest_OnCompleted;
        foreach (Objective obj in addedQuest.objectives)
        {
            print("Subscribed to objective");
            obj.OnCompleted += Objective_OnCompleted;
        }
    }

    private void Quest_OnCompleted(Quest sender)
    {
        Text updateLine = Instantiate(updateTextPrefab) as Text;
        updateLine.transform.SetParent(listTransform, false);
        updateLine.transform.SetAsFirstSibling();
        updateLine.text = "Quest: " + sender.questName + " completed";
        sender.OnCompleted -= Quest_OnCompleted;
    }

    private void Objective_OnCompleted(Objective sender)
    {
        Text updateLine = Instantiate(updateTextPrefab) as Text;
        updateLine.transform.SetParent(listTransform, false);
        updateLine.transform.SetAsFirstSibling();
        updateLine.text = "Objective: " + sender.description + " completed";
        sender.OnCompleted -= Objective_OnCompleted;
    }
}
