using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class IndicatorController : MonoBehaviour
{
    public TargetIndicator boxIndicatorPrefab;
    public Image arrowIndicatorPrefab;

    private List<TargetIndicator> boxIndicatorPool = new List<TargetIndicator>();
    private int boxPoolUsedCount = 0;
    private List<Image> arrowIndicatorPool = new List<Image>();
    private int arrowPoolUsedCount = 0;

    private Vector3 screenCenter;

    void Start()
    {
        screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
    }

    void LateUpdate()
    {
        resetPool();
        TargetableObject[] objects = GameObject.FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[];

        foreach(TargetableObject obj in objects)
        {
            Vector3 targetPosition = Camera.main.WorldToScreenPoint(obj.transform.position);

            // If the target is onscreen show the onscreen indicator & health bar
            if (targetPosition.z > 0f && targetPosition.x >= 0f && targetPosition.x <= Screen.width && targetPosition.y >= 0f && targetPosition.y <= Screen.height)
            {
                TargetIndicator box = getBoxIndicator();
                box.anchoredPosition = new Vector3(targetPosition.x, targetPosition.y, 0f);
            }
            else // Offscreen - show directional arrow
            {
                arrowIndicatorPrefab.enabled = true;

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

                Image arrow = getArrowIndicator();
                arrow.rectTransform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                arrow.rectTransform.anchoredPosition = targetPosition;
            }
        }
        cleanPool();
    }

    private void resetPool()
    {
        boxPoolUsedCount = 0;
        arrowPoolUsedCount = 0;
    }

    private TargetIndicator getBoxIndicator()
    {
        TargetIndicator box;
        if(boxPoolUsedCount < boxIndicatorPool.Count)
        {
            box = boxIndicatorPool[boxPoolUsedCount];
        }
        else
        {
            box = Instantiate(boxIndicatorPrefab);
            box.transform.parent = transform;
            boxIndicatorPool.Add(box);
        }

        boxPoolUsedCount++;
        return box;
    }

    private Image getArrowIndicator()
    {
        Image arrow;
        if (arrowPoolUsedCount < arrowIndicatorPool.Count)
        {
            arrow = arrowIndicatorPool[arrowPoolUsedCount];
        }
        else
        {
            arrow = Instantiate(arrowIndicatorPrefab);
            arrow.transform.parent = transform;
            arrowIndicatorPool.Add(arrow);
        }

        arrowPoolUsedCount++;
        return arrow;
    }

    private void cleanPool()
    {
        while(arrowIndicatorPool.Count > arrowPoolUsedCount)
        {
            Image lastArrow = arrowIndicatorPool[arrowIndicatorPool.Count - 1];
            arrowIndicatorPool.Remove(lastArrow);
            Destroy(lastArrow.gameObject);
        }

        while (boxIndicatorPool.Count > boxPoolUsedCount)
        {
            TargetIndicator lastBox = boxIndicatorPool[boxIndicatorPool.Count - 1];
            boxIndicatorPool.Remove(lastBox);
            Destroy(lastBox.gameObject);
        }
    }
}
