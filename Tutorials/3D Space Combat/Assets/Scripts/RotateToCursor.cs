using UnityEngine;
using System.Collections;

public class RotateToCursor : MonoBehaviour {

    public float distance;

    private Ray ray;
    private RaycastHit hit;

    void Update ()
    {
        // Adjust the distance to shoot towards based on the collider we're looking at
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            distance = hit.distance;
        }

        // Get the direction the mouse is pointing
        Vector3 targetPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);
        transform.LookAt(targetPosition);
    }
}
