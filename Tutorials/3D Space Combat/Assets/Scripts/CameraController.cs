using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private ParticleSystem warpSpeedLines;
    [SerializeField]
    private ParticleSystem boostSpeedLines;
    [SerializeField]
    private float fieldOfViewChange;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float damping = 5;
    [SerializeField]
    private float rotationDamping = 20;

    private bool _isWarping = false;
    private float _startingDamping;
    private CameraShake _cameraShake;
    private Player _player;
    private Camera _camera;

    public float Damping
    {
        get { return damping; }
        set { damping = value; }
    }

    void Start()
    {
        _cameraShake = GetComponent<CameraShake>();
        _player = GameManager.instance.Player;
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

    public void ShakeCamera(float duration, float speed, float magnitude)
    {
        _cameraShake.Duration = duration;
        _cameraShake.Speed = speed;
        _cameraShake.Magnitude = magnitude;
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
        ScaleDamping(1f, 3f);
        boostSpeedLines.startSpeed = 400f;
        boostSpeedLines.Play();
    }

    public void EnterCombatBoost()
    {
        ShakeCamera(2f, 40f, 0.1f);
        ScaleDamping(1f, 1.85f);
        boostSpeedLines.startSpeed = 250f;
        boostSpeedLines.Play();
    }

    public void ExitBoost()
    {
        ScaleDamping(1f, 1f);
        boostSpeedLines.Stop();
    }

    private void ScaleDamping(float time, float scale)
    {
        Go.to(this, time, new GoTweenConfig().floatProp("Damping", _startingDamping * scale));
    }
    #endregion
}
