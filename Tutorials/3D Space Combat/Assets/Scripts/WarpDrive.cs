using UnityEngine;
using System.Collections;

public class WarpDrive : MonoBehaviour {

    public float speedUponExit;

    private Rigidbody rb;
    private bool _isWarpEnabled = true;

    public bool IsWarpEnabled
    {
        get
        {
            return _isWarpEnabled;
        }
        set
        {
            _isWarpEnabled = value;
            if (!value)
            {
                rb.velocity = Vector3.zero;
                rb.Sleep();
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(_isWarpEnabled && Input.GetKey(KeyCode.Space))
        {
            //warpParticleSystem.Play();
            //rb.velocity = transform.forward * 50000;
        }
    }
}
