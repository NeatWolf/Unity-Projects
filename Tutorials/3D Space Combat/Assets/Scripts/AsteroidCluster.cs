using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidCluster : MonoBehaviour {

    [SerializeField]
    private GameObject[] asteroidPrefabs;
    [SerializeField]
    private int asteroidCount;
    [SerializeField]
    private float minSize, maxSize;
    [SerializeField]
    private AsteroidFieldType type;
    [SerializeField]
    private float innerRadius, outerRadius;
    [SerializeField]
    private float heightY;
    [SerializeField]
    private float widthX, lengthZ;
    [Header("Fog")]
    [SerializeField]
    private CloudsToy cloudsPrefab;
    [SerializeField]
    private int fogAmount;

    public enum AsteroidFieldType
    {
        Belt,
        Sphere,
        Box
    }

	void Start ()
    {
        List<Vector3> asteroidPositions = new List<Vector3>();
        for (int i = 0; i < asteroidCount; i++)
        {
            GameObject hazard = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

            Vector3 spawnPosition = Vector3.zero;
            switch (type)
            {
                case AsteroidFieldType.Sphere:
                    spawnPosition = Random.onUnitSphere.normalized * Random.Range(innerRadius, outerRadius) + transform.position;
                    break;
                case AsteroidFieldType.Box:
                    spawnPosition = new Vector3(transform.position.x + Random.Range(-widthX / 2, widthX / 2),
                        transform.position.y + Random.Range(-heightY / 2, heightY / 2),
                        transform.position.z + Random.Range(-lengthZ / 2, lengthZ / 2));
                    break;
                case AsteroidFieldType.Belt:
                    var pointOnRing2D = GetPointOnRing(innerRadius, outerRadius);
                    spawnPosition = new Vector3(pointOnRing2D.x, transform.position.y + Random.Range(-heightY / 2, heightY / 2), pointOnRing2D.y) + transform.position;
                    break;
            }
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

    public Vector2 GetPointOnRing(float aMinRadius, float aMaxRadius)
    {
        var v = Random.insideUnitCircle;
        return v.normalized * aMinRadius + v * (aMaxRadius - aMinRadius);
    }
}
