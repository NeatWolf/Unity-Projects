using UnityEngine;
using System.Collections;

public class RotateToCursor : MonoBehaviour {

    [SerializeField]
    private float defaultDistance;

    private float _distance;
    private Ray _ray;
    private RaycastHit _hit;

    void Update ()
    {
        // Adjust the distance to shoot towards based on the collider we're looking at
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit) && !_hit.collider.CompareTag("Player") && !_hit.collider.CompareTag("PlayerChild"))
        {
            _distance = _hit.distance;
        }
        else
        {
            _distance = defaultDistance;
        }

        // Get the direction the mouse is pointing
        Vector3 targetPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distance);
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);
        transform.LookAt(targetPosition);
    }
}
