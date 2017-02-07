using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WarpAudio : MonoBehaviour {

    public Timer timer;
    public int chargeUpTime = 10;
    public AudioClip warpChargingSound;
    public AudioClip warpBoomSound;
    public AudioClip warpStartingSound;
    public AudioSource audioSource;

    private bool _countingDown = false;
    private bool _miniGameStarted = false;
    private bool _chargingSoundPlayed = false;
    private bool _boomSoundPlayed = false;
    private bool _goSoundPlayed = false;
    private bool _startingSoundPlayed = false;
    private AudioSource _playAudioSource;

    void Start()
    {
        timer = Instantiate(timer);
        _playAudioSource = audioSource.gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (_countingDown)
        {
            if (timer.currentTime > 0)
            {
                if (!_startingSoundPlayed)
                {
                    _playAudioSource.clip = warpStartingSound;
                    _playAudioSource.Play();
                    _startingSoundPlayed = true;
                }
                if (!_chargingSoundPlayed && timer.currentTime < 6.4f && timer.currentTime > 6.2f)
                {
                    if (_playAudioSource.isPlaying)
                    {
                        StartCoroutine(FadeOut(_playAudioSource, 4f));
                    }
                    audioSource.PlayOneShot(warpChargingSound, 1f);
                    _chargingSoundPlayed = true;
                }
                if (!_boomSoundPlayed && timer.currentTime < 2.5f && timer.currentTime > 2.3f)
                {
                    if (_playAudioSource.isPlaying)
                    {
                        StartCoroutine(FadeOut(_playAudioSource, 3f));
                    }
                    audioSource.PlayOneShot(warpBoomSound, 1f);
                    _boomSoundPlayed = true;
                    _goSoundPlayed = true;
                }
                if (!_miniGameStarted)
                {
                    _miniGameStarted = true;
                }
            }
            else
            {
                if (!_goSoundPlayed)
                {
                    if (_playAudioSource.isPlaying)
                    {
                        StartCoroutine(FadeOut(_playAudioSource, 1.5f));
                    }
                    audioSource.clip = warpBoomSound;
                    audioSource.time = 2.55f;
                    audioSource.Play();
                }

                _countingDown = false;
                _chargingSoundPlayed = false;
                _boomSoundPlayed = false;
                _goSoundPlayed = false;
                _startingSoundPlayed = false;
            }
        }
    }

    public void Engage()
    {
        if (_countingDown) return;

        Countdown(chargeUpTime);
        _countingDown = true;
        _miniGameStarted = false;
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
