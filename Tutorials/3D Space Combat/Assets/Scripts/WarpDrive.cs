using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WarpDrive : MonoBehaviour {

    public ParticleSystem warpParticleSystem;
    public BarTimingMiniGame miniGame;
    public Text countdownText;
    public Timer timer;
    public int chargeUpTime = 10;

    private Vector3 targetPosition = Vector3.zero;
    private Enums.WARP_DRIVE_STATE state;
    private bool countingDown = false;
    private bool miniGameStarted = false;

    void Start()
    {
        state = Enums.WARP_DRIVE_STATE.off;
        timer = Instantiate(timer);
        countdownText.text = "";
        miniGame.OnResultReady += ProcessResult;
    }

    void Update()
    {
        if (countingDown)
        {
            if (timer.currentTime > 0)
            {
                countdownText.text = string.Format("FTL in {0} . . .", ((int)timer.currentTime + 1).ToString());
                if (!miniGameStarted)
                {
                    miniGame.StartMiniGame();
                    miniGameStarted = true;
                }
            }
            else
            {
                countdownText.text = "";
                if (TargetPosition != Vector3.zero)
                {
                    state = Enums.WARP_DRIVE_STATE.on;

                    // Do warp
                    transform.position = TargetPosition;
                    TargetPosition = Vector3.zero;
                    //

                    state = Enums.WARP_DRIVE_STATE.waitingForCommand;
                    countingDown = false;
                }
            }
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }
        set
        {
            targetPosition = value;
        }
    }

    public Enums.WARP_DRIVE_STATE State
    {
        get
        {
            return state;
        }
    }

    public void PowerDown()
    {
        state = Enums.WARP_DRIVE_STATE.off;
    }

    public void Engage()
    {
        if (!countingDown)
        {
            state = Enums.WARP_DRIVE_STATE.charging;
            Countdown(chargeUpTime);
            countingDown = true;
            miniGameStarted = false;
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
}
