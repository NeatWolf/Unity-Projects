using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Objective : MonoBehaviour
{
    public int index;
    public string description;
    public ObjectiveType kind;
    public ObjectiveState state;
    public Objective nextObjective;
    public ObjectiveTarget[] targets;

    private float progress = 0f;

    public delegate void ObjectiveCompletedDelegate(Objective sender);
    public event ObjectiveCompletedDelegate OnCompleted;

    public Quest ParentScript { get; set; }

    public enum ObjectiveState
    {
        inactive,
        active,
        complete
    }

    public enum ObjectiveType
    {
        travel,
        destroy,
        talk,
        collect
    }

    void Update()
    {
        if(targets != null && targets.Length > 0)
        {
            switch (kind)
            {
                case ObjectiveType.destroy:
                    progress = 0f;
                    foreach (var target in targets)
                    {
                        if (target == null || target.gameObject == null)
                        {
                            progress += 1f / targets.Length;
                        }
                    }
                    break;
                case ObjectiveType.travel:
                    progress = 0f;
                    
                    foreach(var target in targets)
                    {
                        if (target.state == ObjectiveState.complete)
                        {
                            progress += 1f / targets.Length;
                        }
                    }
                    break;
                case ObjectiveType.talk:
                    break;
                case ObjectiveType.collect:
                    break;
            }
        }

        if (Mathf.Approximately(1f, progress) && state != ObjectiveState.complete)
        {
            state = ObjectiveState.complete;
            OnCompletedObjective();
        }
    }

    private void OnCompletedObjective()
    {
        if(nextObjective != null)
        {
            ParentScript.currentObjective = nextObjective;
            ParentScript.currentObjective.state = ObjectiveState.active;
        }
        else
        {
            // All objectives complete, end quest
            ParentScript.OnObjectivesCompleted();
        }
        if(OnCompleted != null)
        {
            OnCompleted(this);
        }
        print(string.Format("completed objective: {0}", description));
    }

    public void AssignTargets(ObjectiveTarget[] targets)
    {
        this.targets = targets;
        foreach(var target in this.targets)
        {
            target.kind = kind;
        }
    }

    internal void AssignTarget(ObjectiveTarget target)
    {
        target.kind = kind;
        targets = new ObjectiveTarget[] { target };
    }
}
