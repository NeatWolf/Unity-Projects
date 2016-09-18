using UnityEngine;
using System.Collections;

public class PrimaryWeapon : MonoBehaviour {

    public float fireRate;
    public Transform[] shotSpawns;
    public GameObject shotPrefab;
    public AudioClip shootingClip;
    public AudioSource[] shotAudioSources;
    public float minShotVolume = 0.5f;
    public float maxShotVolume = 1f;

    private float nextFire;

    void Start()
    {

    }
	
	void Update ()
    {
        if (GameManager.instance.isShootingEnabled && CompareTag("Player") && !GameManager.instance.isMenuOpen)
        {
            if (Input.GetKey(KeyCode.Mouse0) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                foreach (var sp in shotSpawns)
                {
                    Instantiate(shotPrefab, sp.position, sp.rotation);
                    PlayAudioSources();
                }
            }
        }
	}

    private void PlayAudioSources()
    {
        foreach(var audioSource in shotAudioSources)
        {
            float volume = Random.Range(minShotVolume, maxShotVolume);
            audioSource.PlayOneShot(shootingClip, volume);
        }
    }
}
