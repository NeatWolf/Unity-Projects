using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour {

    public int count;
    public GameObject enemyShipPrefab;
    public GameObject friendlyShipPrefab;
    public Vector3 spawnPosition;
    public float spawnRadius;
    public Vector3 newSpawnPosition;

    private List<GameObject> enemies;
    private List<GameObject> friendlies;

    void Start ()
    {
        enemies = SpawnPrefabs(enemyShipPrefab, count, spawnPosition, spawnRadius);
        friendlies = SpawnPrefabs(friendlyShipPrefab, count, spawnPosition, spawnRadius);
    }
	
	void Update ()
    {
	    foreach(var enemy in enemies)
        {
            if(enemy == null)
            {
                enemies.Remove(enemy);
            }
        }

        foreach (var friendly in friendlies)
        {
            if (friendly == null)
            {
                friendlies.Remove(friendly);
            }
        }

        if(enemies.Count < count)
        {
            while(enemies.Count != count)
            {
                enemies.Add(Instantiate(enemyShipPrefab, newSpawnPosition, Quaternion.identity) as GameObject);
            }
        }

        if (friendlies.Count < count)
        {
            while (friendlies.Count != count)
            {
                friendlies.Add(Instantiate(friendlyShipPrefab, newSpawnPosition, Quaternion.identity) as GameObject);
            }
        }
    }

    private List<GameObject> SpawnPrefabs(GameObject prefab, int count, Vector3 center, float radius)
    {
        List<GameObject> spawned = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(center.x - radius, center.x + radius), Random.Range(center.y - radius, center.y + radius), Random.Range(center.z - radius, center.z + radius));
            spawned.Add(Instantiate(prefab, spawnPosition, Quaternion.identity) as GameObject);
        }
        return spawned;
    }
}
