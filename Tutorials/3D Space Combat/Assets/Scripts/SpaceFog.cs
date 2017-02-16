using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceFog : MonoBehaviour
{
    [SerializeField]
    private GameObject cloudPrefab;
    [SerializeField]
    private int cloudCount;
    [SerializeField]
    private float clusterSize;

    private Transform _location;
    private List<GameObject> _clouds;
    private float _clusterSizeSqr;

    void Start()
    {
        _location = transform;
        _clusterSizeSqr = clusterSize * clusterSize;
    }

    void Update()
    {
        if (_clouds == null)
        {
            _clouds = new List<GameObject>();
            CreateClouds();
        }

        foreach (var cloud in _clouds)
        {
            if ((cloud.transform.position - _location.position).sqrMagnitude > _clusterSizeSqr)
            {
                cloud.transform.position = Random.insideUnitSphere * clusterSize + _location.position;
            }
        }
    }

    private void CreateClouds()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            var position = Random.insideUnitSphere * clusterSize + _location.position;
            _clouds.Add(Instantiate(cloudPrefab, position, Quaternion.identity) as GameObject);
        }
    }
}
