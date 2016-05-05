using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public float distance = 50;
    public float height = 5;
    public float damping = 5;
    public float rotationDamping = 20;
    private Vector3 wantedPosition;
    private Quaternion wantedRotation;

    void Update()
    {

        if (height < 0 && distance > 100)
        {
            distance = 100;
            height = 0;
        }
        if (height > 10 && distance < 50)
        {
            distance = 50;
            height = 10;
        }
        if (height >= 0 && height <= 10)
        {
            distance += (Input.GetAxis("Mouse ScrollWheel")) * 10;
            height += -(Input.GetAxis("Mouse ScrollWheel")) * 2;

        }

    }

    void FixedUpdate()
    {
        wantedPosition = target.TransformPoint(0, height, -distance);
        transform.localPosition = Vector3.Lerp(transform.position, wantedPosition, damping * Time.deltaTime);
        wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
        transform.localRotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationDamping * Time.deltaTime);
    }
}
