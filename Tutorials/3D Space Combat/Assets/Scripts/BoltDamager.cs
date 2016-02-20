using UnityEngine;
using System.Collections;

public class BoltDamager : MonoBehaviour {

    public int damage;
    public string targetTag;

    private Collider thisCollider;

    void Start()
    {
        thisCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other != thisCollider && other.CompareTag(targetTag))
        {
            DamageInfo damageInfo = new DamageInfo(gameObject, damage);
            other.transform.SendMessage("Damage", damageInfo);
            other.transform.SendMessage("ImpactExplosion", transform.position);
            Destroy(gameObject);
        }
    }
}
