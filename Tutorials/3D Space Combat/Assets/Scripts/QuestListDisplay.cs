using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestListDisplay : MonoBehaviour {

    public Transform listTransform;
    public QuestDisplay questDisplayPrefab;

    public QuestManager questManager;

	void Start ()
    {
        questManager.OnChanged += QuestManager_OnChanged;
	}

    void OnDestroy()
    {
        questManager.OnChanged -= QuestManager_OnChanged;
    }

    void Update ()
    {
        
	}

    private void QuestManager_OnChanged(QuestManager sender)
    {
        if(sender == questManager)
        {
            Initialize(questManager);
        }
    }

    public void Initialize(QuestManager questManager)
    {
        Debug.Log("Initializing QuestsDisplay");

        // Destroy all children
        for(int i = 0; i < listTransform.childCount; i++)
        {
            Destroy(listTransform.GetChild(i).gameObject);
        }

        // Initialize quest list
        this.questManager = questManager;
        List<Quest> quests = questManager.quests;
        foreach(Quest quest in quests)
        {
            QuestDisplay display = Instantiate(questDisplayPrefab) as QuestDisplay;
            display.transform.SetParent(listTransform, false);
            display.Initialize(quest);
        }
    }
}
