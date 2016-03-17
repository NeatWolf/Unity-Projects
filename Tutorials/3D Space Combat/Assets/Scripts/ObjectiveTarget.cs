using UnityEngine;
using System.Collections;

public class ObjectiveTarget : MonoBehaviour {

    public bool visibleIndicator;
    //public GameObject targetGameObject;
    public Objective.ObjectiveState state;
    public Objective.ObjectiveType kind;

    void Start()
    {
        state = Objective.ObjectiveState.active;
        visibleIndicator = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (kind == Objective.ObjectiveType.collect || kind == Objective.ObjectiveType.travel))
        {
            state = Objective.ObjectiveState.complete;
        }
    }

    public void Initialize(Objective.ObjectiveState state, bool visibleIndicator)
    {
        this.state = state;
        this.visibleIndicator = visibleIndicator;
    }
}
