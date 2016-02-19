using UnityEngine;
using System.Collections;

public class BoltMover : MonoBehaviour {

    public float speed;
    public float timeoutToDestroy;

    //private Rigidbody rb;

	void Start()
    {
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = transform.forward * speed;
        Destroy(gameObject, timeoutToDestroy);
	}

    void Update()
    {
        float moveX = transform.rotation.eulerAngles.y;
        float moveY = transform.rotation.eulerAngles.x;
        transform.Translate(0f, 0f, speed * Time.deltaTime);
    }
}
