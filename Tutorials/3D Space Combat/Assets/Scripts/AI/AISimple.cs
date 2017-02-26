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

    private Transform _target;
    private float distance;
    private float maneuverTimer;
    private float _chooseTargetTimer;
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
    private bool _attackInitialized;

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
        if (GameManager.instance != null && GameManager.instance.Player != null && !IsTargetInRange(GameManager.playerTransform.position)) return;
        if (_target == null && Time.time >= _chooseTargetTimer)
        {
            _chooseTargetTimer = Time.time + Random.Range(5, 30);
            ChooseTarget();
        }

        // Evade for duration of timer or if there are no more targets available
        if (attackRunTimer < attackRunWait || _target == null)
        {
            attackRunTimer += Time.deltaTime;

            EvadeManeuver();
        }
        // Attack run
        else
        {
            AttackManeuver();
        }
	}

    private void ChooseTarget()
    {
        var targets = FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[];
        if (targets == null || targets.Length == 0) return;
        var enemyTargets = targets.Where(t => IsEnemy(t.Allegiance));
        if (enemyTargets == null || enemyTargets.Count() == 0) return;
        var closeEnemyTargets = enemyTargets.Where(t => IsTargetInRange(t.transform.position));
        if (closeEnemyTargets == null || closeEnemyTargets.Count() == 0) return;

        var targetableObject = closeEnemyTargets.ToList()[Random.Range(0, closeEnemyTargets.Count() - 1)];
        _target = targetableObject.transform;
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
        if(Vector3.Distance(transform.position, _target.position) > 30)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_target.position - transform.position), rotationDamping * 2f);
            weaponController.Fire();
            rb.AddForce(transform.forward * moveSpeed);
        }
        // Switch back to evade
        else
        {
            attackRunWait = Random.Range(5f, 15f);
            attackRunTimer = 0;
            _attackInitialized = false;
        }
    }
}
