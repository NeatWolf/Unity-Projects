using UnityEngine;
using System.Collections;

public class WarpTarget : MonoBehaviour {

    [SerializeField]
    private string targetName;
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private Collider targetBoundary;

    public string TargetName { get { return targetName; } }

    public Bounds Bounds { get { return targetBoundary.bounds; } }

    public Vector3 Position { get { return targetTransform.position; } }
}
