using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class WarpFov : MonoBehaviour {

    private float _originalValue;
    private Camera _camera;
    private GoTween _beginTween;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _originalValue = _camera.fieldOfView;
    }

    public void Begin(float duration, float fOVZoomInValue, GoEaseType easeType = GoEaseType.Linear)
    {
        _beginTween = Go.to(_camera, duration, new GoTweenConfig().floatProp("fieldOfView", fOVZoomInValue).setEaseType(easeType));
    }

    public void End(float duration, GoEaseType easeType = GoEaseType.Linear)
    {
        if (_beginTween.state == GoTweenState.Running)
        {
            _beginTween.pause();
            _beginTween.destroy();
        }

        Go.to(_camera, duration, new GoTweenConfig().floatProp("fieldOfView", _originalValue).setEaseType(easeType));
    }
}
