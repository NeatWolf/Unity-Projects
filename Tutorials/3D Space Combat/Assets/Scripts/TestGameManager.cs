using UnityEngine;
using System.Collections;

public class TestGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipPrefab;
    [SerializeField]
    private GameObject friendlyShipPrefab;
    [SerializeField]
    private float spawnRadius = 500f;
    [SerializeField]
    private int spawnCount = 20;

    [HideInInspector]
    public bool isInCombat = false;
    [HideInInspector]
    public bool isShootingEnabled = true;

    public static TestGameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        SpawnEnemies(spawnCount);
        SpawnFriendlies(spawnCount);
	}

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            Instantiate(enemyShipPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void SpawnFriendlies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            Instantiate(friendlyShipPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
