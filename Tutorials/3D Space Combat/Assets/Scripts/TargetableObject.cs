using UnityEngine;
using System.Collections;

public class TargetableObject : MonoBehaviour {

    public Allegiance allegiance;

    public enum Allegiance
    {
        Friendly,
        Enemy
    }
}
