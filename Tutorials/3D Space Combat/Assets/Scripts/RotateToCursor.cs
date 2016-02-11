using UnityEngine;
using System.Collections;

public class RotateToCursor : MonoBehaviour {

    public float distance;

	void Update ()
    {
        // Get the direction the mouse is pointing
        Vector3 targetPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);
        transform.LookAt(targetPosition);
    }
}
