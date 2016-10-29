using UnityEngine;
using System.Collections;

public class AIWeaponController : MonoBehaviour
{
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public AudioClip shootingClip;
    public float minShotVolume = 0.5f;
    public float maxShotVolume = 1f;

    private float nextFire;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        if(Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            float volume = Random.Range(minShotVolume, maxShotVolume);
            if (audioSource != null)
            {
                audioSource.PlayOneShot(shootingClip, volume);
            }
            else
            {
                Debug.LogWarning("No AudioSource attached to gameObject, so no audio will play from here.");
            }
        }
    }
}
