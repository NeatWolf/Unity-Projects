using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIProgressBarController : MonoBehaviour {

    public Image barFullImage;
    public Image barEmptyImage;

    public float fillAmount
    {
        get
        {
            return barFullImage.fillAmount;
        }
        set
        {
            barFullImage.fillAmount = value;
        }
    }

	void Start ()
    {

	}
	
	void Update ()
    {
	
	}
}
