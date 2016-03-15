using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissionDetails : MonoBehaviour
{
    public Text missionName;
    public Text missionDescription;
    public VerticalLayoutGroup objectivesVerticalGroup;
    public MissionObjective missionObjectivePrefab;

    private List<MissionObjective> objectives;

    void Start()
    {
        objectives = new List<MissionObjective>();
    }

    public void SetDetails(string name, string description, Dictionary<string, Objective.ObjectiveState> objectives)
    {
        ClearObjectives();
        missionName.text = name.ToUpper();
        missionDescription.text = description;
        foreach(var objective in objectives)
        {
            AddObjective(objective.Key, objective.Value);
        }
    }

    public void AddObjective(string description, Objective.ObjectiveState state)
    {
        MissionObjective objective = Instantiate(missionObjectivePrefab) as MissionObjective;
        objective.SetDescription(description);
        objectives.Add(objective);
        objective.transform.SetParent(objectivesVerticalGroup.transform);
        if (state.Equals(Objective.ObjectiveState.complete))
        {
            objective.IsCompleted = true;
        }
        else
        {
            objective.IsCompleted = false;
        }
    }

    public void ClearObjectives()
    {
        objectives.Clear();
        foreach(MissionObjective obj in objectivesVerticalGroup.GetComponentsInChildren<MissionObjective>())
        {
            Destroy(obj.gameObject);
        }
    }
}
