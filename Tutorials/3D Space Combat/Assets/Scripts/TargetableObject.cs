using UnityEngine;
using System.Collections;

public class TargetableObject : MonoBehaviour {

    [SerializeField]
    private Enums.Allegiance allegiance;

    public Enums.Allegiance Allegiance { get { return allegiance; } }
}
