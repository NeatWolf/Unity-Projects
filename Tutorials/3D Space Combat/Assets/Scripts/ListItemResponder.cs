using UnityEngine;
using System.Collections;

public class ListItemResponder : MonoBehaviour
{
    public QuestDetailsScreen questDetailsScreenPrefab;

	void Start ()
    {
        Debug.Log(string.Format("Subscribed to OnClick"));
        QuestDisplay.OnClick += ListItemDisplay_OnClick;
	}

    void OnDestroy()
    {
        Debug.Log("Unsubscribe from OnClick");
        QuestDisplay.OnClick -= ListItemDisplay_OnClick;
    }

    void Update ()
    {
	
	}

    private void ListItemDisplay_OnClick(Quest senderItem)
    {
        Debug.Log(string.Format("Handle click from {0}", senderItem.questName));
    }
}
