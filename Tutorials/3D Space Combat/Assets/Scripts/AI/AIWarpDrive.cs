using UnityEngine;
using System.Collections;

/// <summary>
/// A warp drive that allows AI ships to enter and exit the scene by entering and exiting warp
/// </summary>
public class AIWarpDrive : MonoBehaviour {

    public float enterDistance = 1000f;
    public float warpSpeed = 1000f;
    public float enterSpeed = 10f;

    private bool _isEntering;
    private Vector3 _enterPosition;
    private Rigidbody rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
	    if (_isEntering)
        {
            if (Vector3.Distance(transform.position, _enterPosition) < 50f)
            {
                _isEntering = false;
                rb.velocity = Vector3.forward * enterSpeed;
            }
        }
	}

    public void EnterFromWarp(Vector3 position, Quaternion rotation)
    {
        Quaternion inverseQuaternion = Quaternion.Inverse(rotation);
        _enterPosition = transform.position;
        transform.position = transform.position + (transform.forward * enterDistance);
        rb.velocity = Vector3.forward * warpSpeed;
        _isEntering = true;
    }

    public void ExitToWarp()
    {
        PerformExitToWarp();
    }

    IEnumerator PerformExitToWarp()
    {
        rb.velocity = Vector3.forward * warpSpeed;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
