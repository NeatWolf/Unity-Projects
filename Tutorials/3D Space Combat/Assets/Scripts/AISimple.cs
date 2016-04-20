using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TargetableObject))]
public class AISimple : MonoBehaviour {

    public float followRange;
    public float snipeRange;
    public float moveSpeed;
    public float combatBoostSpeed;
    public float strafeSpeed;
    public float lookSpeed;

    private TargetableObject targetableObject;
    private Transform target;
    private float distance;
    private float maneuverTimer;
    private float chooseTargetTimer;
    private Rigidbody rb;
    private AIWeaponController weaponController;
    private Maneuver currentManeuver;
    private bool isStrafingRight;
    private TargetableObject.Allegiance allegiance;

    private enum Maneuver
    {
        Attack,
        Sniping
    };

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        weaponController = GetComponent<AIWeaponController>();
        allegiance = GetComponent<TargetableObject>().allegiance;
    }
	
	void FixedUpdate ()
    {
        if (Vector3.Distance(GameManager.playerTransform.position, transform.position) < 2000f)
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
	}

    private void ChooseTarget()
    {
        if (Time.time >= chooseTargetTimer || target == null)
        {
            chooseTargetTimer = Time.time + Random.Range(5, 30);

            bool possibleTarget = false;
            TargetableObject[] objects = FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[];

            // Check if there is an appropriate target
            foreach(var obj in objects)
            {
                if(obj.allegiance != allegiance)
                {
                    possibleTarget = true;
                }
            }

            // Choose a random target
            if(possibleTarget)
            {
                do
                {
                    targetableObject = objects[Random.Range(0, objects.Length)];
                    target = targetableObject.transform;
                } while (target == null || targetableObject.allegiance == allegiance);
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
