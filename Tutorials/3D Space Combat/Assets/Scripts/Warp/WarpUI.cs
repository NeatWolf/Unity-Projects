using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class WarpUI : MonoBehaviour
{
    public BarTimingMiniGame miniGame;
    public UICountdown countdown;

    public delegate void ReadyDelegate();
    public event ReadyDelegate ChargeComplete;

    public void Begin()
    {
        BeginCountdown();
        BeginMinigame();
    }

    private void BeginCountdown()
    {
        countdown.Finished += Countdown_Finished;
        countdown.Play();
    }

    private void BeginMinigame()
    {
        miniGame.ResultReady += MiniGame_ResultReady;
        miniGame.StartMiniGame();
    }

    private void OnChargeComplete()
    {
        if (ChargeComplete != null) ChargeComplete();
    }

    private void Countdown_Finished()
    {
        OnChargeComplete();
    }

    private void MiniGame_ResultReady(Enums.MiniGameResult result)
    {
        switch (result)
        {
            case Enums.MiniGameResult.perfect:
                countdown.TimeLeft = 0.01f;
                break;
            case Enums.MiniGameResult.good:
                countdown.TimeLeft = 3f;
                break;
            default:
                break;
        }
    }
}
