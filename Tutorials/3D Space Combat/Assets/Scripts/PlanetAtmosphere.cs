using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class PlanetAtmosphere : MonoBehaviour
{
    public Text warningText;
    public Text countdownText;
    public GameObject timerPrefab;
    public int destroyTime = 5;

    private Timer timer;

    private bool _countingDown = false;
    private bool _justFinished = false;

    void Start()
    {
        timer = Instantiate(timerPrefab).GetComponent<Timer>();
        warningText.text = "";
        countdownText.text = "";
    }

    void Update()
    {
        if (_countingDown)
        {
            warningText.text = string.Format("Entering Atmosphere. Destruction Imminent.");
            if (timer.currentTime > 0)
            {
                countdownText.text = string.Format("{0}", ((int)timer.currentTime + 1).ToString());
            }
            else
            {
                _countingDown = false;
                DamageInfo damageInfo = new DamageInfo(gameObject, int.MaxValue);
                GameObject.FindGameObjectWithTag("Player").transform.SendMessage("Damage", damageInfo);
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
        timer.startTime = startTime;
        timer.ResetTimer();
        timer.StartTimer();
        _countingDown = true;
    }

    private void CancelCountdown()
    {
        _countingDown = false;
        _justFinished = true;
    }
}
