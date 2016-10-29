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
    public Shield shield;

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

    public void Damage(DamageInfo damageInfo)
    {
        if (CompareTag("Player"))
        {
            GameManager.instance.cameraController.ShakeCamera(0.3f, 10f, 0.3f);
        }

        if (shield != null && shield.Charge > 0f)
        {
            return;
        }

        health -= damageInfo.Damage;

        if(healthBar != null)
        {
            healthBar.fillAmount = (float)health / (float)maxHealth;
        }

        if (health <= 0)
        {
            GameObject explosion = Instantiate(destroyExplosion, transform.position, transform.rotation) as GameObject;

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

    public void ImpactExplosion(Vector3 position)
    {
        if (shield != null && shield.Charge > 0f)
        {
            return;
        }

        Instantiate(impactExplosion, position, transform.rotation);
    }
}
