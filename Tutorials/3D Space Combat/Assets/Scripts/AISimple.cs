using UnityEngine;
using System.Collections;

public class AISimple : MonoBehaviour {

    public float followRange;
    public float snipeRange;
    public float moveSpeed;
    public float combatBoostSpeed;
    public float strafeSpeed;

    private TargetableObject targetableObject;
    private Transform target;
    private float distance;
    private float maneuverTimer;
    private float chooseTargetTimer;
    private Rigidbody rb;
    private AIWeaponController weaponController;
    private Maneuver currentManeuver;
    private bool isStrafingRight;

    private enum Maneuver
    {
        Attack,
        Sniping
    };

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        weaponController = GetComponent<AIWeaponController>();
    }
	
	void FixedUpdate ()
    {
        ChooseTarget();

        if (target != null)
        {
            distance = Vector3.Distance(target.position, transform.position);
            if (distance < 500f)
            {
                Shooting();
            }
        }
        else
        {
            // Fight is finished
        }
	}

    private void ChooseTarget()
    {
        if (Time.time >= chooseTargetTimer || target == null)
        {
            chooseTargetTimer = Time.time + Random.Range(5, 30);

            int attempts = 100;
            TargetableObject[] objects = FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[];
            if(objects.Length > 0)
            {
                do
                {
                    targetableObject = objects[Random.Range(0, objects.Length)];
                    target = targetableObject.transform;
                    attempts--;
                    if(attempts <= 0)
                    {
                        target = null;
                        break;
                    }
                } while (target == null || targetableObject.allegiance == GetComponent<TargetableObject>().allegiance);
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

    private void PerformAttackManeuver()
    {
        transform.LookAt(target);
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
            transform.LookAt(2.0f * transform.position - target.position);
            rb.AddForce(transform.forward * moveSpeed);
        }
        else
        {
            transform.LookAt(target);
            if (strafeRight)
            {
                rb.AddForce(transform.right * strafeSpeed);
            }
            else
            {
                rb.AddForce(transform.right * -strafeSpeed);
            }
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
