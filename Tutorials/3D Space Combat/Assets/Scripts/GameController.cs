using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public int hazardCount;
    public GameObject[] hazards;
    public Vector3 hazardSpawnPosition;

	void Start ()
    {
        SpawnHazards();
	}
	
    void SpawnHazards()
    {
        for (int i = 0; i < hazardCount; i++)
        {
            GameObject hazard = hazards[Random.Range(0, hazards.Length)];
            Vector3 spawnPosition = new Vector3(Random.Range(-hazardSpawnPosition.x, hazardSpawnPosition.x), Random.Range(-hazardSpawnPosition.y, hazardSpawnPosition.y), Random.Range(-hazardSpawnPosition.z, hazardSpawnPosition.z));
            Instantiate(hazard, spawnPosition, Quaternion.identity);
        }
    }
}
