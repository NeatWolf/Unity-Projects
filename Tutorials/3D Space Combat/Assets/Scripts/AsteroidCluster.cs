using UnityEngine;
using System.Collections;

public class AsteroidCluster : MonoBehaviour {

    public GameObject[] asteroidPrefabs;
    public int asteroidCount;
    public float minSize, maxSize;
    public float innerRadius, outerRadius;
    public CloudsToy cloudsPrefab;
    public int fogAmount;

	void Start ()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            GameObject hazard = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
            Vector3 spawnPosition = Random.onUnitSphere * Random.Range(innerRadius, outerRadius) + transform.position;
            GameObject hazardGO = Instantiate(hazard, spawnPosition, Quaternion.identity) as GameObject;
            float hazardScale = Random.Range(minSize, maxSize);
            hazardGO.transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
        }

        for (int i = 0; i < fogAmount; i++)
        {
            Vector3 spawnPosition = Random.onUnitSphere * Random.Range(innerRadius, outerRadius) + transform.position;
            //CloudsToy instance = Instantiate(cloudsPrefab.gameObject, spawnPosition, Quaternion.identity) as CloudsToy;
        }
    }
}
