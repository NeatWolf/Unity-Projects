using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour {

    [SerializeField]
    private int count;
    [SerializeField]
    private GameObject enemyShipPrefab;
    [SerializeField]
    private GameObject friendlyShipPrefab;
    [SerializeField]
    private Vector3 spawnPosition;
    [SerializeField]
    private float spawnRadius;
    [SerializeField]
    private Vector3 newSpawnPosition;

    private List<GameObject> _enemies;
    private List<GameObject> _friendlies;

    void Start ()
    {
        _enemies = SpawnPrefabs(enemyShipPrefab, count, spawnPosition, spawnRadius);
        _friendlies = SpawnPrefabs(friendlyShipPrefab, count, spawnPosition, spawnRadius);
    }
	
	void Update ()
    {
	    foreach(var enemy in _enemies)
        {
            if(enemy == null)
            {
                _enemies.Remove(enemy);
            }
        }

        foreach (var friendly in _friendlies)
        {
            if (friendly == null)
            {
                _friendlies.Remove(friendly);
            }
        }

        if(_enemies.Count < count)
        {
            while(_enemies.Count != count)
            {
                _enemies.Add(Instantiate(enemyShipPrefab, newSpawnPosition, Quaternion.identity) as GameObject);
            }
        }

        if (_friendlies.Count < count)
        {
            while (_friendlies.Count != count)
            {
                _friendlies.Add(Instantiate(friendlyShipPrefab, newSpawnPosition, Quaternion.identity) as GameObject);
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
