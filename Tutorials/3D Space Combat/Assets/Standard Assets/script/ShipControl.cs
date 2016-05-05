using UnityEngine;
using System.Collections;

public class ShipControl : MonoBehaviour
{

    // Use this for initialization
    public float speed = 0;
    public float MaxSpeed = 1000;
    public float RotationSpeed = 5;
    public float mouseDelta = 100;
    public float Rdamping = 0.1f;
    public Camera cam;
    private float rotationz;
    private Vector3 moveDirection;
    private Vector3 screenPosition;
    Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        body.angularDrag = 1;
        body.drag = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (speed <= MaxSpeed)
        {
            speed = speed + Input.GetAxis("Vertical");
            if (speed > 0)
            {

            }
            if (speed > MaxSpeed)
            {
                speed = MaxSpeed;

            }
            if (speed < 0)
            {
                speed = 0;

            }
        }

    }
    void FixedUpdate()
    {
        moveDirection = Vector3.forward * speed;
        moveDirection = transform.TransformDirection(moveDirection);
        Rotation();
        body.AddForce(moveDirection);
    }

    void Rotation()
    {
        screenPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1900));
        Vector3 direction = screenPosition - transform.position;
        rotationz = Screen.width * 0.5f;
        if (rotationz < Screen.width - Input.mousePosition.x - mouseDelta)
        {
            body.AddRelativeTorque(0, 0, RotationSpeed);
        }
        if (rotationz > Screen.width - Input.mousePosition.x + mouseDelta)
        {
            body.AddRelativeTorque(0, 0, -RotationSpeed);
        }
        body.isKinematic = true;
        body.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, transform.up), Rdamping);
        body.isKinematic = false;
    }
}
