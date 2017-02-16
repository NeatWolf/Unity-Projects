using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyExplosion;
    [SerializeField]
    private GameObject impactExplosion;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int xp;
    [SerializeField]
    private UIProgressBarController healthBar;
    [SerializeField]
    private Shield shield;

    private int _health;
    private LevelUpSystem _levelUpSystem;
    private AudioSource _audioSource;

    void Start()
    {
        _health = maxHealth;
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if(playerGO != null)
        {
            _levelUpSystem = playerGO.GetComponent<LevelUpSystem>();
        }
        else
        {
            Debug.LogError("Player is missing from the scene");
        }
        _audioSource = GetComponent<AudioSource>();

    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    public int Health
    {
        get { return _health; }
    }

    public void Damage(DamageInfo damageInfo)
    {
        if (CompareTag("Player"))
        {
            GameManager.instance.CameraController.ShakeCamera(0.3f, 10f, 0.3f);
        }

        if (shield != null && shield.Charge > 0f)
        {
            return;
        }

        _health -= damageInfo.Damage;

        if(healthBar != null)
        {
            healthBar.fillAmount = (float)_health / (float)maxHealth;
        }

        if (_health <= 0)
        {
            GameObject explosion = Instantiate(destroyExplosion, transform.position, transform.rotation) as GameObject;

            if (_levelUpSystem != null && damageInfo.Sender.CompareTag("PlayerWeapon"))
            {
                _levelUpSystem.GainExperience(xp);
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
