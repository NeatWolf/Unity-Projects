using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class QuestManager : MonoBehaviour {

    public QuestListDisplay questsDisplayPrefab;
    public Transform questMenuParent;
    public List<Quest> quests = new List<Quest>();

    public delegate void QuestManagerDelegate(QuestManager sender);
    public event QuestManagerDelegate OnChanged;

    private Quest _currentQuest;

    void Start()
    {

    }

    void Update()
    {

    }

    private void Quest_OnCompleted(Quest sender)
    {
        Remove(sender);
    }

    public void Display()
    {
        QuestListDisplay display = Instantiate(questsDisplayPrefab) as QuestListDisplay;
        display.transform.SetParent(questMenuParent, false);
        display.Initialize(this);
    }

    public void Add(Quest newQuest)
    {
        if(newQuest == null)
        {
            return;
        }
        quests.Add(newQuest);
        newQuest.OnCompleted += Quest_OnCompleted;
        if(OnChanged != null)
        {
            OnChanged(this);
        }
    }

    public void Remove(Quest quest)
    {
        if (quest == null)
        {
            return;
        }
        if (!quests.Contains(quest))
        {
            return;
        }
        quest.OnCompleted -= Quest_OnCompleted;
        quests.Remove(quest);
        if (OnChanged != null)
        {
            OnChanged(this);
        }
    }
}
