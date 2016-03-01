using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DestinationIndicator : MonoBehaviour {

    public Image entryPoint;
    public Text destinationName;

    public void SetAll(string name, Vector3 namePosition, Vector3 entryPosition)
    {
        SetDestinationName(name);
        SetNamePosition(namePosition);
        SetEntryPointPosition(entryPosition);
    }

    public void SetEntryPointPosition(Vector3 entryPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(entryPosition);
        entryPoint.rectTransform.anchoredPosition = screenPosition;
        print(string.Format("Entry point screen position: {0}", entryPoint.rectTransform.anchoredPosition));
    }

    public void SetNamePosition(Vector3 namePosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(namePosition);
        destinationName.rectTransform.anchoredPosition = screenPosition;
        print(string.Format("Name screen position: {0}", destinationName.rectTransform.anchoredPosition));
    }

    public void SetDestinationName(string destName)
    {
        destinationName.text = destName;
    }
}
