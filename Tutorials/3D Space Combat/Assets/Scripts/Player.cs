using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Linq;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float idleSpeed = 100.0f;
    [SerializeField]
    private float moveSpeed = 200.0f;
    [SerializeField]
    private float strafeSpeed = 100.0f;
    [SerializeField]
    private float strafeRotation = 5f;
    [SerializeField]
    private float lookSpeed = 0.1f;
    [SerializeField]
    private int verticalLookLimit = 60;
    [SerializeField]
    private float tilt = 5.0f;
    [SerializeField]
    private float lookDamping = 0.05f;
    [SerializeField]
    private float rotationSpeed = 20f;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private Transform cameraTarget;
    [SerializeField]
    private AudioMixerSnapshot thrusterOnAudio;
    [SerializeField]
    private AudioMixerSnapshot thrusterOffAudio;
    [SerializeField]
    private ThrusterGroup thrusters;
    [SerializeField]
    private WarpManager warpManager;

    private Rigidbody _rb;
    private WarpAudio _warpDrive;
    private Boosters _boosters;
    private float _rotationZ;
    private Vector3 _screenPosition;
    private bool _movementLocked = false;
    private bool _controlsLocked = false;
    private State _currentState = State.Default;
    private AudioSource _audioSource;

    public enum State
    {
        Default,
        WarpStandby,
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
        _warpDrive = GetComponent<WarpAudio>();
        _audioSource = GetComponent<AudioSource>();
        _boosters = GetComponent<Boosters>();
    }

    void Update()
    {
        if (!_controlsLocked && Input.GetKeyDown(KeyCode.Tab)) LockOntoWarpTarget();
        if (_currentState == State.WarpStandby) EngageWarp();

        var isInCombat = IsInCombat();
        if (GameManager.instance.IsInCombat != isInCombat) GameManager.instance.IsInCombat = isInCombat;
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
            warpManager.Destination = target.Position;
            StartCoroutine(RotateTowards(target.Position));
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
        LockMovement(false);
        _currentState = State.Default;
        cameraController.ExitWarp();
    }

    private bool IsInCombat()
    {
        var targetableObjects = (GameObject.FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[]);
        var enemies = targetableObjects.Where(t => t.Allegiance == Enums.Allegiance.Enemy);
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

        var power = Mathf.Log10(forwardForce.sqrMagnitude / 83.965f) / Mathf.Log10(275855);
        if (_currentState != State.Warping) thrusters.SetPower(power);
    }

    private Vector3 Move(Vector3 forwardForce)
    {
        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");
        Vector3 mousePosition = Input.mousePosition;

        // Add base force
        forwardForce += transform.forward * idleSpeed;

        forwardForce = AddForwardForce(forwardForce, inputVertical);
        _boosters.RecalculateBoost();
        forwardForce += transform.forward * _boosters.BoostSpeed;

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
        }
        else
        {
            //thrusterOnAudio.TransitionTo(0.5f);
            forwardForce += transform.forward * moveSpeed * inputVertical;
        }
        return forwardForce;
    }

    private void Rotate(float inputHorizontal)
    {
        _screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1900));
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

    public IEnumerator LockMovementDelayed(bool isLocked, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        LockMovement(isLocked);
    }

    // TODO create class for all controls to pass through and perform check on _controlsLocked
    public void LockControls(bool isLocked)
    {
        _controlsLocked = isLocked;
    }

    public IEnumerator LockControlsDelayed(bool isLocked, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        LockControls(isLocked);
    }

    IEnumerator RotateTowards(Vector3 lookAtPosition)
    {
        Quaternion direction = Quaternion.LookRotation(lookAtPosition);
        while (Quaternion.Angle(_rb.rotation, direction) > 0.1)
        {
            _rb.rotation = Quaternion.Slerp(_rb.rotation, direction, lookSpeed);
            yield return null;
        }
    }
}
