using UnityEngine;
using System.Collections;

public class AISimple : MonoBehaviour {

    public float followRange;
    public float snipeRange;
    public float moveSpeed;
    public float strafeSpeed;

    private Transform player;
    private float distance;
    private float timer;
    private Rigidbody rb;
    private AIWeaponController weaponController;
    private Maneuver currentManeuver;
    private bool isStrafingRight;

    private enum Maneuver
    {
        Attack,
        Sniping
    };

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        weaponController = GetComponent<AIWeaponController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	void FixedUpdate ()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if (distance < 500f)
        {
            if (Time.time >= timer)
            {
                timer = Time.time + Random.Range(5, 10);
                currentManeuver = (Maneuver)Random.Range(0, 2);
                //print(string.Format("currentManeuver: {0}", currentManeuver));
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
	}

    void PerformAttackManeuver()
    {
        transform.LookAt(player);
        weaponController.Fire();

        if(distance > followRange)
        {
            rb.AddForce(transform.forward * moveSpeed);
        }
    }

    void PerformSnipingManeuver(bool strafeRight)
    {
        if (distance < snipeRange)
        {
            transform.LookAt(2.0f * transform.position - player.position);
            rb.AddForce(transform.forward * moveSpeed);
        }
        else
        {
            transform.LookAt(player);
            if (strafeRight)
            {
                rb.AddForce(transform.right * strafeSpeed);
            }
            else
            {
                rb.AddForce(transform.right * -strafeSpeed);
            }
            transform.LookAt(player);
            weaponController.Fire();
        }
    }

    private bool RandomBoolean()
    {
        if(Random.value >= 5)
        {
            return true;
        }
        return false;
    }
}
