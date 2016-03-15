using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestsDisplay : MonoBehaviour {

    public Transform targetTransform;
    public QuestDisplay questDisplayPrefab;

    public QuestManager questManager;

	void Start ()
    {
        QuestManager.OnChanged += QuestManager_OnChanged;
	}

    void OnDestroy()
    {
        QuestManager.OnChanged -= QuestManager_OnChanged;
    }

    void Update ()
    {
        
	}

    private void QuestManager_OnChanged(QuestManager questManager)
    {
        if (this.questManager == questManager)
        {
            Initialize(questManager);
        }
    }

    public void Initialize(QuestManager questManager)
    {
        Debug.Log("Initializing QuestsDisplay");

        // Destroy all children
        for(int i = 0; i < targetTransform.childCount; i++)
        {
            Destroy(targetTransform.GetChild(i).gameObject);
        }

        // Initialize quest list
        this.questManager = questManager;
        List<Quest> quests = questManager.quests;
        foreach(Quest quest in quests)
        {
            QuestDisplay display = Instantiate(questDisplayPrefab) as QuestDisplay;
            display.transform.SetParent(targetTransform, false);
            display.Initialize(quest);
        }
    }
}
