using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour {

    public WarpEffects effects;
    public WarpPlayer player;

    private Action<AbstractGoTween> _onComplete;

    public Vector3 Destination { get; set; }

    public void Warp()
    {
        Warp(Destination);
    }

    public void Warp(Vector3 destination)
    {
        effects.EnterWarp();
        player.SetOnCompleteHandler(delegate(AbstractGoTween t)
        {
            effects.ExitWarp();
            _onComplete.Invoke(t);
        });
        player.Warp(destination);
    }

    public void SetOnCompleteHandler(Action<AbstractGoTween> handler)
    {
        _onComplete = handler;
    }
}
