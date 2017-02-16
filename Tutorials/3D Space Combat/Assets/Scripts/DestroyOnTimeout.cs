using UnityEngine;
using System.Collections;

public class DestroyOnTimeout : MonoBehaviour {

    [SerializeField]
    private float timeoutToDestroy;

    void Start ()
    {
        Destroy(gameObject, timeoutToDestroy);
    }
}
