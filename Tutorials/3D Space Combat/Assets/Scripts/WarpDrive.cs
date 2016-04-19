using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WarpDrive : MonoBehaviour {

    public ParticleSystem warpParticleSystem;
    public CameraController cameraController;
    public RadialBlur radialBlur;
    public BarTimingMiniGame miniGame;
    public Text countdownText;
    public Timer timer;
    public int chargeUpTime = 10;
    public float warpSpeed;

    private Vector3 _targetPosition = Vector3.zero;
    private Enums.WarpDriveState _state;
    private bool _countingDown = false;
    private bool _miniGameStarted = false;

    void Start()
    {
        _state = Enums.WarpDriveState.off;
        timer = Instantiate(timer);
        countdownText.text = "";
        miniGame.ResultReady += ProcessResult;
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

                // Perform warp
                warpParticleSystem.Play();
                radialBlur.TurnOn();
                StartCoroutine(PerformWarpMove(5f, 0.5f, 0.25f, 200f));
                _countingDown = false;
            }
        }
    }

    public Enums.WarpDriveState State
    {
        get
        {
            return _state;
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        Debug.Log(string.Format("Target Position: {0}", _targetPosition));
    }

    public void PowerDown()
    {
        _state = Enums.WarpDriveState.off;
    }

    public void Engage()
    {
        if (!_countingDown)
        {
            _state = Enums.WarpDriveState.charging;
            Countdown(chargeUpTime);
            _countingDown = true;
            _miniGameStarted = false;
        }
    }

    IEnumerator PerformWarpMove(float time, float effectsStartTime, float effectsEndTime, float speedLinesSpeed)
    {
        cameraController.EnterWarp(effectsStartTime, speedLinesSpeed);
        _state = Enums.WarpDriveState.on;
        GameObject startPosition = new GameObject();
        startPosition.transform.position = transform.position;
        GameObject endPosition = new GameObject();
        endPosition.transform.position = _targetPosition;

        if (endPosition.transform.position == Vector3.zero)
        {
            yield break;
        }

        float timeSinceStarted = 0f;
        float percentageComplete = 0f;
        float startTime = Time.time;

        while (percentageComplete < 1f)
        {
            timeSinceStarted = Time.time - startTime;
            percentageComplete = timeSinceStarted / time;
            transform.position = Vector3.Lerp(startPosition.transform.position, endPosition.transform.position, percentageComplete);
            yield return null;
        }
        Destroy(startPosition);
        Destroy(endPosition);
        warpParticleSystem.Stop();
        cameraController.ExitWarp(effectsEndTime);
        //radialBlur.TurnOff();
        _state = Enums.WarpDriveState.waitingForCommand;
    }

    private void Countdown(int startTime)
    {
        timer.startTime = startTime;
        timer.ResetTimer();
        timer.StartTimer();
    }

    private void ProcessResult(Enums.MiniGameResult result)
    {
        switch (result)
        {
            case Enums.MiniGameResult.perfect:
                timer.currentTime = 0.01f;
                break;
            case Enums.MiniGameResult.good:
                timer.currentTime = 3f;
                break;
            default:
                break;
        }
    }
}
