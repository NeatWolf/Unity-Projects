using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]
public class AINavigator : MonoBehaviour {

    public float forwardSpeed = 1000f;
    public float rotateSpeed = 1f;
    public float avoidanceSpeed = 50f;
    public float avoidanceSpread = 5f;
    public float avoidanceRange = 400f;

    private Rigidbody _rb;
    private bool _isActive = false;
    private Vector3 _destination;
    private float _avoidanceDirection;
    private float _originalAvoidanceSpeed;

    //private static readonly float HARD_AVOIDANCE_SPREAD = 6f;
    //private static readonly float HARD_AVOIDANCE_RANGE = 8f;

    void Start ()
    {
        _rb = GetComponent<Rigidbody>();
        _originalAvoidanceSpeed = avoidanceSpeed;
	}

    void FixedUpdate()
    {
        if (_isActive)
        {
            Vector3 targetDir = _destination - transform.position;

            // Dot product determines if we are looking at the target
            // A return value of 1 would indicate we are looking right at it
            if (Vector3.Dot(targetDir, transform.forward) > 0.8f)
            {
                _rb.AddForce(transform.forward * forwardSpeed);
            }

            PerformRotation();
        }
    }

    private void PerformRotation()
    {
        RaycastHit forwardHit, rightHit, leftHit, hardRightHit, hardLeftHit;
        bool forwardObstacle = Physics.Raycast(transform.position, transform.forward, out forwardHit, avoidanceRange);
        bool rightObstacle = Physics.Raycast(transform.position, Quaternion.AngleAxis(avoidanceSpread, Vector3.up) * transform.forward, out rightHit, avoidanceRange);
        bool leftObstacle = Physics.Raycast(transform.position, Quaternion.AngleAxis(-avoidanceSpread, Vector3.up) * transform.forward, out leftHit, avoidanceRange);
        //bool hardRightObstacle = Physics.Raycast(transform.position, Quaternion.AngleAxis(HARD_AVOIDANCE_SPREAD * avoidanceSpread, Vector3.up) * transform.forward, out hardRightHit, avoidanceRange);
        //bool hardLeftObstacle = Physics.Raycast(transform.position, Quaternion.AngleAxis(HARD_AVOIDANCE_SPREAD * -avoidanceSpread, Vector3.up) * transform.forward, out hardLeftHit, avoidanceRange);

        Debug.DrawLine(transform.position, transform.position + transform.forward * avoidanceRange);
        Debug.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(avoidanceSpread, Vector3.up) * transform.forward * avoidanceRange);
        Debug.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-avoidanceSpread, Vector3.up) * transform.forward * avoidanceRange);
        //Debug.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(HARD_AVOIDANCE_SPREAD * avoidanceSpread, Vector3.up) * transform.forward * (avoidanceRange / HARD_AVOIDANCE_RANGE));
        //Debug.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(HARD_AVOIDANCE_SPREAD * -avoidanceSpread, Vector3.up) * transform.forward * (avoidanceRange / HARD_AVOIDANCE_RANGE));

        if (forwardObstacle)
        {
            Debug.Log("Npc collider hit: " + forwardHit.transform.gameObject.name);
        }

        if (_avoidanceDirection != 0f ||
            (forwardObstacle && !forwardHit.collider.CompareTag("NpcAccessPoint"))
            || (rightObstacle && !rightHit.collider.CompareTag("NpcAccessPoint"))
            || (leftObstacle && !leftHit.collider.CompareTag("NpcAccessPoint")))
        {
            if (rightObstacle || leftObstacle)
            {
                _avoidanceDirection = 0f;
            }

            if (forwardObstacle || _avoidanceDirection != 0f)
            {
                if (_avoidanceDirection == 0f)
                {
                    _avoidanceDirection = Random.Range(0, 1) == 0 ? avoidanceSpeed : -avoidanceSpeed;
                }
                _rb.AddTorque(0f, _avoidanceDirection, 0f);
            }
            else if (rightObstacle && !leftObstacle)
            {
                _avoidanceDirection = 0f;
                _rb.AddTorque(0f, -avoidanceSpeed, 0f);
            }
            else if (leftObstacle && !rightObstacle)
            {
                _avoidanceDirection = 0f;
                _rb.AddTorque(0f, avoidanceSpeed, 0f);
            }
        }
        else
        {
            _avoidanceDirection = 0f;
            avoidanceSpeed = _originalAvoidanceSpeed;
            Vector3 targetDir = _destination - transform.position;
            _rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), rotateSpeed * Time.deltaTime));
        }
    }

    public void Start(Vector3 destination)
    {
        _destination = destination;
        _isActive = true;
    }

    public void Stop()
    {
        _isActive = false;
    }
}
