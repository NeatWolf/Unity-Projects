using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour {

    public Text textName;
    public Image sprite;

    public delegate void ListItemDisplayDelegate(Quest sender);
    public static event ListItemDisplayDelegate OnClick;

    public Quest quest;

	void Start ()
    {
	    if(quest != null)
        {
            Initialize(quest);
        }
	}
	
	void Update ()
    {
	
	}

    public void Initialize(Quest quest)
    {
        Debug.Log(string.Format("Initializing QuestDisplay ({0})", quest.questName));
        this.quest = quest;
        if(textName != null)
        {
            textName.text = quest.questName;
        }
        if(sprite != null)
        {
            sprite.sprite = quest.sprite;
        }
    }

    public void Click()
    {
        string displayName = "nothing";
        if(quest != null)
        {
            displayName = quest.questName;
        }
        if(OnClick != null)
        {
            Debug.Log(string.Format("Clicked on {0}", displayName));
            OnClick(quest);
        }
        else
        {
            Debug.Log(string.Format("Nobody is listening to {0}", displayName));
        }
    }
}
