using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class PlanetAtmosphere : MonoBehaviour
{
    [SerializeField]
    private Text warningText;
    [SerializeField]
    private Text countdownText;
    [SerializeField]
    private GameObject timerPrefab;
    [SerializeField]
    private int destroyTime = 5;

    private Timer _timer;

    private bool _countingDown = false;
    private bool _justFinished = false;

    void Start()
    {
        _timer = Instantiate(timerPrefab).GetComponent<Timer>();
        warningText.text = "";
        countdownText.text = "";
    }

    void Update()
    {
        if (_countingDown)
        {
            warningText.text = string.Format("Entering Atmosphere. Destruction Imminent.");
            if (_timer.CurrentTime > 0)
            {
                countdownText.text = string.Format("{0}", ((int)_timer.CurrentTime + 1).ToString());
            }
            else
            {
                _countingDown = false;
                DamageInfo damageInfo = new DamageInfo(gameObject, int.MaxValue);
                HealthController health = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
                if (health != null)
                {
                    health.Damage(damageInfo);
                }
            }
        }
        else if (_justFinished)
        {
            _justFinished = false;
            warningText.text = "";
            countdownText.text = "";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entering atmosphere. Destruction imminent");
            Countdown(destroyTime);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Leaving atmosphere");
            CancelCountdown();
        }
    }

    private void Countdown(int startTime)
    {
        _timer.StartTime = startTime;
        _timer.ResetTimer();
        _timer.StartTimer();
        _countingDown = true;
    }

    private void CancelCountdown()
    {
        _countingDown = false;
        _justFinished = true;
    }
}
