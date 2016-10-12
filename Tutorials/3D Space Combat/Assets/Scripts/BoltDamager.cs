using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BoltDamager : MonoBehaviour {

    public int damage;
    public TargetableObject.Allegiance targetAllegiance;

    private Collider thisCollider;
    private TargetableObject otherTarget;

    void Start()
    {
        thisCollider = GetComponent<Collider>();
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
