using UnityEngine;
using System.Collections;

public class AISimple : MonoBehaviour {

    public Transform target;
    public float lookAtDistance;
    public float attackRange;
    public float moveSpeed;
    public float damping;

    private float distance;
    private Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        distance = Vector3.Distance(target.position, transform.position);

        if(distance < lookAtDistance)
        {
            rb.rotation = Quaternion.LookRotation(target.position);
        }
	}
}
