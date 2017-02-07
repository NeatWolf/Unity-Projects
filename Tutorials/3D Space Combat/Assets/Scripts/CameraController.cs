using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour
{
    public Transform cam;
    public ParticleSystem warpSpeedLines;
    public ParticleSystem boostSpeedLines;
    public float fieldOfViewChange;
    public Transform target;
    public float damping = 5;
    public float rotationDamping = 20;

    private bool _isWarping = false;
    private Animator _anim;
    private TiltShift _tiltShift;
    private float _startingDamping;
    private Vector3 _wantedPosition;
    private CameraShake _cameraShake;
    private Player _player;
    private Camera _camera;

    public bool IsWarping
    {
        get
        {
            return _isWarping;
        }
    }

    public float Damping
    {
        get { return damping; }
        set { damping = value; }
    }

    void Start()
    {
        _anim = GetComponent<Animator>();
        _tiltShift = cam.GetComponent<TiltShift>();
        _cameraShake = GetComponent<CameraShake>();
        _player = GameManager.playerTransform.GetComponent<Player>();
        _camera = cam.GetComponent<Camera>();
        _startingDamping = damping;
    }

    void LateUpdate()
    {
        if (_isWarping) return;

        transform.localPosition = Vector3.Lerp(transform.position, target.position, damping * Time.deltaTime);
        Quaternion desiredRotation;
        if (!_player.IsMovementLocked)
        {
            Vector3 screenPosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1900));
            Vector3 direction = screenPosition - transform.position;
            desiredRotation = Quaternion.LookRotation(direction, target.up);
        }
        else
        {
            desiredRotation = Quaternion.LookRotation(target.forward, target.up);
        }
        transform.localRotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationDamping * Time.deltaTime);
    }

    private void LockToPlayer()
    {
        transform.position = target.position;
    }

    public void PerformDock()
    {
        //isLocked = true;
    }

    public void PerformUndock()
    {
        //anim.SetTrigger("Undock");
        //Invoke("UnlockController", 6f);
    }

    public void ShakeCamera(float duration, float speed, float magnitude)
    {
        _cameraShake.duration = duration;
        _cameraShake.speed = speed;
        _cameraShake.magnitude = magnitude;
        _cameraShake.PlayShake();
    }

    #region Warp
    public void EnterWarp()
    {
        _isWarping = true;
    }

    public void ExitWarp()
    {
        _isWarping = false;
    }
    #endregion

    #region Boost
    public void EnterBoost()
    {
        ShakeCamera(2f, 40f, 0.1f);
        if (GameManager.instance.isInCombat)
        {
            StartCoroutine(PerformFollowSpeedUp(1f, 1.85f));
            boostSpeedLines.startSpeed = 250f;
            boostSpeedLines.Play();
        }
        else
        {
            StartCoroutine(PerformFollowSpeedUp(1f, 3f));
            boostSpeedLines.startSpeed = 400f;
            boostSpeedLines.Play();
        }
    }

    public void ExitBoost()
    {
        StartCoroutine(PerformFollowSpeedDown(1f, _startingDamping));
        damping = _startingDamping;
        boostSpeedLines.Stop();
    }

    IEnumerator PerformFollowSpeedUp(float time, float scale)
    {
        float startSpeed = damping;
        float endSpeed = damping * scale;

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            damping = Mathf.Lerp(startSpeed, endSpeed, percentageComplete);
            yield return null;
        }
    }

    IEnumerator PerformFollowSpeedDown(float time, float endValue)
    {
        float startSpeed = damping;
        float endSpeed = endValue;

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            damping = Mathf.Lerp(startSpeed, endSpeed, percentageComplete);
            yield return null;
        }
    }
    #endregion
}
