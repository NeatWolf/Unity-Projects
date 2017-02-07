using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour {

    public WarpEffects effects;
    public WarpPlayer player;
    public WarpCamera warpCamera;
    public ThrusterGroup thrusters;

    public Transform cameraTarget;

    private Action<AbstractGoTween> _onComplete;

    public Vector3 Destination { get; set; }

    public void Warp()
    {
        Warp(Destination);
    }

    public void Warp(Vector3 destination)
    {
        effects.EnterWarp();
        thrusters.SetMaxPower();
        player.SetOnCompleteHandler(delegate(AbstractGoTween t)
        {
            effects.ExitWarp();
            _onComplete.Invoke(t);
        });
        player.Warp(destination);

        var camOffset = CalculatePositionOffset(cameraTarget.position, player.transform.position);
        warpCamera.Warp(destination + camOffset);
    }

    public void SetOnCompleteHandler(Action<AbstractGoTween> handler)
    {
        _onComplete = handler;
    }

    private Vector3 CalculatePositionOffset(Vector3 offsetFromTarget, Vector3 target)
    {
        var x = offsetFromTarget.x - target.x;
        var y = offsetFromTarget.y - target.y;
        var z = offsetFromTarget.z - target.z;
        return new Vector3(x, y, z);
    }
}
