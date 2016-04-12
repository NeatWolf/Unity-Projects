using UnityEngine;
using System.Collections;

public class PrimaryWeapon : MonoBehaviour {

    public float fireRate;
    public Transform[] shotSpawns;
    public GameObject shotPrefab;
    public AudioClip shootingClip;

    private float nextFire;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shootingClip;
        audioSource.playOnAwake = false;
    }
	
	void Update ()
    {
        if (GameManager.instance.isShootingEnabled && CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Mouse0) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                foreach (var sp in shotSpawns)
                {
                    Instantiate(shotPrefab, sp.position, sp.rotation);
                    audioSource.Play();
                }
            }
        }
	}
}
