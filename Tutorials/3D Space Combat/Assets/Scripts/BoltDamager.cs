using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BoltDamager : MonoBehaviour {

    public int damage;
    public TargetableObject.Allegiance targetAllegiance;

    private Collider thisCollider;
    private Rigidbody rb;
    private TargetableObject otherTarget;
    private float speed;

    void Start()
    {
        thisCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        speed = GetComponent<BoltMover>().speed;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.rotation.eulerAngles, out hit))
        {
            // distance = velocity * time
            float distanceForHit = rb.velocity.magnitude * Time.deltaTime;
            if (distanceForHit > 0f)
            {
                if (hit.distance < distanceForHit)
                {
                    OnTriggerEnter(hit.collider);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        otherTarget = other.GetComponent<TargetableObject>();
        if (thisCollider != null && otherTarget != null)
        {
            if (other != thisCollider)
            {
                if (otherTarget.allegiance == targetAllegiance)
                {
                    DamageInfo damageInfo = new DamageInfo(gameObject, damage);
                    HealthController health = other.GetComponent<HealthController>();
                    Shield shield = other.GetComponent<Shield>();
                    if(shield != null && shield.Charge > 0f)
                    {
                        shield.Damage(damageInfo);
                    }
                    else if (health != null)
                    {
                        health.Damage(damageInfo);
                        health.ImpactExplosion(transform.position);
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}
