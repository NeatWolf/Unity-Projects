using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BoltDamager : MonoBehaviour {

    [SerializeField]
    private int damage;
    [SerializeField]
    private Enums.Allegiance allegiance;

    private Collider _thisCollider;
    private Rigidbody _rb;
    private TargetableObject _otherTarget;
    private float _speed;

    void Start()
    {
        _thisCollider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _speed = GetComponent<BoltMover>().speed;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.rotation.eulerAngles, out hit))
        {
            // distance = velocity * time
            float distanceForHit = _rb.velocity.magnitude * Time.deltaTime;
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
        _otherTarget = other.GetComponent<TargetableObject>();
        if (_thisCollider != null && _otherTarget != null)
        {
            if (_otherTarget.Allegiance != allegiance)
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
