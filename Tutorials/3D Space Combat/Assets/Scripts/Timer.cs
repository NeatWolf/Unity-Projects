using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

    public float startTime;

    [HideInInspector]
    public float currentTime;

    private bool started = false;

    void Start()
    {
        currentTime = startTime;
    }
	
	void Update()
    {
        if (started)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                currentTime = 0;
            }
        }
	}

    public void StartTimer()
    {
        started = true;
    }

    public void StopTimer()
    {
        started = false;
        currentTime = startTime;
    }

    public void PauseTimer()
    {
        started = false;
    }

    public void ResetTimer()
    {
        currentTime = startTime;
    }
}
