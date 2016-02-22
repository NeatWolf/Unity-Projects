using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IndicatorController : MonoBehaviour
{

    public Transform target;
    public Image offScreenIndicator;
    public Image healthBarFull;
    public Image healthBarEmpty;

    private Vector3 screenCenter;

    void Start()
    {
        screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
    }

    void LateUpdate()
    {
        offScreenIndicator.enabled = false;
        Vector3 targetPosition = Camera.main.WorldToScreenPoint(target.position);

        // If the target is onscreen show the onscreen indicator & health bar
        if (targetPosition.z > 0f && targetPosition.x >= 0f && targetPosition.x <= Screen.width && targetPosition.y >= 0f && targetPosition.y <= Screen.height)
        {
            print(targetPosition.ToString());
            healthBarEmpty.rectTransform.anchoredPosition = new Vector3(targetPosition.x, targetPosition.y -25, 0f);
            healthBarFull.rectTransform.anchoredPosition = new Vector3(targetPosition.x, targetPosition.y -25, 0f);
        }
        else // Offscreen - show directional arrow
        {
            offScreenIndicator.enabled = true;

            if (targetPosition.z < 0)
            {
                targetPosition *= -1;
            }

            // Make origin the center of the screen instead of bottom-left
            targetPosition -= screenCenter;

            // Calculate the angle from the center of the screen to the target off-screen
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x);
            angle -= 90 * Mathf.Deg2Rad;

            float cos = Mathf.Cos(angle);
            float sin = -Mathf.Sin(angle);

            targetPosition = screenCenter + new Vector3(sin * 150, cos * 150, 0);

            float m = cos / sin;

            Vector3 screenBounds = screenCenter * 0.9f;

            // Top and bottom
            if (cos > 0)
            {
                targetPosition = new Vector3(screenBounds.y / m, screenBounds.y, 0);
            }
            else
            {
                targetPosition = new Vector3(-screenBounds.y / m, -screenBounds.y, 0);
            }

            // Right and left
            if (targetPosition.x > screenBounds.x)
            {
                targetPosition = new Vector3(screenBounds.x, screenBounds.x * m, 0);
            }
            else if (targetPosition.x < -screenBounds.x)
            {
                targetPosition = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
            }

            // Move origin back to bottom-left
            targetPosition += screenCenter;

            offScreenIndicator.rectTransform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            offScreenIndicator.rectTransform.anchoredPosition = targetPosition;
        }
    }
}
