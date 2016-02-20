using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyIndicatorsController : MonoBehaviour {

    public Transform target;

	void Update ()
    {
        Vector3 targetPosition = Camera.main.WorldToViewportPoint(target.position);

        // If the target is on-screen don't show the indicator
        if(targetPosition.z > Camera.main.nearClipPlane && targetPosition.x >= 0.0f && targetPosition.x <= 1.0f && targetPosition.y >= 0.0f && targetPosition.y <= 1.0f)
        {
            return;
        }

        // Set indicator to center of the screen
        
        // Calculate the angle from the center of the screen to the target off-screen

        // Rotate indicator to point towards target

        // ...
	}
}
