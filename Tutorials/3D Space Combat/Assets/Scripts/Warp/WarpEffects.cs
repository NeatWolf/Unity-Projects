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
    public float startDuration = 1f;
    public float endDuration = 1f;
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
        //EndStars(duration * 0.1f);
        GrowFieldOfView();
        EndVignette();
        EndRadialBlur();
    }

    private void ShrinkFieldOfView()
    {
        warpFov.Begin(startDuration, fOVZoomInValue, easeType);
    }

    private void GrowFieldOfView()
    {
        warpFov.End(endDuration, easeType);
    }

    private void BeginRadialBlur()
    {
        radialBlur.Begin(startDuration, radialBlurStrength, easeType);
    }

    private void EndRadialBlur()
    {
        radialBlur.End(endDuration, easeType);
    }

    private void BeginVignette()
    {
        vignette.Begin(startDuration, vignetteStrength, easeType);
    }

    private void EndVignette()
    {
        vignette.End(endDuration, easeType);
    }

    private void BeginStars()
    {
        warpStars.Begin(startDuration, easeType);
    }

    public void EndStars()
    {
        warpStars.End(endDuration * 0.1f, easeType);
        //warpStars.Stop();
    }
}
