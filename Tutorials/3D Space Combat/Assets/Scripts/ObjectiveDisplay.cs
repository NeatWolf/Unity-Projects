using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectiveDisplay : MonoBehaviour {

    public Text textName;
    public Image statusImage;
    public Sprite completedSprite;
    public Sprite inProgressSprite;

    [HideInInspector]
    public Objective objective;

    void Start ()
    {
        if (objective != null)
        {
            Initialize(objective);
        }
    }
	
	void Update ()
    {
	
	}

    public void Initialize(Objective objective)
    {
        this.objective = objective;
        if (textName != null)
        {
            textName.text = objective.description;
        }
        if (statusImage != null)
        {
            switch (objective.state)
            {
                case Objective.ObjectiveState.hidden:
                    gameObject.SetActive(false);
                    break;
                case Objective.ObjectiveState.active:
                    statusImage.sprite = inProgressSprite;
                    break;
                case Objective.ObjectiveState.complete:
                    statusImage.sprite = completedSprite;
                    break;
            }
            if(objective.state == Objective.ObjectiveState.complete)
            {
                statusImage.sprite = completedSprite;
            }
            else if (objective.state == Objective.ObjectiveState.active)
            {
                statusImage.sprite = inProgressSprite;
            }
        }
    }
}
