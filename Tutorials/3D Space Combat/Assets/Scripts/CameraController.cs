using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour
{
    public float offset;
    public Transform player;
    public Transform cam;
    public ParticleSystem speedLines;
    public float moveSpeed;
    public float rotationSpeed;
    public float fieldOfViewChange;
    public float shakeIntensity = 0f;
    public bool isShaking = false;

    private bool isLocked = false;
    private bool isWarping = false;
    private Animator anim;
    private TiltShift tiltShift;

    public bool IsWarping
    {
        get
        {
            return isWarping;
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        tiltShift = cam.GetComponent<TiltShift>();
    }

    void LateUpdate()
    {
        if (player != null)
        {
            if (!isLocked)
            {
                // Set the position behind the player by a given offset
                Vector3 wantedPosition = player.position - player.forward * offset;

                // Remove z-axis rotation
                Quaternion rotationXY = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, 0f);

                // Set the position and rotation
                transform.position = Vector3.Lerp(transform.position, wantedPosition, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationXY, rotationSpeed * Time.deltaTime);
            }

            if (isShaking)
            {
                if (shakeIntensity > 0)
                {
                    transform.position = transform.position + Random.insideUnitSphere * shakeIntensity;
                }
            }
        }
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

    public void EnterWarp(float effectsTime, float speedLinesSpeed)
    {
        isShaking = true;
        isWarping = true;
        StartCoroutine(PerformEnterWarp(effectsTime, speedLinesSpeed));
        moveSpeed = 500f;
    }

    public void ExitWarp(float time)
    {
        isShaking = false;
        isWarping = false;
        moveSpeed = 6f;
        StartCoroutine(PerformExitWarp(time));
    }

    IEnumerator PerformFollowSpeedUp(float time, float scale)
    {
        float startSpeed = moveSpeed;
        float endSpeed = moveSpeed * scale;

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            moveSpeed = Mathf.Lerp(startSpeed, endSpeed, percentageComplete);
            yield return null;
        }
    }

    IEnumerator PerformFollowSpeedDown(float time, float scale)
    {
        float startSpeed = moveSpeed;
        float endSpeed = moveSpeed / scale;

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            moveSpeed = Mathf.Lerp(startSpeed, endSpeed, percentageComplete);
            yield return null;
        }
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
        speedLines.Play();

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            tiltShift.maxBlurSize = Mathf.Lerp(0f, 5f, percentageComplete);
            cameraGO.fieldOfView = Mathf.Lerp(startFoV, endFoV, percentageComplete);
            speedLines.startSpeed = Mathf.Lerp(startLinesSpeed, endLinesSpeed, percentageComplete);
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
        float startLinesSpeed = speedLines.startSpeed;
        float endLinesSpeed = 0f;

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            tiltShift.maxBlurSize = Mathf.Lerp(5f, 0f, percentageComplete);
            cameraGO.fieldOfView = Mathf.Lerp(startFoV, endFoV, percentageComplete);
            speedLines.startSpeed = Mathf.Lerp(startLinesSpeed, endLinesSpeed, percentageComplete);
            yield return null;
        }
        speedLines.Stop();
    }
}
