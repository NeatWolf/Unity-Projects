using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceFog : MonoBehaviour
{
    public GameObject cloudPrefab;
    public int cloudCount;
    public float clusterSize;

    private Transform location;
    private List<GameObject> clouds;
    private float clusterSizeSqr;

    void Start()
    {
        location = transform;
        clusterSizeSqr = clusterSize * clusterSize;
    }

    void Update()
    {
        if (clouds == null)
        {
            clouds = new List<GameObject>();
            CreateClouds();
        }

        foreach (var cloud in clouds)
        {
            if ((cloud.transform.position - location.position).sqrMagnitude > clusterSizeSqr)
            {
                cloud.transform.position = Random.insideUnitSphere * clusterSize + location.position;
            }
        }
    }

    private void CreateClouds()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            var position = Random.insideUnitSphere * clusterSize + location.position;
            clouds.Add(Instantiate(cloudPrefab, position, Quaternion.identity) as GameObject);
        }
    }
}
