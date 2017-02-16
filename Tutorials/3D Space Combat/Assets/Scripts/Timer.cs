using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

    [SerializeField]
    private float startTime;

    public float CurrentTime { get; set; }
    public float StartTime
    {
        get { return startTime; }
        set { startTime = value; }
    }

    private bool started = false;

    void Awake()
    {
        CurrentTime = startTime;
    }
	
	void Update()
    {
        if (started)
        {
            CurrentTime -= Time.deltaTime;

            if (CurrentTime < 0)
            {
                CurrentTime = 0;
                started = false;
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
        CurrentTime = startTime;
    }

    public void PauseTimer()
    {
        started = false;
    }

    public void ResetTimer()
    {
        CurrentTime = startTime;
    }
}
