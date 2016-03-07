using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 initialPosition;
    private float _amplitude;
    private bool isShaking = false;

	void Start ()
    {
        initialPosition = transform.localPosition;
	}
	
	void Update ()
    {
        if (isShaking)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * _amplitude;
        }
	}

    public void StartShaking(float amplitude)
    {
        _amplitude = amplitude;
        isShaking = true;
    }

    public void StopShaking()
    {
        isShaking = false;
    }
}
