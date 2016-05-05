using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour
{
    public float offset;
    public Transform player;
    public Transform cam;
    public ParticleSystem warpSpeedLines;
    public ParticleSystem boostSpeedLines;
    public float moveSpeed;
    public float rotationSpeed;
    public float fieldOfViewChange;
    public float shakeIntensity = 0f;
    public bool isShaking = false;

    private bool isLocked = false;
    private bool isWarping = false;
    private Animator anim;
    private TiltShift tiltShift;
    private float startingMoveSpeed;
    private Vector3 velocity = Vector3.zero;

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
        startingMoveSpeed = moveSpeed;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            if (!isLocked)
            {
                // Set the position behind the player by a given offset
                Vector3 wantedPosition = player.transform.position - player.transform.forward * offset;

                // Remove z-axis rotation
                Quaternion rotationXY = Quaternion.Euler(player.transform.rotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, 0f);

                // Set the position and rotation
                transform.position = Vector3.Lerp(transform.position, wantedPosition, moveSpeed * Time.deltaTime);
                //transform.position = Vector3.SmoothDamp(transform.position, wantedPosition, ref velocity, moveSpeed);
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

    public void ShakeForSeconds(float time, float intensity)
    {
        StartCoroutine(PerformShakeForSeconds(time, intensity));
    }

    IEnumerator PerformShakeForSeconds(float time, float intensity)
    {
        if (intensity > 0)
        {
            float originalIntensity = shakeIntensity;
            shakeIntensity = intensity;
            isShaking = true;
            float timeSinceStarted = 0f;
            float percentageComplete = 0f;
            float startTime = Time.time;

            while (percentageComplete < 1f)
            {
                timeSinceStarted = Time.time - startTime;
                percentageComplete = timeSinceStarted / time;
                shakeIntensity = Mathf.Lerp(intensity, 0f, percentageComplete);
                yield return null;
            }
            isShaking = false;
            shakeIntensity = originalIntensity;
        }
        else
        {
            Debug.LogWarning("Shake intensity was set to zero or negative");
        }
    }

    #region Warp
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
        moveSpeed = startingMoveSpeed;
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
            tiltShift.maxBlurSize = Mathf.Lerp(0f, 5f, percentageComplete);
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
            tiltShift.maxBlurSize = Mathf.Lerp(5f, 0f, percentageComplete);
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
        isShaking = true;
        if (GameManager.instance.isInCombat)
        {
            moveSpeed *= 1.5f;
            boostSpeedLines.startSpeed = 150f;
            boostSpeedLines.Play();
        }
        else
        {
            moveSpeed *= 3.5f;
            boostSpeedLines.startSpeed = 300f;
            boostSpeedLines.Play();
        }
    }

    public void ExitBoost()
    {
        isShaking = false;
        moveSpeed = startingMoveSpeed;
        boostSpeedLines.Stop();
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
    #endregion
}
