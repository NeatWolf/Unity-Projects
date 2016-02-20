using UnityEngine;
using System.Collections;

public class BoltDamager : MonoBehaviour {

    public float damage;
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
            other.transform.SendMessage("Damage", damage);
            other.transform.SendMessage("ImpactExplosion", transform.position);
            Destroy(gameObject);
        }
    }
}
