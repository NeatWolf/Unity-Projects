using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPlayer : MonoBehaviour {

    public float speed;
    public GoEaseType easeType;

    private Action<AbstractGoTween> _onComplete;
    private Action<AbstractGoTween> _onUpdate;

    public void Warp(Vector3 destination)
    {
        GameManager.instance.CameraController.ShakeCamera(0.8f, 10, 0.15f);
        var duration = CalculateDuration(destination, speed);
        var tween = Go.to(transform, duration, new GoTweenConfig().vector3Prop("position", destination).setEaseType(easeType).onComplete(_onComplete));
        tween.setOnUpdateHandler(_onUpdate);
    }

    public void SetOnCompleteHandler(Action<AbstractGoTween> handler)
    {
        _onComplete = handler;
    }

    public void SetOnUpdateHandler(Action<AbstractGoTween> handler)
    {
        _onUpdate = handler;
    }

    private float CalculateDuration(Vector3 destination, float speed)
    {
        return Mathf.Clamp(Vector3.Distance(transform.position, destination) / (speed * 100f), 2f, 5f);
    }
}
