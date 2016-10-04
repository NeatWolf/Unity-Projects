using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class TeleportWeapon : MonoBehaviour {

    public Transform shotSpawn;
    public GameObject shotPrefab;
    public AudioClip shootingClip;
    public AudioSource[] shotAudioSources;
    public float minShotVolume = 0.5f;
    public float maxShotVolume = 1f;

    private GameObject currentShot;

    void Start()
    {

    }

    void Update()
    {
        if (GameManager.instance.isShootingEnabled && CompareTag("PlayerWeapon") && !GameManager.instance.isMenuOpen)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (currentShot != null)
                {
                    // Perform teleport
                    Teleport(currentShot.transform.position);
                }
                else
                {
                    // Fire shot
                    currentShot = Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation) as GameObject;
                    PlayAudioSources();
                }
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

    private void Teleport(Vector3 position)
    {
        GameManager.playerTransform.position = position;
        Destroy(currentShot);
    }
}
