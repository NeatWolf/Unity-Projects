using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPlayer : MonoBehaviour {

    public float speed;
    public GoEaseType easeType;

    private Action<AbstractGoTween> _onComplete;

    public void Warp(Vector3 destination)
    {
        var duration = CalculateDuration(destination, speed);
        Go.to(transform, duration, new GoTweenConfig().vector3Prop("position", destination).setEaseType(easeType).onComplete(_onComplete));
    }

    public void SetOnCompleteHandler(Action<AbstractGoTween> handler)
    {
        _onComplete = handler;
    }

    private float CalculateDuration(Vector3 destination, float speed)
    {
        return Mathf.Clamp(Vector3.Distance(transform.position, destination) / (speed * 100f), 2f, 5f);
    }
}
