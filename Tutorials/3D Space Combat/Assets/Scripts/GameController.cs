using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public int hazardCount;
    public GameObject[] hazards;
    public Vector3 hazardSpawnPosition;
    public Vector2 hazardSizeRange;

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
            GameObject hazardGO = Instantiate(hazard, spawnPosition, Quaternion.identity) as GameObject;
            float hazardScale = Random.Range(hazardSizeRange.x, hazardSizeRange.y);
            hazardGO.transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
        }
    }
}
