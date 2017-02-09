using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InverseVignette))]
public class WarpVignette : MonoBehaviour
{

    private InverseVignette _vignette;
    private GoTween _beginTween;

    void Start()
    {
        _vignette = GetComponent<InverseVignette>();
    }

    public void Begin(float duration, float strength, GoEaseType easeType = GoEaseType.Linear)
    {
        _beginTween = Go.to(_vignette, duration, new GoTweenConfig().floatProp("Intensity", strength).setEaseType(easeType));
    }

    public void End(float duration, GoEaseType easeType = GoEaseType.Linear)
    {
        if (_beginTween.state == GoTweenState.Running)
        {
            _beginTween.pause();
            _beginTween.destroy();
        }

        Go.to(_vignette, duration, new GoTweenConfig().floatProp("Intensity", 0f).setEaseType(easeType));
    }
}
