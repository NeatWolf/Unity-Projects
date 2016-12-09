using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class IndicatorController : MonoBehaviour
{
    public TargetIndicator boxIndicatorPrefab;
    public Image arrowIndicatorPrefab;
    public DestinationIndicator warpIndicatorPrefab;
    public Image objectiveArrowPrefab;
    public ObjectiveIndicator objectiveIconPrefab;
    public float projectileSpeed;

    private List<TargetIndicator> boxIndicatorPool = new List<TargetIndicator>();
    private int boxPoolUsedCount = 0;
    private List<Image> arrowIndicatorPool = new List<Image>();
    private int arrowPoolUsedCount = 0;
    private List<Image> objectiveArrowPool = new List<Image>();
    private int objectiveArrowPoolUsedCount = 0;
    private List<ObjectiveIndicator> objectiveIconPool = new List<ObjectiveIndicator>();
    private int objectiveIconPoolUsedCount = 0;
    private Vector3 screenCenter;
    private DestinationIndicator warpIndicatorInstance;

    private readonly float minAlpha = 75f;
    private readonly float maxAlpha = 200f;
    private readonly float alphaThreshold = 60f;
    private Image lastArrow;

    private enum ArrowType
    {
        enemy,
        waypoint
    }

    void Start()
    {
        screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        warpIndicatorInstance = Instantiate(warpIndicatorPrefab);
        warpIndicatorInstance.transform.SetParent(transform);
    }

    void LateUpdate()
    {
        resetPool();

        if (!GameManager.instance.isMenuOpen)
        {
            // POSITION OBJECTIVE ARROWS AND INDICATORS
            Vector3[] waypoints = GameManager.questManager.GetActiveQuestObjectiveTargets();
            if (waypoints != null)
            {
                foreach (Vector3 waypoint in waypoints)
                {
                    Vector3 targetPosition = Camera.main.WorldToScreenPoint(waypoint);

                    // If the target is onscreen show the onscreen indicator
                    if (targetPosition.z > 0f && targetPosition.x >= 0f && targetPosition.x <= Screen.width && targetPosition.y >= 0f && targetPosition.y <= Screen.height)
                    {
                        if (targetPosition.z > 500f)
                        {
                            ObjectiveIndicator indicatorImage = getObjectiveIcon();
                            indicatorImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(targetPosition.x, targetPosition.y, 0f);
                            indicatorImage.SetDistance(Vector3.Distance(waypoint, GameManager.playerTransform.position));
                        }
                    }
                    else
                    {
                        PositionArrowIndicator(targetPosition, ArrowType.waypoint);
                    }
                }
            }

            // POSITION ENEMY ARROWS AND BOXES
            var objects = (GameObject.FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[]).Where(t => t.allegiance == TargetableObject.Allegiance.Enemy);

            foreach (TargetableObject obj in objects)
            {
                if (GameManager.playerTransform != null)
                {
                    if (Vector3.Distance(obj.transform.position, GameManager.playerTransform.position) < 500f)
                    {
                        Vector3 targetPosition = Camera.main.WorldToScreenPoint(obj.transform.position);

                        // If the target is onscreen show the onscreen indicator & health bar
                        if (targetPosition.z > 0f && targetPosition.x >= 0f && targetPosition.x <= Screen.width && targetPosition.y >= 0f && targetPosition.y <= Screen.height)
                        {
                            TargetIndicator box = getBoxIndicator();
                            box.anchoredPosition = new Vector3(targetPosition.x, targetPosition.y, 0f);
                            box.healthBarFillAmount = (float)obj.GetComponent<HealthController>().Health / (float)obj.GetComponent<HealthController>().maxHealth;

                            float multiplier = (maxAlpha - minAlpha) / alphaThreshold;
                            float currentAlpha = maxAlpha;
                            box.healthBarVisible = false;
                            if (targetPosition.z < alphaThreshold)
                            {
                                box.healthBarVisible = true;
                                currentAlpha = minAlpha + (targetPosition.z * multiplier);
                            }
                            
                            box.boxAlpha = currentAlpha / 255f;

                            //Vector3 lead = CalculateLead(player.transform.position, obj.transform.position, projectileSpeed * 1.5f, obj.gameObject.GetComponent<Rigidbody>().velocity, player.GetComponent<Rigidbody>().velocity);
                            //box.trajectory.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(lead) - screenCenter;
                        }
                        else // Offscreen - show directional arrow
                        {
                            if (waypoints == null || !OverlapsWaypoint(waypoints, obj.transform.position))
                            {
                                PositionArrowIndicator(targetPosition, ArrowType.enemy);
                            }
                        }
                    }
                }
            }
            

            // Warp target indicators
            warpIndicatorInstance.gameObject.SetActive(false);
            if (GameManager.instance.isCursorVisible)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                int layerMask = 1 << 2;
                layerMask = ~layerMask;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    WarpTarget target = hit.collider.gameObject.GetComponent<WarpTarget>();
                    if (target != null)
                    {
                        warpIndicatorInstance.gameObject.SetActive(true);
                        warpIndicatorInstance.SetDestinationName(target.targetName);

                        // Works with sphere colliders
                        Vector3 centerPosition = Camera.main.WorldToScreenPoint(target.targetBoundary.bounds.center);
                        float diffPosition = Camera.main.WorldToScreenPoint(target.targetBoundary.bounds.max).y - centerPosition.y;
                        Vector3 topPosition = centerPosition + new Vector3(0f, diffPosition * 0.8f, 0f);

                        warpIndicatorInstance.SetNamePosition(topPosition);

                        // Disable entry point indicator if it is currently overlapping an objective marker
                        if (waypoints == null || !OverlapsWaypoint(waypoints, target.targetTransform.position))
                        {
                            warpIndicatorInstance.SetEntryPointPosition(target.targetTransform.position);
                        }
                        else
                        {
                            warpIndicatorInstance.DisableEntryPoint();
                        }
                    }
                }
            }
        }
        cleanPool();
    }

    private void resetPool()
    {
        boxPoolUsedCount = 0;
        arrowPoolUsedCount = 0;
        objectiveArrowPoolUsedCount = 0;
        objectiveIconPoolUsedCount = 0;
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
            box.transform.SetParent(transform);
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
            arrow.transform.SetParent(transform);
            arrowIndicatorPool.Add(arrow);
        }

        arrowPoolUsedCount++;
        return arrow;
    }

    private Image getObjectiveArrow()
    {
        Image arrow;
        if (objectiveArrowPoolUsedCount < objectiveArrowPool.Count)
        {
            arrow = objectiveArrowPool[objectiveArrowPoolUsedCount];
        }
        else
        {
            arrow = Instantiate(objectiveArrowPrefab);
            arrow.transform.SetParent(transform);
            objectiveArrowPool.Add(arrow);
        }

        objectiveArrowPoolUsedCount++;
        return arrow;
    }

    private ObjectiveIndicator getObjectiveIcon()
    {
        ObjectiveIndicator icon;
        if (objectiveIconPoolUsedCount < objectiveIconPool.Count)
        {
            icon = objectiveIconPool[objectiveIconPoolUsedCount];
        }
        else
        {
            icon = Instantiate(objectiveIconPrefab);
            icon.transform.SetParent(transform);
            objectiveIconPool.Add(icon);
        }

        objectiveIconPoolUsedCount++;
        return icon;
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

        // Objective waypoint offscreen arrows
        while (objectiveArrowPool.Count > objectiveArrowPoolUsedCount)
        {
            Image lastArrow = objectiveArrowPool[objectiveArrowPool.Count - 1];
            objectiveArrowPool.Remove(lastArrow);
            Destroy(lastArrow.gameObject);
        }

        // Objective waypoint onscreen indicator
        while (objectiveIconPool.Count > objectiveIconPoolUsedCount)
        {
            ObjectiveIndicator lastIcon = objectiveIconPool[objectiveIconPool.Count - 1];
            objectiveIconPool.Remove(lastIcon);
            Destroy(lastIcon.gameObject);
        }
    }

    private Vector3 CalculateLead(Vector3 shooterPosition, Vector3 targetPosition, float projectileSpeed, Vector3 targetVelocity, Vector3 playerVelocity)
    {
        Vector3 V = targetVelocity - playerVelocity;
        Vector3 D = targetPosition - shooterPosition;
        float A = V.sqrMagnitude - projectileSpeed * projectileSpeed;
        float B = 2 * Vector3.Dot(D, V);
        float C = D.sqrMagnitude;
        if (A >= 0)
        {
            Debug.LogError("No solution exists");
            return targetPosition;
        }
        else {
            float rt = Mathf.Sqrt(B * B - 4 * A * C);
            float dt1 = (-B + rt) / (2 * A);
            float dt2 = (-B - rt) / (2 * A);
            float dt = (dt1 < 0 ? dt2 : dt1);
            return targetPosition + V * dt;
        }
    }

    private void PositionArrowIndicator(Vector3 targetPosition, ArrowType arrowType)
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

        Vector3 screenBounds = screenCenter * 0.97f;

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

        Image arrow;
        switch (arrowType)
        {
            case ArrowType.enemy:
                arrow = getArrowIndicator();
                break;
            case ArrowType.waypoint:
                arrow = getObjectiveArrow();
                break;
            default:
                arrow = getArrowIndicator();
                break;
        }
        
        arrow.rectTransform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        arrow.rectTransform.anchoredPosition = targetPosition;
    }

    private bool OverlapsWaypoint(Vector3[] waypoints, Vector3 target)
    {
        foreach (var waypoint in waypoints)
        {
            if (target == waypoint)
            {
                return true;
            }
        }
        return false;
    }
}
