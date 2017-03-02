using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpStarsGroup : MonoBehaviour {

    public List<WarpStars> warpStarSystems;

    public void Begin(float duration, GoEaseType easeType = GoEaseType.Linear)
    {
        foreach(var system in warpStarSystems)
        {
            system.Begin(duration, easeType);
        }
    }

    public void End(float duration, GoEaseType easeType = GoEaseType.Linear)
    {
        foreach (var system in warpStarSystems)
        {
            system.End(duration, easeType);
        }
    }

    public void Stop()
    {
        foreach(var system in warpStarSystems)
        {
            system.Stop();
        }
    }
}
