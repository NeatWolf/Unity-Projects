using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float idleSpeed;
    public float moveSpeed;
    public float strafeSpeed;
    public float boostSpeed;
    public float warpSpeed;
    public float lookSpeed;
    public int verticalLookLimit;
    public float tilt;
    public float zRealignWaitTime;
    public ParticleSystem warpParticleSystem;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        zRealignWaitTime = Time.time + zRealignWaitTime;
    }

    void FixedUpdate()
    {
        // Don't allow any other controls when in warp speed
        if (Input.GetKey(KeyCode.Space))
        {
            warpParticleSystem.Play();
            rb.AddForce(transform.forward * warpSpeed);
        }
        else
        {
            warpParticleSystem.Stop();
            float inputVertical = Input.GetAxis("Vertical");
            float inputHorizontal = Input.GetAxis("Horizontal");
            Vector3 mousePosition = Input.mousePosition;

            #region Velocity Logic
            // Add base force
            rb.AddForce(transform.forward * idleSpeed);

            // Add forward or backward force
            if (inputVertical > 0)
            {
                rb.AddForce(transform.forward * moveSpeed * inputVertical);

                // Add boost speed force
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    rb.AddForce(transform.forward * boostSpeed);
                }
            }
            else
            {
                rb.AddForce(transform.forward * idleSpeed * inputVertical);
            }

            rb.AddForce(transform.right * strafeSpeed * inputHorizontal);
            #endregion

            #region Rotation Logic
            // Get the direction the mouse is pointing
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            Quaternion rayDirection = Quaternion.LookRotation(ray.direction);
            //rb.AddTorque(ray.direction.x * lookSpeed, ray.direction.y * lookSpeed, 0f);

            // Clamp the direction on the x-axis (up/down) to prevent spiraling camera
            Vector3 directionEuler = rayDirection.eulerAngles;
            Quaternion modifiedDirection = Quaternion.Euler(Mathf.Clamp(directionEuler.x > 180 ? directionEuler.x - 360 : directionEuler.x, -verticalLookLimit, verticalLookLimit), directionEuler.y, directionEuler.z);

            // Rotate along the z-axis to counteract x movement
            directionEuler = modifiedDirection.eulerAngles;
            //print(string.Format("z-axis rotation: {0}", transform.InverseTransformDirection(rb.velocity).x * tilt));
            modifiedDirection = Quaternion.Euler(directionEuler.x, directionEuler.y, transform.InverseTransformDirection(rb.velocity).x * tilt);

            // Rotate back to 0 if it has been knocked out of rotation
            float distanceFromCenterX = Mathf.Abs((Screen.width / 2) - mousePosition.x);
            float distanceFromCenterY = Mathf.Abs((Screen.height / 2) - mousePosition.y);

            //print(string.Format("distance from center = ({0}, {1})", distanceFromCenterX, distanceFromCenterY));
            if (inputHorizontal == 0 && distanceFromCenterX < 32 && distanceFromCenterY < 32)
            {
                modifiedDirection = Quaternion.Euler(modifiedDirection.eulerAngles.x, modifiedDirection.eulerAngles.y, 0f);
            }

            // Apply the rotation
            rb.rotation = Quaternion.Slerp(rb.rotation, modifiedDirection, lookSpeed);
            //rb.AddTorque(modifiedDirection.eulerAngles * lookSpeed);
            #endregion
        }
    }
}
