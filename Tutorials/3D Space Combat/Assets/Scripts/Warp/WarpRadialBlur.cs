using MorePPEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RadialBlur))]
public class WarpRadialBlur : MonoBehaviour {

    private RadialBlur _radialBlur;
    private GoTween _beginTween;

    void Start()
    {
        _radialBlur = GetComponent<RadialBlur>();
    }

    public void Begin(float duration, float strength, GoEaseType easeType = GoEaseType.Linear)
    {
        _beginTween = Go.to(_radialBlur, duration, new GoTweenConfig().floatProp("BlurStrength", strength).setEaseType(easeType));
    }

    public void End(float duration, GoEaseType easeType = GoEaseType.Linear)
    {
        if (_beginTween.state == GoTweenState.Running)
        {
            _beginTween.pause();
            _beginTween.destroy();
        }

        Go.to(_radialBlur, duration, new GoTweenConfig().floatProp("BlurStrength", 0f).setEaseType(easeType));
    }
}
