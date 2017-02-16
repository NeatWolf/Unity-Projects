using UnityEngine;
using System.Collections;

public class PrimaryWeapon : MonoBehaviour {

    [SerializeField]
    private float fireRate;
    [SerializeField]
    private GameObject shotPrefab;
    [SerializeField]
    private AudioClip shootingClip;
    [SerializeField]
    private float minShotVolume = 0.5f;
    [SerializeField]
    private float maxShotVolume = 1f;

    private float _nextFire;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
	
	void Update ()
    {
        if (GameManager.instance.IsShootingEnabled && CompareTag("PlayerWeapon") && !GameManager.instance.IsMenuOpen)
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
