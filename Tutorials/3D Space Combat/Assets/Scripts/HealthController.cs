using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

    public GameObject destroyExplosion;
    public GameObject impactExplosion;
    public float health;

    void Start() { }

    void Update()
    {
        if(health <= 0)
        {
            GameObject explosion = Instantiate(destroyExplosion, transform.position, transform.rotation) as GameObject;
            Detonator explosionDetonator = explosion.GetComponent<Detonator>();
            explosionDetonator.size = transform.localScale.x * 2;
            Destroy(gameObject);
        }
    }

    void Damage(float damage)
    {
        health -= damage;
    }

    void ImpactExplosion(Vector3 position)
    {
        Instantiate(impactExplosion, position, transform.rotation);
    }
}
