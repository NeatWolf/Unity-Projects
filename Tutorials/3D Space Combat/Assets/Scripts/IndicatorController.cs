using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class IndicatorController : MonoBehaviour
{
    [SerializeField]
    private TargetIndicator boxIndicatorPrefab;
    [SerializeField]
    private Image arrowIndicatorPrefab;
    [SerializeField]
    private DestinationIndicator warpIndicatorPrefab;
    [SerializeField]
    private Image objectiveArrowPrefab;
    [SerializeField]
    private ObjectiveIndicator objectiveIconPrefab;
    [SerializeField]
    private float projectileSpeed;

    private List<TargetIndicator> _boxIndicatorPool = new List<TargetIndicator>();
    private int _boxPoolUsedCount = 0;
    private List<Image> _arrowIndicatorPool = new List<Image>();
    private int _arrowPoolUsedCount = 0;
    private List<Image> _objectiveArrowPool = new List<Image>();
    private int _objectiveArrowPoolUsedCount = 0;
    private List<ObjectiveIndicator> _objectiveIconPool = new List<ObjectiveIndicator>();
    private int _objectiveIconPoolUsedCount = 0;
    private Vector3 _screenCenter;
    private DestinationIndicator _warpIndicatorInstance;

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
        _screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        _warpIndicatorInstance = Instantiate(warpIndicatorPrefab);
        _warpIndicatorInstance.transform.SetParent(transform);
    }

    void LateUpdate()
    {
        resetPool();

        if (!GameManager.instance.IsMenuOpen)
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
            var enemies = GameManager.instance.TargetableObjects.Where(t => t.Allegiance == Enums.Allegiance.Enemy);

            foreach (TargetableObject obj in enemies)
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
                            box.healthBarFillAmount = (float)obj.GetComponent<HealthController>().Health / (float)obj.GetComponent<HealthController>().MaxHealth;

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
            _warpIndicatorInstance.gameObject.SetActive(false);
            if (GameManager.instance.IsCursorVisible)
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
                        _warpIndicatorInstance.gameObject.SetActive(true);
                        _warpIndicatorInstance.SetDestinationName(target.TargetName);

                        // Works with sphere colliders
                        Vector3 centerPosition = Camera.main.WorldToScreenPoint(target.Bounds.center);
                        float diffPosition = Camera.main.WorldToScreenPoint(target.Bounds.max).y - centerPosition.y;
                        Vector3 topPosition = centerPosition + new Vector3(0f, diffPosition * 0.8f, 0f);

                        _warpIndicatorInstance.SetNamePosition(topPosition);

                        // Disable entry point indicator if it is currently overlapping an objective marker
                        if (waypoints == null || !OverlapsWaypoint(waypoints, target.Position))
                        {
                            _warpIndicatorInstance.SetEntryPointPosition(target.Position);
                        }
                        else
                        {
                            _warpIndicatorInstance.DisableEntryPoint();
                        }
                    }
                }
            }
        }
        cleanPool();
    }

    private void resetPool()
    {
        _boxPoolUsedCount = 0;
        _arrowPoolUsedCount = 0;
        _objectiveArrowPoolUsedCount = 0;
        _objectiveIconPoolUsedCount = 0;
    }

    private TargetIndicator getBoxIndicator()
    {
        TargetIndicator box;
        if(_boxPoolUsedCount < _boxIndicatorPool.Count)
        {
            box = _boxIndicatorPool[_boxPoolUsedCount];
        }
        else
        {
            box = Instantiate(boxIndicatorPrefab);
            box.transform.SetParent(transform);
            _boxIndicatorPool.Add(box);
        }

        _boxPoolUsedCount++;
        return box;
    }

    private Image getArrowIndicator()
    {
        Image arrow;
        if (_arrowPoolUsedCount < _arrowIndicatorPool.Count)
        {
            arrow = _arrowIndicatorPool[_arrowPoolUsedCount];
        }
        else
        {
            arrow = Instantiate(arrowIndicatorPrefab);
            arrow.transform.SetParent(transform);
            _arrowIndicatorPool.Add(arrow);
        }

        _arrowPoolUsedCount++;
        return arrow;
    }

    private Image getObjectiveArrow()
    {
        Image arrow;
        if (_objectiveArrowPoolUsedCount < _objectiveArrowPool.Count)
        {
            arrow = _objectiveArrowPool[_objectiveArrowPoolUsedCount];
        }
        else
        {
            arrow = Instantiate(objectiveArrowPrefab);
            arrow.transform.SetParent(transform);
            _objectiveArrowPool.Add(arrow);
        }

        _objectiveArrowPoolUsedCount++;
        return arrow;
    }

    private ObjectiveIndicator getObjectiveIcon()
    {
        ObjectiveIndicator icon;
        if (_objectiveIconPoolUsedCount < _objectiveIconPool.Count)
        {
            icon = _objectiveIconPool[_objectiveIconPoolUsedCount];
        }
        else
        {
            icon = Instantiate(objectiveIconPrefab);
            icon.transform.SetParent(transform);
            _objectiveIconPool.Add(icon);
        }

        _objectiveIconPoolUsedCount++;
        return icon;
    }

    private void cleanPool()
    {
        while(_arrowIndicatorPool.Count > _arrowPoolUsedCount)
        {
            Image lastArrow = _arrowIndicatorPool[_arrowIndicatorPool.Count - 1];
            _arrowIndicatorPool.Remove(lastArrow);
            Destroy(lastArrow.gameObject);
        }

        while (_boxIndicatorPool.Count > _boxPoolUsedCount)
        {
            TargetIndicator lastBox = _boxIndicatorPool[_boxIndicatorPool.Count - 1];
            _boxIndicatorPool.Remove(lastBox);
            Destroy(lastBox.gameObject);
        }

        // Objective waypoint offscreen arrows
        while (_objectiveArrowPool.Count > _objectiveArrowPoolUsedCount)
        {
            Image lastArrow = _objectiveArrowPool[_objectiveArrowPool.Count - 1];
            _objectiveArrowPool.Remove(lastArrow);
            Destroy(lastArrow.gameObject);
        }

        // Objective waypoint onscreen indicator
        while (_objectiveIconPool.Count > _objectiveIconPoolUsedCount)
        {
            ObjectiveIndicator lastIcon = _objectiveIconPool[_objectiveIconPool.Count - 1];
            _objectiveIconPool.Remove(lastIcon);
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
        targetPosition -= _screenCenter;

        // Calculate the angle from the center of the screen to the target off-screen
        float angle = Mathf.Atan2(targetPosition.y, targetPosition.x);
        angle -= 90 * Mathf.Deg2Rad;

        float cos = Mathf.Cos(angle);
        float sin = -Mathf.Sin(angle);

        targetPosition = _screenCenter + new Vector3(sin * 150, cos * 150, 0);

        float m = cos / sin;

        Vector3 screenBounds = _screenCenter * 0.97f;

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
        targetPosition += _screenCenter;

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
