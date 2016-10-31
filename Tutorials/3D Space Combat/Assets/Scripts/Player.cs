using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    public float idleSpeed = 100.0f;
    public float moveSpeed = 200.0f;
    public float strafeSpeed = 100.0f;
    public float strafeRotation = 5f;
    public float combatBoostSpeed = 400.0f;
    public float boostSpeed = 1000.0f;
    public float lookSpeed = 0.1f;
    public int verticalLookLimit = 60;
    public float tilt = 5.0f;
    public CameraController cameraController;
    public Camera cam;
    public Transform cameraTarget;
    public ParticleSystem[] thrusters;
    public float mouseDelta = 100;
    public float lookDamping = 0.1f;
    public float rotationDamping = 20f;
    public float rotationSpeed = 5f;
    public AudioMixerSnapshot thrusterOnAudio;
    public AudioMixerSnapshot thrusterOffAudio;

    private Rigidbody rb;
    private WarpDrive warpDrive;
    private float rotationz;
    private Vector3 screenPosition;
    private bool movementLocked = false;
    private bool controlsLocked = false;
    private State currentState = State.Default;
    private AudioSource _audioSource;

    private float startingBoostSpeed;
    private float startingCombatBoostSpeed;

    private enum State
    {
        Default,
        WarpStandby,
        Docked,
        Boosting
    }

    public bool IsMovementLocked
    {
        get
        {
            return movementLocked;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        warpDrive = GetComponent<WarpDrive>();
        _audioSource = GetComponent<AudioSource>();
        startingBoostSpeed = boostSpeed;
        startingCombatBoostSpeed = combatBoostSpeed;
        boostSpeed = 0f;
        combatBoostSpeed = 0f;
    }

    void Update()
    {
        if(currentState == State.Docked)
        {
            if (!controlsLocked && Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.instance.UndockPlayer();
            }
            else
            {
                return;
            }
        }

        // Lock onto warp target
        if (!controlsLocked && Input.GetKeyDown(KeyCode.Tab))
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
                    warpDrive.SetTarget(target.targetTransform.position);
                    StartCoroutine(RotateTowards(target.targetTransform.position));
                    currentState = State.WarpStandby;
                }
            }
        }

        // Engage warp
        if(currentState == State.WarpStandby)
        {
            if (!controlsLocked && Input.GetKeyDown(KeyCode.Space))
            {
                warpDrive.Engage();
            }

            if (warpDrive.State == Enums.WarpDriveState.waitingForCommand)
            {
                warpDrive.PowerDown();
                LockMovement(false);
                currentState = State.Default;
            }
        }

        // Check for enemies that are near
        TargetableObject[] objects = GameObject.FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[];

        if(objects.Length <= 0)
        {
            GameManager.instance.isInCombat = false;
        }
        else
        {
            foreach (TargetableObject obj in objects)
            {
                if (obj.allegiance != TargetableObject.Allegiance.Friendly)
                {
                    if (Vector3.Distance(obj.transform.position, transform.position) < 500)
                    {
                        GameManager.instance.isInCombat = true;
                        break;
                    }
                }
                else
                {
                    GameManager.instance.isInCombat = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 forwardForce = Vector3.zero;

        if (!movementLocked)
        {
            float inputVertical = Input.GetAxis("Vertical");
            float inputHorizontal = Input.GetAxis("Horizontal");
            Vector3 mousePosition = Input.mousePosition;

            // Add base force
            forwardForce += transform.forward * idleSpeed;

            // Add forward or backward force
            if (inputVertical > 0)
            {
                //thrusterOnAudio.TransitionTo(0.5f);
                forwardForce += transform.forward * moveSpeed * inputVertical;

                // Add boost speed force
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if(currentState != State.Boosting)
                    {
                        StartCoroutine(LerpBoost(1f, 0f, GameManager.instance.isInCombat ? startingCombatBoostSpeed : startingBoostSpeed, GameManager.instance.isInCombat));
                        cameraController.EnterBoost();
                    }
                    currentState = State.Boosting;
                    GameManager.instance.isShootingEnabled = false;


                }
                else
                {
                    if(currentState == State.Boosting)
                    {
                        StartCoroutine(LerpBoost(1f, GameManager.instance.isInCombat ? startingCombatBoostSpeed : startingBoostSpeed, 0f, GameManager.instance.isInCombat));
                        cameraController.ExitBoost();

                        currentState = State.Default;
                        GameManager.instance.isShootingEnabled = true;
                    }

                }
            }
            else
            {
                //thrusterOffAudio.TransitionTo(1f);
                forwardForce += transform.forward * idleSpeed * inputVertical;

                if (currentState == State.Boosting)
                {
                    StartCoroutine(LerpBoost(1f, GameManager.instance.isInCombat ? startingCombatBoostSpeed : startingBoostSpeed, 0f, GameManager.instance.isInCombat));
                    cameraController.ExitBoost();

                    currentState = State.Default;
                    GameManager.instance.isShootingEnabled = true;
                }
            }

            if (GameManager.instance.isInCombat)
            {
                forwardForce += transform.forward * combatBoostSpeed;
            }
            else
            {
                forwardForce += transform.forward * boostSpeed;
            }

            Rotation(inputHorizontal);
            rb.AddForce(forwardForce);
            rb.AddForce(transform.right * strafeSpeed * inputHorizontal);
            _audioSource.volume = Mathf.Clamp(Vector3.SqrMagnitude(forwardForce) / 882000f, 0f, 0.15f);
            //Debug.Log(Vector3.SqrMagnitude(forwardForce));
        }

        
        foreach(var thruster in thrusters)
        {
            // For moving speed we want lifetime at 0.5, for idle speed we want lifetime at 0.4
            thruster.startLifetime = Mathf.Clamp(Mathf.Log10(forwardForce.sqrMagnitude / 83.965f) / Mathf.Log10(275855), 0.1f, 0.7f);
        }
    }

    private void Rotation(float inputHorizontal)
    {
        screenPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1900));
        Vector3 direction = screenPosition - transform.position;
        rotationz = Screen.width * 0.5f;

        //rb.isKinematic = true;
        rb.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, transform.up), lookDamping);
        
        // Rotate player around z-axis when turning with the mouse. Compensate the camera by rotating the camera target in the opposite direction
        rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles.x, rb.transform.rotation.eulerAngles.y, (-inputHorizontal * strafeRotation) + (((Screen.width * 0.5f) - Input.mousePosition.x) / rotationSpeed));
        cameraTarget.localRotation = Quaternion.Euler(0f, 0f, (inputHorizontal * strafeRotation) - ((Screen.width * 0.5f) - Input.mousePosition.x) / (rotationSpeed));
        //rb.isKinematic = false;

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    transform.Rotate(Vector3.forward * Time.deltaTime * rotationDamping);
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    transform.Rotate(Vector3.back * Time.deltaTime * rotationDamping);
        //}
    }

    public void LockMovement(bool isLocked)
    {
        movementLocked = isLocked;
    }

    public void LockControls(bool isLocked)
    {
        controlsLocked = isLocked;
    }

    public IEnumerator LockControlsDelayed(bool isLocked, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        LockControls(isLocked);
    }

    public void Dock(Transform dockingTransform)
    {
        StartCoroutine(PerformDock(dockingTransform, 3f));
    }

    public void Undock()
    {
        StartCoroutine(PerformUndock(3f));
    }

    IEnumerator RotateTowards(Vector3 lookAtPosition)
    {
        //Vector3 direction = (lookAtPosition - transform.position).normalized;
        //Quaternion directionQuaternion = Quaternion.Euler(direction.x, direction.y, direction.z);
        Quaternion direction = Quaternion.LookRotation(lookAtPosition);
        while (Quaternion.Angle(rb.rotation, direction) > 0.1)
        {
            //print(string.Format("rotation: {0}", rb.rotation));
            rb.rotation = Quaternion.Slerp(rb.rotation, direction, lookSpeed);
        }
        yield return null;
    }

    IEnumerator LerpBoost(float time, float startValue, float endValue, bool isInCombat)
    {
        float startSpeed = startValue;
        float endSpeed = endValue;

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            if (isInCombat)
            {
                combatBoostSpeed = Mathf.Lerp(startSpeed, endSpeed, percentageComplete);
            }
            else
            {
                boostSpeed = Mathf.Lerp(startSpeed, endSpeed, percentageComplete);
            }
            yield return null;
        }
    }

    #region Docking coroutines
    IEnumerator PerformDock(Transform dockingTransform, float time)
    {
        Debug.Log("Player docking coroutine");
        currentState = State.Docked;
        LockMovement(true);
        GameManager.instance.isShootingEnabled = false;
        GameManager.instance.isCursorVisible = false;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 midPosition = dockingTransform.position + new Vector3(0f, 10f, 0f);
        Vector3 endPosition = dockingTransform.position;
        Quaternion endRotation = dockingTransform.rotation;

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        if (Vector3.Distance(transform.position, dockingTransform.position) > 1f)
        {
            while (transform.position != midPosition && transform.rotation != endRotation)
            {
                timeSinceStarted = Time.time - startTime;
                percentageComplete = timeSinceStarted / time;
                transform.position = Vector3.Lerp(startPosition, midPosition, percentageComplete);
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, percentageComplete);
                yield return null;
            }
        }

        startPosition = transform.position;
        timeSinceStarted = 0f;
        percentageComplete = 0f;
        startTime = Time.time;

        while(percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);
            yield return null;
        }
    }

    IEnumerator PerformUndock(float time)
    {
        Debug.Log("Player undocking coroutine");
        Vector3 startPosition = transform.position;
        Vector3 midPosition = startPosition + new Vector3(0f, 10f, 0f);
        Vector3 endPosition = midPosition + (transform.forward * 100f) + (transform.up * 5f);
        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (transform.position != midPosition)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            transform.position = Vector3.Lerp(startPosition, midPosition, percentageComplete);
            yield return null;
        }

        startPosition = transform.position;
        timeSinceStarted = 0f;
        percentageComplete = 0f;
        startTime = Time.time;

        while (transform.position != endPosition)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);
            yield return null;
        }
        currentState = State.Default;
        LockMovement(false);
        GameManager.instance.isShootingEnabled = true;
        GameManager.instance.isCursorVisible = true;
    }
    #endregion
}
