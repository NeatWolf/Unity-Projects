using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Linq;

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
    public WarpManager warpManager;

    private Rigidbody _rb;
    private WarpDrive _warpDrive;
    private float _rotationZ;
    private Vector3 _screenPosition;
    private bool _movementLocked = false;
    private bool _controlsLocked = false;
    private State _currentState = State.Default;
    private AudioSource _audioSource;

    private float startingBoostSpeed;
    private float startingCombatBoostSpeed;

    public enum State
    {
        Default,
        WarpStandby,
        Docked,
        Boosting,
        Warping
    }

    public bool IsMovementLocked
    {
        get
        {
            return _movementLocked;
        }
    }

    public State CurrentState { get { return _currentState; } }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _warpDrive = GetComponent<WarpDrive>();
        _audioSource = GetComponent<AudioSource>();
        startingBoostSpeed = boostSpeed;
        startingCombatBoostSpeed = combatBoostSpeed;
        boostSpeed = 0f;
        combatBoostSpeed = 0f;
    }

    void Update()
    {
        if (_currentState == State.Docked)
        {
            if (_controlsLocked || Input.GetKeyDown(KeyCode.Space)) return;

            GameManager.instance.UndockPlayer();
        }

        if (!_controlsLocked && Input.GetKeyDown(KeyCode.Tab)) LockOntoWarpTarget();
        if (_currentState == State.WarpStandby) EngageWarp();

        GameManager.instance.isInCombat = IsInCombat();
    }

    private void LockOntoWarpTarget()
    {
        if (_currentState == State.WarpStandby) CancelWarpLock();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            WarpTarget target = hit.collider.gameObject.GetComponent<WarpTarget>();
            if (target == null) return;

            LockMovement(true);
            _warpDrive.SetTarget(target.targetTransform.position);
            warpManager.Destination = target.targetTransform.position;
            StartCoroutine(RotateTowards(target.targetTransform.position));
            _currentState = State.WarpStandby;
        }
    }

    private void CancelWarpLock()
    {
        LockMovement(false);
        _currentState = State.Default;
    }

    private void EngageWarp()
    {
        if (!_controlsLocked && Input.GetKeyDown(KeyCode.Space))
        {
            _currentState = State.Warping;
            cameraController.EnterWarp();
            warpManager.SetOnCompleteHandler(t => ExitWarp());
            warpManager.Warp();
        }
    }

    private void ExitWarp()
    {
        _warpDrive.PowerDown();
        LockMovement(false);
        _currentState = State.Default;
        cameraController.ExitWarp();
    }

    private bool IsInCombat()
    {
        var targetableObjects = (GameObject.FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[]);
        var enemies = targetableObjects.Where(t => t.allegiance == TargetableObject.Allegiance.Enemy);
        var closeEnemies = enemies.Where(t => Vector3.Distance(t.transform.position, transform.position) < 500).Count();

        return closeEnemies > 0;
    }

    void FixedUpdate()
    {
        Vector3 forwardForce = Vector3.zero;

        if (!_movementLocked)
        {
            forwardForce = Move(forwardForce);
            _audioSource.volume = Mathf.Clamp(Vector3.SqrMagnitude(forwardForce) / 882000f, 0f, 0.15f);
        }

        foreach (var thruster in thrusters)
        {
            // For moving speed we want lifetime at 0.5, for idle speed we want lifetime at 0.4
            thruster.startLifetime = Mathf.Clamp(Mathf.Log10(forwardForce.sqrMagnitude / 83.965f) / Mathf.Log10(275855), 0.1f, 0.7f);
        }
    }

    private Vector3 Move(Vector3 forwardForce)
    {
        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");
        Vector3 mousePosition = Input.mousePosition;

        // Add base force
        forwardForce += transform.forward * idleSpeed;

        forwardForce = AddForwardForce(forwardForce, inputVertical);

        forwardForce += transform.forward * (GameManager.instance.isInCombat ? combatBoostSpeed : boostSpeed);

        Rotate(inputHorizontal);
        _rb.AddForce(forwardForce);
        _rb.AddForce(transform.right * strafeSpeed * inputHorizontal);
        return forwardForce;
    }

    private Vector3 AddForwardForce(Vector3 forwardForce, float inputVertical)
    {
        if (inputVertical <= 0)
        {
            //thrusterOffAudio.TransitionTo(1f);
            forwardForce += transform.forward * idleSpeed * inputVertical;

            if (_currentState == State.Boosting)
            {
                StartCoroutine(LerpBoost(1f, GameManager.instance.isInCombat ? startingCombatBoostSpeed : startingBoostSpeed, 0f, GameManager.instance.isInCombat));
                cameraController.ExitBoost();

                _currentState = State.Default;
                GameManager.instance.isShootingEnabled = true;
            }
            return forwardForce;
        }

        //thrusterOnAudio.TransitionTo(0.5f);
        forwardForce += transform.forward * moveSpeed * inputVertical;

        Boost();

        return forwardForce;
    }

    private void Boost()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_currentState != State.Boosting)
            {
                StartCoroutine(LerpBoost(1f, 0f, GameManager.instance.isInCombat ? startingCombatBoostSpeed : startingBoostSpeed, GameManager.instance.isInCombat));
                cameraController.EnterBoost();
            }
            _currentState = State.Boosting;
            GameManager.instance.isShootingEnabled = false;
        }
        else if (_currentState == State.Boosting)
        {
            StartCoroutine(LerpBoost(1f, GameManager.instance.isInCombat ? startingCombatBoostSpeed : startingBoostSpeed, 0f, GameManager.instance.isInCombat));
            cameraController.ExitBoost();

            _currentState = State.Default;
            GameManager.instance.isShootingEnabled = true;
        }
    }

    private void Rotate(float inputHorizontal)
    {
        _screenPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1900));
        Vector3 direction = _screenPosition - transform.position;
        _rotationZ = Screen.width * 0.5f;

        _rb.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, transform.up), lookDamping);
        
        // Rotate player around z-axis when turning with the mouse. Compensate the camera by rotating the camera target in the opposite direction
        _rb.transform.rotation = Quaternion.Euler(_rb.transform.rotation.eulerAngles.x, _rb.transform.rotation.eulerAngles.y, (-inputHorizontal * strafeRotation) + (((Screen.width * 0.5f) - Input.mousePosition.x) / rotationSpeed));
        cameraTarget.localRotation = Quaternion.Euler(0f, 0f, (inputHorizontal * strafeRotation) - ((Screen.width * 0.5f) - Input.mousePosition.x) / (rotationSpeed));
    }

    public void LockMovement(bool isLocked)
    {
        _movementLocked = isLocked;
    }

    public void LockControls(bool isLocked)
    {
        _controlsLocked = isLocked;
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
        Quaternion direction = Quaternion.LookRotation(lookAtPosition);
        while (Quaternion.Angle(_rb.rotation, direction) > 0.1)
        {
            _rb.rotation = Quaternion.Slerp(_rb.rotation, direction, lookSpeed);
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
        _currentState = State.Docked;
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
        _currentState = State.Default;
        LockMovement(false);
        GameManager.instance.isShootingEnabled = true;
        GameManager.instance.isCursorVisible = true;
    }
    #endregion
}
