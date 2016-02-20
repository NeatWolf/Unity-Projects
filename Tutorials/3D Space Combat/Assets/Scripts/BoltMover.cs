using UnityEngine;
using System.Collections;

public class BoltMover : MonoBehaviour {

    public float speed;
    public float timeoutToDestroy;

	void Start()
    {
        Destroy(gameObject, timeoutToDestroy);
	}

    void Update()
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime);
    }
}
