using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float idleSpeed = 100.0f;
    public float moveSpeed = 200.0f;
    public float strafeSpeed = 100.0f;
    public float combatBoostSpeed = 400.0f;
    public float boostSpeed = 1000.0f;
    public float lookSpeed = 0.1f;
    public int verticalLookLimit = 60;
    public float tilt = 5.0f;

    private Rigidbody rb;
    private WarpDrive warpDrive;
    private bool movementLocked = false;
    private State currentState = State.Default;
    
    private enum State
    {
        Default,
        WarpStandby
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        warpDrive = GetComponent<WarpDrive>();
    }

    void Update()
    {
        // Lock onto warp target
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Cancel lock
            if(currentState == State.WarpStandby)
            {
                LockMovement(false);
                currentState = State.Default;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                WarpTarget target = hit.collider.gameObject.GetComponent<WarpTarget>();
                if(target != null)
                {
                    LockMovement(true);
                    StartCoroutine(RotateTowards(target.targetTransform.position));
                    warpDrive.TargetPosition = target.targetTransform.position;
                    currentState = State.WarpStandby;
                }
            }
        }

        if(currentState == State.WarpStandby)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                warpDrive.Engage();
                LockMovement(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (!movementLocked)
        {
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

        //print(string.Format("position = {0}", transform.position.ToString()));
    }

    public void LockMovement(bool setting)
    {
        movementLocked = setting;
    }

    IEnumerator RotateTowards(Vector3 lookAtPosition)
    {
        //Vector3 direction = (lookAtPosition - transform.position).normalized;
        //Quaternion directionQuaternion = Quaternion.Euler(direction.x, direction.y, direction.z);
        Quaternion direction = Quaternion.LookRotation(lookAtPosition);
        print(string.Format("rotation: {0}, desired rotation: {1}", rb.rotation.ToString(), direction.ToString()));
        while (rb.rotation != direction)
        {
            //print(string.Format("rotation: {0}", rb.rotation));
            rb.rotation = Quaternion.Slerp(rb.rotation, direction, lookSpeed);
        }
        yield return null;
    }
}
