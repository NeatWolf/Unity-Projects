using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {

    public GameObject destroyExplosion;
    public GameObject impactExplosion;
    public float maxHealth;
    public Image healthBar;

    private float health;

    void Start()
    {
        health = maxHealth;
    }

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
        healthBar.fillAmount = health / maxHealth;
    }

    void ImpactExplosion(Vector3 position)
    {
        Instantiate(impactExplosion, position, transform.rotation);
    }
}
