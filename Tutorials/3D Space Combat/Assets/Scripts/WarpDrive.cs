using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WarpDrive : MonoBehaviour {

    public ParticleSystem warpParticleSystem;
    public BarTimingMiniGame miniGame;
    public Text countdownText;
    public Timer timer;
    public int chargeUpTime = 10;
    public float warpSpeed;

    private Vector3 _targetPosition = Vector3.zero;
    private float _distanceToTarget = 0f;
    private Enums.WARP_DRIVE_STATE _state;
    private bool _countingDown = false;
    private bool _miniGameStarted = false;
    private float _currentSpeed;

    void Start()
    {
        _state = Enums.WARP_DRIVE_STATE.off;
        timer = Instantiate(timer);
        countdownText.text = "";
        miniGame.ResultReady += ProcessResult;
        _currentSpeed = warpSpeed;
    }

    void Update()
    {
        if (_countingDown)
        {
            if (timer.currentTime > 0)
            {
                countdownText.text = string.Format("FTL in {0} . . .", ((int)timer.currentTime + 1).ToString());
                if (!_miniGameStarted)
                {
                    miniGame.StartMiniGame();
                    _miniGameStarted = true;
                }
            }
            else
            {
                countdownText.text = "";
                if (_targetPosition != Vector3.zero)
                {
                    _state = Enums.WARP_DRIVE_STATE.on;

                    if(_distanceToTarget > 0f)
                    {
                        warpParticleSystem.Play();
                        Accelerate();
                        _distanceToTarget -= _currentSpeed;
                    }
                    else
                    {
                        warpParticleSystem.Stop();
                        _currentSpeed = warpSpeed;
                        _state = Enums.WARP_DRIVE_STATE.waitingForCommand;
                        _countingDown = false;
                    }
                }
            }
        }
    }

    public Enums.WARP_DRIVE_STATE State
    {
        get
        {
            return _state;
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        print(string.Format("Distance to target: {0}", _distanceToTarget));
    }

    public void PowerDown()
    {
        _state = Enums.WARP_DRIVE_STATE.off;
    }

    public void Engage()
    {
        if (!_countingDown)
        {
            _state = Enums.WARP_DRIVE_STATE.charging;
            Countdown(chargeUpTime);
            _countingDown = true;
            _miniGameStarted = false;
        }
    }

    private void Countdown(int startTime)
    {
        timer.startTime = startTime;
        timer.ResetTimer();
        timer.StartTimer();
    }

    private void ProcessResult(Enums.MINI_GAME_RESULT result)
    {
        switch (result)
        {
            case Enums.MINI_GAME_RESULT.perfect:
                timer.currentTime = 0.01f;
                break;
            case Enums.MINI_GAME_RESULT.good:
                timer.currentTime = 3f;
                break;
            default:
                break;
        }
    }

    private float CalculateDistanceToTarget(Vector3 target)
    {
        return Vector3.Distance(transform.position, target);
    }

    private void Accelerate()
    {
        _currentSpeed += _currentSpeed * Time.deltaTime;
        transform.position += transform.forward * _currentSpeed;
        print(string.Format("Move forward {0}", _currentSpeed));
    }
}
