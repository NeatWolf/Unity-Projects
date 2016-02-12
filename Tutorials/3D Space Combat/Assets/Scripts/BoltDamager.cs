using UnityEngine;
using System.Collections;

public class BoltDamager : MonoBehaviour {

    public float damage;

	void Start() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.transform.SendMessage("Damage", damage);
            other.transform.SendMessage("ImpactExplosion", transform.position);
            Destroy(gameObject);
        }
    }
}
