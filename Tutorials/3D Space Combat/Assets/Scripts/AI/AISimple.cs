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
    private Enums.Allegiance allegiance;

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
        allegiance = GetComponent<TargetableObject>().Allegiance;
    }

    void Start()
    {
        evadeDirectionWait = Random.Range(1f, 3f);
        attackRunWait = Random.Range(5f, 15f);
        ChooseTarget();
    }
	
	void FixedUpdate ()
    {
        if(target != null)// && Vector3.Distance(GameManager.playerTransform.position, transform.position) < 2000f)
        {
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
        else
        {
            ChooseTarget();
        }
	}

    private void ChooseTarget()
    {
        if (Time.time >= chooseTargetTimer || target == null)
        {
            chooseTargetTimer = Time.time + Random.Range(5, 30);

            var objects = (FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[]).Where(t => t.Allegiance == Enums.Allegiance.Friendly).ToList();

            // Check if there is an appropriate target
            if (objects != null && objects.Count > 0)
            {
                targetableObject = objects[Random.Range(0, objects.Count)];
                target = targetableObject.transform;
            }
            else
            {
                target = null;
            }
        }
    }

    private void Shooting()
    {
        if (Time.time >= maneuverTimer)
        {
            maneuverTimer = Time.time + Random.Range(5, 10);
            currentManeuver = (Maneuver)Random.Range(0, 2);
            print(string.Format("currentManeuver: {0}", currentManeuver));
            if (currentManeuver == Maneuver.Sniping)
            {
                isStrafingRight = RandomBoolean();
            }
        }
        else
        {
            switch (currentManeuver)
            {
                case Maneuver.Attack:
                    PerformAttackManeuver();
                    break;
                case Maneuver.Sniping:
                    PerformSnipingManeuver(isStrafingRight);
                    break;
            }
        }
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

    private void PerformAttackManeuver()
    {
        transform.LookAt(target);
        //Quaternion lookAtRotation = Quaternion.LookRotation(target.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, lookSpeed);
        weaponController.Fire();

        if (distance > followRange)
        {
            rb.AddForce(transform.forward * moveSpeed);
        }
    }

    private void PerformSnipingManeuver(bool strafeRight)
    {
        if (distance < snipeRange)
        {
            //Quaternion lookAtRotation = Quaternion.LookRotation(2.0f * transform.position - target.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, lookSpeed);
            transform.LookAt(2.0f * transform.position - target.position);
            rb.AddForce(transform.forward * moveSpeed);
        }
        else
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(target.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, lookSpeed);
            if (strafeRight)
            {
                rb.AddForce(transform.right * strafeSpeed);
            }
            else
            {
                rb.AddForce(transform.right * -strafeSpeed);
            }
            //lookAtRotation = Quaternion.LookRotation(target.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, lookSpeed);
            transform.LookAt(target);
            weaponController.Fire();
        }
    }

    private bool RandomBoolean()
    {
        if(Random.value >= 0.5f)
        {
            return true;
        }
        return false;
    }
}
