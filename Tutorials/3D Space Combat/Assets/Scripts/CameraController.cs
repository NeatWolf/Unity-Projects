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
    public float distance = 50;
    public float height = 5;
    public float damping = 5;
    public float rotationDamping = 20;

    private bool _isWarping = false;
    private Animator _anim;
    private TiltShift _tiltShift;
    private float _startingDamping;
    private Vector3 _wantedPosition;
    private Quaternion _wantedRotation;
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
        _wantedPosition = target.TransformPoint(0, height, -distance);
        transform.localPosition = Vector3.Lerp(transform.position, _wantedPosition, damping * Time.deltaTime);
        //wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
        if (!_player.IsMovementLocked)
        {
            Vector3 screenPosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1900));
            Vector3 direction = screenPosition - transform.position;
            _wantedRotation = Quaternion.LookRotation(direction, target.up);
        }
        else
        {
            _wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
        }
        transform.localRotation = Quaternion.Slerp(transform.rotation, _wantedRotation, rotationDamping * Time.deltaTime);
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

    //private void UnlockController()
    //{
    //    transform.position = cam.transform.localPosition;
    //    transform.rotation = cam.transform.localRotation;
    //    cam.transform.localPosition = Vector3.zero;
    //    cam.transform.localRotation = Quaternion.identity;
    //    isLocked = false;
    //}

    public void ShakeCamera(float duration, float speed, float magnitude)
    {
        _cameraShake.duration = duration;
        _cameraShake.speed = speed;
        _cameraShake.magnitude = magnitude;
        _cameraShake.PlayShake();
    }

    #region Warp
    public void EnterWarp(float effectsTime, float speedLinesSpeed)
    {
        _isWarping = true;
        //ShakeCamera(2f, 40f, 200f);
        StartCoroutine(PerformEnterWarp(effectsTime, speedLinesSpeed));
        damping = 500f;
    }

    public void ExitWarp(float time)
    {
        _isWarping = false;
        damping = _startingDamping;
        StartCoroutine(PerformExitWarp(time));
    }

    IEnumerator PerformEnterWarp(float time, float speedLinesSpeed)
    {
        // Field of view
        Camera cameraGO = cam.GetComponent<Camera>();
        float startFoV = cameraGO.fieldOfView;
        float endFoV = cameraGO.fieldOfView + fieldOfViewChange;

        // Speed lines
        float startLinesSpeed = 0f;
        float endLinesSpeed = speedLinesSpeed;
        warpSpeedLines.Play();

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            _tiltShift.maxBlurSize = Mathf.Lerp(0f, 5f, percentageComplete);
            cameraGO.fieldOfView = Mathf.Lerp(startFoV, endFoV, percentageComplete);
            warpSpeedLines.startSpeed = Mathf.Lerp(startLinesSpeed, endLinesSpeed, percentageComplete);
            yield return null;
        }
    }

    IEnumerator PerformExitWarp(float time)
    {
        // Field of view
        Camera cameraGO = cam.GetComponent<Camera>();
        float startFoV = cameraGO.fieldOfView;
        float endFoV = cameraGO.fieldOfView - fieldOfViewChange;

        // Speed lines
        float startLinesSpeed = warpSpeedLines.startSpeed;
        float endLinesSpeed = 0f;

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            _tiltShift.maxBlurSize = Mathf.Lerp(5f, 0f, percentageComplete);
            cameraGO.fieldOfView = Mathf.Lerp(startFoV, endFoV, percentageComplete);
            warpSpeedLines.startSpeed = Mathf.Lerp(startLinesSpeed, endLinesSpeed, percentageComplete);
            yield return null;
        }
        warpSpeedLines.Stop();
    }
    #endregion

    #region Boost
    public void EnterBoost()
    {
        ShakeCamera(2f, 40f, 0.1f);
        if (GameManager.instance.isInCombat)
        {
            //damping *= 1.85f;
            StartCoroutine(PerformFollowSpeedUp(1f, 1.85f));
            boostSpeedLines.startSpeed = 250f;
            //var emission = boostSpeedLines.emission;
            //var rate = new ParticleSystem.MinMaxCurve();
            //rate.constantMax = 25f;
            //emission.rate = rate;
            boostSpeedLines.Play();
        }
        else
        {
            //damping *= 3f;
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
