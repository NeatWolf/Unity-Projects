using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectiveDisplay : MonoBehaviour {

    public Text textName;
    public Image sprite;

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
        if (sprite != null)
        {
            //sprite.sprite = objective.sprite;
        }
    }
}
