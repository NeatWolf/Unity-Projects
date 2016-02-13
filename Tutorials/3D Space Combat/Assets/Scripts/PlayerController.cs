using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float idleSpeed;
    public float moveSpeed;
    public float strafeSpeed;
    public float warpSpeed;
    public float lookSpeed;
    public int verticalLookLimit;
    public float tilt;
    public ParticleSystem warpParticleSystem;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float inputVertical = Input.GetAxis("Vertical");
        //float inputHorizontal = Input.GetAxis("Horizontal");

        #region Velocity Logic
        // Add base force
        rb.AddForce(transform.forward * idleSpeed);

        // Add forward or backward force
        if(inputVertical > 0)
        {
            rb.AddForce(transform.forward * moveSpeed * inputVertical);

            // Add warp speed force
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.AddForce(transform.forward * warpSpeed);
                warpParticleSystem.Play();
            }
            else
            {
                warpParticleSystem.Stop();
            }
        }
        else
        {
            rb.AddForce(transform.forward * idleSpeed * inputVertical);
        }

        //rb.AddForce(transform.right * strafeSpeed * inputHorizontal);
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
