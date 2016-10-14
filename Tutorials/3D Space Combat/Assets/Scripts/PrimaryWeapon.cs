using UnityEngine;
using System.Collections;

public class PrimaryWeapon : MonoBehaviour {

    public float fireRate;
    public GameObject shotPrefab;
    public AudioClip shootingClip;
    public float minShotVolume = 0.5f;
    public float maxShotVolume = 1f;

    private float _nextFire;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
	
	void Update ()
    {
        if (GameManager.instance.isShootingEnabled && CompareTag("PlayerWeapon") && !GameManager.instance.isMenuOpen)
        {
            if (Input.GetKey(KeyCode.Mouse0) && Time.time > _nextFire)
            {
                _nextFire = Time.time + fireRate;
                Instantiate(shotPrefab, transform.position, transform.rotation);
                PlayAudioSources();
            }
        }
	}

    private void PlayAudioSources()
    {
        float volume = Random.Range(minShotVolume, maxShotVolume);
        _audioSource.PlayOneShot(shootingClip, volume);
    }
}
