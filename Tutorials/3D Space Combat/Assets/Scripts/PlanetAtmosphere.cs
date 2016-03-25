using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class PlanetAtmosphere : MonoBehaviour
{
    public Text countdownText;
    public Timer timer;
    public int destroyTime = 5;

    private bool _countingDown = false;

    void Start()
    {
        timer = Instantiate(timer);
        countdownText.text = "";
    }

    void Update()
    {
        if (_countingDown)
        {
            if(timer.currentTime > 0)
            {
                countdownText.text = string.Format("Entering atmosphere. You're hull will disintegrate in {0} . . .", ((int)timer.currentTime + 1).ToString());
            }
            else
            {
                _countingDown = false;
                DamageInfo damageInfo = new DamageInfo(gameObject, 100000);
                GameObject.FindGameObjectWithTag("Player").transform.SendMessage("Damage", damageInfo);
            }
        }
        else
        {
            countdownText.text = "";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Entering atmosphere. Destruction imminent");
            Countdown(destroyTime);
        }
    }

    void OnTriggerExit(Collider other)
    {
        print("Leaving atmosphere");
        CancelCountdown();
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
    }
}
