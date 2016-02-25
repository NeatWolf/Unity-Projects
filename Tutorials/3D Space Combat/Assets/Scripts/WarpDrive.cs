using UnityEngine;
using System.Collections;

public class WarpDrive : MonoBehaviour {

    public ParticleSystem warpParticleSystem;

    private Rigidbody rb;
    private Vector3 _targetPosition = Vector3.zero;

    public Vector3 TargetPosition
    {
        get
        {
            return _targetPosition;
        }
        set
        {
            _targetPosition = value;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Engage()
    {
        if(TargetPosition != Vector3.zero)
        {
            transform.position = TargetPosition;
            TargetPosition = Vector3.zero;
        }
    }
}
