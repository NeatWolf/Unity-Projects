using MorePPEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEffects : MonoBehaviour {

    public WarpRadialBlur radialBlur;
    public WarpVignette vignette;
    public WarpStarsGroup warpStars;
    public WarpFov warpFov;
    public float radialBlurStrength = 1f;
    public float vignetteStrength = 1.4f;
    public float duration = 1f;
    public GoEaseType easeType = GoEaseType.Linear;
    public float fOVZoomInValue = 50f;

	void Start ()
    {
        Invoke("Warp", 2f);
    }

    public void EnterWarp()
    {
        BeginRadialBlur();
        BeginVignette();
        ShrinkFieldOfView();
        BeginStars();
    }

    public void ExitWarp()
    {
        EndStars(duration * 0.1f);
        GrowFieldOfView();
        EndVignette();
        EndRadialBlur();
    }

    private void ShrinkFieldOfView()
    {
        warpFov.Begin(duration, fOVZoomInValue, easeType);
    }

    private void GrowFieldOfView()
    {
        warpFov.End(duration, easeType);
    }

    private void BeginRadialBlur()
    {
        radialBlur.Begin(duration, radialBlurStrength, easeType);
    }

    private void EndRadialBlur()
    {
        radialBlur.End(duration, easeType);
    }

    private void BeginVignette()
    {
        vignette.Begin(duration, vignetteStrength, easeType);
    }

    private void EndVignette()
    {
        vignette.End(duration, easeType);
    }

    private void BeginStars()
    {
        warpStars.Begin(duration, easeType);
    }

    private void EndStars(float duration)
    {
        warpStars.End(duration, easeType);
    }
}
