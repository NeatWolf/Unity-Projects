using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class QuestManager : MonoBehaviour {

    public QuestsDisplay questsDisplayPrefab;
    public List<Quest> quests = new List<Quest>();

    public delegate void QuestManagerDelegate(QuestManager questManager);
    public static event QuestManagerDelegate OnChanged;

    private Quest _currentQuest;

    void Start()
    {

    }

    void Update()
    {
        //Quest finishedQuest = null;
        //foreach(Quest quest in quests)
        //{
        //    if(quest.state == Objective.ObjectiveState.complete)
        //    {
        //        finishedQuest = quest;
        //        break;
        //    }
        //}
        //if(finishedQuest != null)
        //{
        //    OnQuestCompleted(finishedQuest);
        //}
    }

    public void Display()
    {
        QuestsDisplay display = Instantiate(questsDisplayPrefab) as QuestsDisplay;
        display.Initialize(this);
    }

    public void Add(Quest newQuest)
    {
        if(newQuest == null)
        {
            return;
        }
        quests.Add(newQuest);
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
        quests.Remove(quest);
        if (OnChanged != null)
        {
            OnChanged(this);
        }
    }

    //private void OnQuestCompleted(Quest quest)
    //{
    //    print(string.Format("Quest completed! - {0}", quest.name));
    //    quests.Remove(quest);
    //}

    //public void AddQuest(Quest newQuest)
    //{
    //    quests.Add(newQuest);
    //}

    //public void SetCurrentQuest(Quest currentQuest)
    //{
    //    _currentQuest = currentQuest;
    //}
}
