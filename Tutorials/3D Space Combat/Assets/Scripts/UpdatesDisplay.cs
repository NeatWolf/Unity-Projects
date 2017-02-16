using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UpdatesDisplay : MonoBehaviour {

    public int maxRows;
    public float animationTime;
    public Vector2 localStartPosition;
    public UpdateListItem updateListItemPrefab;

    private Queue<UpdateListItem> itemList = new Queue<UpdateListItem>();
    private Queue<UpdateListItem> itemsToBeAddedQueue = new Queue<UpdateListItem>();
    private float timer = 0f;

	void Start ()
    {
        GameManager.questManager.OnQuestAdd += QuestManager_OnQuestAdd;
        //UpdateListItem updateLine;
        //for (int i = 1; i < 10; i++)
        //{
        //    updateLine = Instantiate(updateListItemPrefab) as UpdateListItem;
        //    updateLine.transform.SetParent(transform);
        //    updateLine.animateTime = animationTime;
        //    StartCoroutine(ShowListItemDelay(updateLine, i));
        //}
        //updateLine = Instantiate(updateListItemPrefab) as UpdateListItem;
        //updateLine.transform.SetParent(transform);
        //updateLine.animateTime = animationTime;
        //StartCoroutine(ShowListItemDelay(updateLine, 9));
    }

    void OnDestroy()
    {
        GameManager.questManager.OnQuestAdd -= QuestManager_OnQuestAdd;
    }

    void Update ()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0f)
        {
            if (itemsToBeAddedQueue.Count > 0)
            {
                ShowListItemOnScreen(itemsToBeAddedQueue.Dequeue());
                timer = animationTime + 0.1f;
            }
        }
	}

    private void QuestManager_OnQuestAdd(Quest addedQuest)
    {
        // Subscribe to quest events
        addedQuest.OnCompleted += Quest_OnCompleted;
        foreach (Objective obj in addedQuest.objectives)
        {
            Debug.Log("Subscribed to objective");
            obj.OnCompleted += Objective_OnCompleted;
            obj.OnStarted += Objective_OnStarted;
        }

        // Notification of new quest
        UpdateListItem updateLine = Instantiate(updateListItemPrefab) as UpdateListItem;
        updateLine.transform.SetParent(transform);
        updateLine.AnimateTime = animationTime;
        updateLine.Text = "New Quest Added";
        itemsToBeAddedQueue.Enqueue(updateLine);
    }

    private void Quest_OnCompleted(Quest sender)
    {
        UpdateListItem updateLine = Instantiate(updateListItemPrefab) as UpdateListItem;
        updateLine.transform.SetParent(transform);
        updateLine.AnimateTime = animationTime;
        updateLine.Text = "Quest completed!";
        sender.OnCompleted -= Quest_OnCompleted;
        itemsToBeAddedQueue.Enqueue(updateLine);
    }

    private void Objective_OnCompleted(Objective sender)
    {
        UpdateListItem updateLine = Instantiate(updateListItemPrefab) as UpdateListItem;
        updateLine.transform.SetParent(transform);
        updateLine.AnimateTime = animationTime;
        updateLine.Text = "Completed: " + sender.description;
        sender.OnCompleted -= Objective_OnCompleted;
        itemsToBeAddedQueue.Enqueue(updateLine);
    }

    private void Objective_OnStarted(Objective sender)
    {
        UpdateListItem updateLine = Instantiate(updateListItemPrefab) as UpdateListItem;
        updateLine.transform.SetParent(transform);
        updateLine.AnimateTime = animationTime;
        updateLine.Text = "Current Objective: " + sender.description;
        sender.OnStarted -= Objective_OnStarted;
        itemsToBeAddedQueue.Enqueue(updateLine);
    }

    IEnumerator ShowListItemDelay(UpdateListItem listItem, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        itemsToBeAddedQueue.Enqueue(listItem);
    }

    private void ShowListItemOnScreen(UpdateListItem listItem)
    {
        listItem.GetComponent<RectTransform>().anchoredPosition = localStartPosition;
        itemList.Enqueue(listItem);
        listItem.AddToTop();
        while (itemList.Count > maxRows)
        {
            itemList.Dequeue().RemoveFromBottom();
        }
        foreach(var item in itemList)
        {
            item.MoveDown();
        }
    }
}
