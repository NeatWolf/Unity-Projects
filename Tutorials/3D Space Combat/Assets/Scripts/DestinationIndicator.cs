using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DestinationIndicator : MonoBehaviour {

    public Image entryPoint;
    public Text destinationName;

    private RectTransform rt;

	void Start ()
    {
        rt = GetComponent<RectTransform>();
	}

    public void SetSize(float size)
    {
        rt.sizeDelta = new Vector2(size, size);
    }

    public void SetEntryPointPosition(Vector3 entryPosition)
    {
        Vector3 parentScreenPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(entryPosition);
        entryPoint.rectTransform.anchoredPosition = new Vector2(screenPosition.x - parentScreenPosition.x, screenPosition.y - parentScreenPosition.y);
    }

    public void SetDestinationName(string destName)
    {
        destinationName.text = destName;
    }
}
