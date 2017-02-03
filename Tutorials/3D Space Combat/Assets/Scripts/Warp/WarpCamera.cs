using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpCamera : MonoBehaviour {

    public float speed;
    public GoEaseType easeType;

    private Action<AbstractGoTween> _onComplete;

    public void Warp(Vector3 destination)
    {
        var duration = CalculateDuration(destination, speed);
        var tween = Go.to(transform, duration, new GoTweenConfig().vector3Prop("position", destination).setEaseType(easeType).onComplete(_onComplete));
    }

    public void SetOnCompleteHandler(Action<AbstractGoTween> handler)
    {
        _onComplete = handler;
    }

    private float CalculateDuration(Vector3 destination, float speed)
    {
        return Vector3.Distance(transform.position, destination) / (speed * 100f);
    }
}
