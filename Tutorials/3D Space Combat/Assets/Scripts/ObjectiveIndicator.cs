using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectiveIndicator : MonoBehaviour {

    public Image iconImage;
    public Text distanceText;

    public void SetDistance(float distance)
    {
        if (distance >= 1000)
        {
            float distanceAmount = Round(distance / 1000f, 1);
            distanceText.text = string.Format("{0} K", distanceAmount.ToString());
        }
        else
        {
            float distanceAmount = Mathf.Round(distance);
            distanceText.text = string.Format("{0} M", distanceAmount.ToString());
        }
    }

    private float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
