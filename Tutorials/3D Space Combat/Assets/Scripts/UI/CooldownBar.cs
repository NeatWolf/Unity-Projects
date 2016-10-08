using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour {

    public Image filledImage;
    public float waitTime = 5f;

    private bool _coolingDown = false;

    public bool CoolingDown
    {
        get
        {
            return _coolingDown;
        }
    }

    void Update ()
    {
        if (_coolingDown && filledImage.fillAmount > 0f)
        {
            filledImage.fillAmount -= Time.deltaTime / waitTime;
        }
        else
        {
            _coolingDown = false;
        }
	}

    public void Cooldown()
    {
        filledImage.fillAmount = 1f;
        _coolingDown = true;
    }

    public void Cooldown(float time)
    {
        waitTime = time;
        filledImage.fillAmount = 1f;
        _coolingDown = true;
    }
}
