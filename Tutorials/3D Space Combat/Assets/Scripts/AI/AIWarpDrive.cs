using UnityEngine;
using System.Collections;

/// <summary>
/// A warp drive that allows AI ships to enter and exit the scene by entering and exiting warp
/// </summary>
public class AIWarpDrive : MonoBehaviour {

    public float distance = 1000f;
    public float time = 0.5f;

    private bool _isEntering;
    private Vector3 _enterPosition;
    private Rigidbody rb;

	void Awake ()
    {
        rb = GetComponent<Rigidbody>();
	}

    public void EnterFromWarp(Vector3 position, Quaternion rotation)
    {
        StartCoroutine(PerformEnterFromWarp(distance, time));
    }

    public void ExitToWarp()
    {
        PerformExitToWarp(distance, time);
    }

    IEnumerator PerformEnterFromWarp(float distance, float time)
    {
        Vector3 startPosition = transform.position - (transform.forward * distance);
        Vector3 endPosition = transform.position;
        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);
            yield return null;
        }
    }

    IEnumerator PerformExitToWarp(float distance, float time)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + (transform.forward * distance);
        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);
            yield return null;
        }

        Destroy(gameObject);
    }
}
