using MorePPEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEffects : MonoBehaviour {

    public WarpRadialBlur radialBlur;
    public WarpStarsGroup warpStars;
    public WarpFov warpFov;
    public float radialBlurStrength = 1;
    public float duration = 1;
    public GoEaseType easeType = GoEaseType.Linear;
    public float fOVZoomInValue = 50;

	void Start ()
    {
        Invoke("Warp", 2f);
    }

    public void EnterWarp()
    {
        EnterRadialBlur();
        ShrinkFieldOfView();
        BeginStars();
    }

    public void ExitWarp()
    {
        EndStars();
        GrowFieldOfView();
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

    private void EnterRadialBlur()
    {
        radialBlur.Begin(duration, radialBlurStrength, easeType);
    }

    private void EndRadialBlur()
    {
        radialBlur.End(duration, easeType);
    }

    private void BeginStars()
    {
        warpStars.Begin(duration, easeType);
    }

    private void EndStars()
    {
        warpStars.End(duration, easeType);
    }
}
