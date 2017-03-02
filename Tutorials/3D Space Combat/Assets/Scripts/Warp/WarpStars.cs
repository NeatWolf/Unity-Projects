using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WarpStars : MonoBehaviour {

    private float _maxSpeed;
    private ParticleSystem _stars;
    private GoTween _beginTween;

	void Start ()
    {
        _stars = GetComponent<ParticleSystem>();
        var starsMain = _stars.main;
        _maxSpeed = starsMain.startSpeed.constant;
        starsMain.startSpeed = new ParticleSystem.MinMaxCurve(0f);
    }

    public void Begin(float duration, GoEaseType easeType = GoEaseType.Linear)
    {
        var starsMain = _stars.main;
        starsMain.startSpeed = new ParticleSystem.MinMaxCurve(0f);
        _stars.Play();
        _beginTween = Go.to(_stars, duration, new GoTweenConfig().floatProp("startSpeed", _maxSpeed).setEaseType(easeType));
    }

    public void End(float duration, GoEaseType easeType = GoEaseType.Linear)
    {
        if (_beginTween.state == GoTweenState.Running)
        {
            _beginTween.pause();
            _beginTween.destroy();
        }

        Go.to(_stars, duration, new GoTweenConfig().floatProp("startSpeed", 100f).setEaseType(easeType).onComplete(t => Stop()));
    }

    public void Stop()
    {
        _stars.Stop();
    }
}
