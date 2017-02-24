using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TargetableObject))]
public class AISimple : MonoBehaviour {

    public float followRange;
    public float snipeRange;
    public float moveSpeed;
    public float combatBoostSpeed;
    public float strafeSpeed;
    public float lookSpeed;
    public float rotationDamping;

    private TargetableObject targetableObject;
    private Transform target;
    private float distance;
    private float maneuverTimer;
    private float chooseTargetTimer;
    private float evadeDirectionTimer;
    private float evadeDirectionWait;
    private float attackRunTimer;
    private float attackRunWait;
    private Rigidbody rb;
    private AIWeaponController weaponController;
    private Maneuver currentManeuver;
    private bool isStrafingRight;
    private Enums.Allegiance _allegiance;

    private Quaternion rotationChange = Quaternion.identity;
    private bool attackInitialized;

    private enum Maneuver
    {
        Attack,
        Sniping
    };

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        weaponController = GetComponent<AIWeaponController>();
        _allegiance = GetComponent<TargetableObject>().Allegiance;
    }

    void Start()
    {
        evadeDirectionWait = Random.Range(1f, 3f);
        attackRunWait = Random.Range(5f, 15f);
        ChooseTarget();
    }
	
	void FixedUpdate ()
    {
        if (!IsTargetInRange(GameManager.playerTransform.position)) return;
        if (target == null) ChooseTarget();

        // Evading
        if (attackRunTimer < attackRunWait)
        {
            attackRunTimer += Time.deltaTime;

            EvadeManeuver();
        }
        // Attack run
        else
        {
            if (!attackInitialized)
            {
                ChooseTarget();
                attackInitialized = true;
                if (target == null)
                {
                    Debug.Log("Could not find a suitable target");
                }
            }
            else
            {
                if (target != null)
                {
                    AttackManeuver();
                }
                else
                {
                    attackInitialized = false;
                }
            }
        }
	}

    private void ChooseTarget()
    {
        if (Time.time >= chooseTargetTimer || target == null)
        {
            chooseTargetTimer = Time.time + Random.Range(5, 30);

            var targets = FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[];
            var closeTargets = targets.Where(t => IsTargetInRange(t.transform.position));
            var closeEnemyTargets = closeTargets.Where(t => IsEnemy(t.Allegiance)).ToList();

            if (closeEnemyTargets == null || closeEnemyTargets.Count == 0)
            {
                target = null;
                return;
            }
            targetableObject = closeEnemyTargets[Random.Range(0, closeEnemyTargets.Count - 1)];
            target = targetableObject.transform;
        }
    }

    private bool IsTargetInRange(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) < 2000f;
    }

    private bool IsEnemy(Enums.Allegiance allegiance)
    {
        return allegiance != Enums.Allegiance.Inanimate && allegiance != _allegiance;
    }

    private void EvadeManeuver()
    {
        // Perform evade
        if(evadeDirectionTimer < evadeDirectionWait)
        {
            evadeDirectionTimer += Time.deltaTime;

            transform.rotation = Quaternion.Slerp(transform.rotation, rotationChange, rotationDamping);
            //Debug.Log(string.Format("Rotation: ", transform.rotation));
            rb.AddForce(transform.forward * moveSpeed);
        }
        // Choose new direction
        else
        {
            evadeDirectionWait = Random.Range(1f, 3f);
            evadeDirectionTimer = 0;

            rotationChange = transform.rotation * Quaternion.Euler(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
        }
    }

    private void AttackManeuver()
    {
        // Charge towards target until you're close
        if(Vector3.Distance(transform.position, target.position) > 30)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotationDamping * 2f);
            weaponController.Fire();
            rb.AddForce(transform.forward * moveSpeed);
        }
        // Switch back to evade
        else
        {
            attackRunWait = Random.Range(5f, 15f);
            attackRunTimer = 0;
            attackInitialized = false;
        }
    }
}
