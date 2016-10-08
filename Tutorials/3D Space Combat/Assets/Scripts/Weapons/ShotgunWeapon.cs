using UnityEngine;
using System.Collections;

public class ShotgunWeapon : MonoBehaviour {

    public CooldownBar cooldownBar;
    public float fireRate = 2f;
    public int boltsPerShot = 10;
    public float bulletSpread = 15f;
    public Transform shotSpawn;
    public GameObject shotPrefab;
    public AudioClip shootingClip;
    public AudioSource[] shotAudioSources;
    public float minShotVolume = 0.5f;
    public float maxShotVolume = 1f;

    void Start()
    {

    }

    void Update()
    {
        if (GameManager.instance.isShootingEnabled && CompareTag("PlayerWeapon") && !GameManager.instance.isMenuOpen)
        {
            if (Input.GetMouseButtonDown(1) && !cooldownBar.CoolingDown)
            {
                Fire();
                cooldownBar.Cooldown(fireRate);
            }
        }
    }

    private void PlayAudioSources()
    {
        foreach (var audioSource in shotAudioSources)
        {
            float volume = Random.Range(minShotVolume, maxShotVolume);
            audioSource.PlayOneShot(shootingClip, volume);
        }
    }

    private void Fire()
    {
        for (int i = 0; i < boltsPerShot; i++)
        {
            Vector3 rot = shotSpawn.rotation.eulerAngles;
            Vector3 rotation = new Vector3(rot.x + Random.Range(-bulletSpread, bulletSpread), rot.y + Random.Range(-bulletSpread, bulletSpread), rot.z + Random.Range(-bulletSpread, bulletSpread));
            Instantiate(shotPrefab, shotSpawn.position, Quaternion.Euler(rotation));
        }
    }
}
