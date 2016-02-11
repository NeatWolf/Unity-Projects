using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float strafeSpeed;
    public float lookSpeed;
    public int verticalLookLimit;
    public float tilt;
    public float drag;

    private Rigidbody rb;
    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
    }

    void FixedUpdate()
    {
        float inputVertical = Input.GetAxis("Vertical");
        //float inputHorizontal = Input.GetAxis("Horizontal");

        #region Velocity Logic
        rb.AddForce(transform.forward * moveSpeed * inputVertical);
        //rb.AddForce(transform.right * strafeSpeed * inputHorizontal);
        //rb.velocity = (rb.rotation * Vector3.forward) * moveSpeed * inputVertical;
        //rb.velocity += (rb.rotation * Vector3.right) * strafeSpeed * inputHorizontal;
        #endregion

        #region Rotation Logic
        // Get the direction the mouse is pointing
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Quaternion rayDirection = Quaternion.LookRotation(ray.direction);

        // Clamp the direction on the x-axis (up/down) to prevent spiraling camera
        Vector3 directionEuler = rayDirection.eulerAngles;
        Quaternion clampedDirection = Quaternion.Euler(Mathf.Clamp(directionEuler.x > 180 ? directionEuler.x - 360 : directionEuler.x, -verticalLookLimit, verticalLookLimit), directionEuler.y, directionEuler.z);

        // Rotate along the z-axis to counteract x movement
        directionEuler = clampedDirection.eulerAngles;
        Quaternion tiltedDirection = Quaternion.Euler(directionEuler.x, directionEuler.y, transform.InverseTransformDirection(rb.velocity).x * tilt);

        // Apply the rotation
        rb.rotation = Quaternion.Slerp(rb.rotation, tiltedDirection, lookSpeed);
        #endregion
    }
}
