using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WarpDrive : MonoBehaviour {

    public CameraController cameraController;
    public BarTimingMiniGame miniGame;
    public Text countdownText;
    public Timer timer;
    public int chargeUpTime = 10;
    public float warpSpeed;
    public AudioClip warpChargingSound;
    public AudioClip warpBoomSound;
    public AudioClip warpStartingSound;
    public AudioSource audioSource;

    private Vector3 _targetPosition = Vector3.zero;
    private Enums.WarpDriveState _state;
    private bool _countingDown = false;
    private bool _miniGameStarted = false;
    private bool _chargingSoundPlayed = false;
    private bool _boomSoundPlayed = false;
    private bool _goSoundPlayed = false;
    private bool _startingSoundPlayed = false;
    private AudioSource playAudioSource;

    void Start()
    {
        _state = Enums.WarpDriveState.off;
        timer = Instantiate(timer);
        countdownText.text = "";
        miniGame.ResultReady += ProcessResult;
        playAudioSource = audioSource.gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (_countingDown)
        {
            if (timer.currentTime > 0)
            {
                if (!_startingSoundPlayed)
                {
                    playAudioSource.clip = warpStartingSound;
                    playAudioSource.Play();
                    _startingSoundPlayed = true;
                }
                if (!_chargingSoundPlayed && timer.currentTime < 6.4f && timer.currentTime > 6.2f)
                {
                    if (playAudioSource.isPlaying)
                    {
                        StartCoroutine(FadeOut(playAudioSource, 4f));
                    }
                    audioSource.PlayOneShot(warpChargingSound, 1f);
                    _chargingSoundPlayed = true;
                }
                if (!_boomSoundPlayed && timer.currentTime < 2.5f && timer.currentTime > 2.3f)
                {
                    if (playAudioSource.isPlaying)
                    {
                        StartCoroutine(FadeOut(playAudioSource, 3f));
                    }
                    audioSource.PlayOneShot(warpBoomSound, 1f);
                    _boomSoundPlayed = true;
                    _goSoundPlayed = true;
                }
                countdownText.text = string.Format("FTL in {0} . . .", ((int)timer.currentTime + 1).ToString());
                if (!_miniGameStarted)
                {
                    miniGame.StartMiniGame();
                    _miniGameStarted = true;
                }
            }
            else
            {
                if (!_goSoundPlayed)
                {
                    if (playAudioSource.isPlaying)
                    {
                        StartCoroutine(FadeOut(playAudioSource, 1.5f));
                    }
                    audioSource.clip = warpBoomSound;
                    audioSource.time = 2.55f;
                    audioSource.Play();
                }
                countdownText.text = "";
                miniGame.Close();

                // Perform warp
                StartCoroutine(PerformWarpMove(5f, 0.5f, 0.15f, 200f));
                _countingDown = false;
                _chargingSoundPlayed = false;
                _boomSoundPlayed = false;
                _goSoundPlayed = false;
                _startingSoundPlayed = false;
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
        GameManager.instance.cameraController.ShakeCamera(2f, 40f, 0.4f);
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
        cameraController.ExitWarp(effectsEndTime);
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

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
