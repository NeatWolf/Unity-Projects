using UnityEngine;
using System.Collections;

public class DestroyOnTimeout : MonoBehaviour {

    public float timeoutToDestroy;

    void Start ()
    {
        Destroy(gameObject, timeoutToDestroy);
    }
}
