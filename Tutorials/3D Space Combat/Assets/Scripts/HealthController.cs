using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class HealthController : MonoBehaviour
{
    public GameObject destroyExplosion;
    public GameObject impactExplosion;
    public int maxHealth;
    public int xp;
    public UIProgressBarController healthBar;

    private int health;
    private LevelUpSystem levelUpSystem;

    void Start()
    {
        health = maxHealth;
        levelUpSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelUpSystem>();
    }

    public int Health
    {
        get
        {
            return health;
        }
    }

    void Damage(DamageInfo damageInfo)
    {
        health -= damageInfo.Damage;

        if(healthBar != null)
        {
            healthBar.fillAmount = (float)health / (float)maxHealth;
        }

        if (health <= 0)
        {
            GameObject explosion = Instantiate(destroyExplosion, transform.position, transform.rotation) as GameObject;
            Detonator explosionDetonator = explosion.GetComponent<Detonator>();
            explosionDetonator.size = transform.localScale.x * 2;

            if (damageInfo.Sender.CompareTag("PlayerWeapon"))
            {
                levelUpSystem.GainExperience(xp);
            }

            Destroy(gameObject);
        }
    }

    void ImpactExplosion(Vector3 position)
    {
        Instantiate(impactExplosion, position, transform.rotation);
    }
}
