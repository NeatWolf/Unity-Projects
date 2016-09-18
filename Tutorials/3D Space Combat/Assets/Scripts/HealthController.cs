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
    private AudioSource audioSource;

    void Start()
    {
        health = maxHealth;
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if(playerGO != null)
        {
            levelUpSystem = playerGO.GetComponent<LevelUpSystem>();
        }
        else
        {
            Debug.LogError("Player is missing from the scene");
        }
        audioSource = GetComponent<AudioSource>();
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
        if (CompareTag("Player"))
        {
            GameManager.instance.cameraController.ShakeCamera(0.5f, 10f, 0.2f);
        }
        health -= damageInfo.Damage;

        if(healthBar != null)
        {
            healthBar.fillAmount = (float)health / (float)maxHealth;
        }

        if (health <= 0)
        {
            GameObject explosion = Instantiate(destroyExplosion, transform.position, transform.rotation) as GameObject;
            //Detonator explosionDetonator = explosion.GetComponent<Detonator>();
            //explosionDetonator.size = transform.localScale.x * 2;

            if (levelUpSystem != null && damageInfo.Sender.CompareTag("PlayerWeapon"))
            {
                levelUpSystem.GainExperience(xp);
            }

            if (CompareTag("Player"))
            {
                GameManager.instance.GameOver();
            }

            Destroy(gameObject);
        }
    }

    void ImpactExplosion(Vector3 position)
    {
        Instantiate(impactExplosion, position, transform.rotation);
    }
}
