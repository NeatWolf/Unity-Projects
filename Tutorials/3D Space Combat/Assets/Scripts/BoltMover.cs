using UnityEngine;
using System.Collections;

public class BoltMover : MonoBehaviour {

    public float speed;
    public float timeoutToDestroy;

    private Rigidbody rb;

	void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, timeoutToDestroy);
        rb.AddForce(transform.forward * speed * 30f);
    }
}
