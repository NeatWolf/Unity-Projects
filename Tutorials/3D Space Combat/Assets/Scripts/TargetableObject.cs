using UnityEngine;
using System.Collections;

public class TargetableObject : MonoBehaviour {

    [SerializeField]
    private Enums.Allegiance allegiance;

    public Enums.Allegiance Allegiance { get { return allegiance; } }

    void Start()
    {
        GameManager.instance.AddTargetableObject(this);
    }

    void OnDestroy()
    {
        GameManager.instance.RemoveTargetableObject(this);
    }
}
