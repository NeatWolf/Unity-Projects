using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.NPC;

public class NpcSpawnManager : MonoBehaviour {

    public List<GameObject> npcPrefabs;
    public int maxNpcCount;

    private int _currentNpcCount = 0;
    private List<CompleteNpc> _npcs = new List<CompleteNpc>();
    private List<NpcAccessPoint> _accessPoints;
    private List<NpcAccessPoint> _availableAccessPoints;

    void Awake()
    {
        foreach (var npc in npcPrefabs)
        {
            _npcs.Add(new CompleteNpc(npc, npc.GetComponent<NpcType>()));
        }
    }

    void Start()
    {
        _accessPoints = new List<NpcAccessPoint>(FindObjectsOfType<NpcAccessPoint>());
        _availableAccessPoints = new List<NpcAccessPoint>(_accessPoints);

        for (int i = 0; i < _accessPoints.Count; i++)
        {
            _accessPoints[i].AccessPointAvailable += AccessPoint_AccessPointAvailable;
            SpawnNpc(_accessPoints[i]);
        }
    }

    private void AccessPoint_AccessPointAvailable(object sender, AccessPointAvailableEventArgs e)
    {
        if (e != null && e.removedNpc)
        {
            _currentNpcCount--;
        }

        NpcAccessPoint accessPoint = sender as NpcAccessPoint;
        if (accessPoint != null)
        {
            if (!_availableAccessPoints.Contains(accessPoint))
            {
                _availableAccessPoints.Add(accessPoint);
            }
            if (_currentNpcCount < maxNpcCount)
            {
                SpawnNpc(_availableAccessPoints[Random.Range(0, _availableAccessPoints.Count - 1)]);
            }
        }
    }

    private List<NpcAccessPoint> GetAccessPointsForNpcType(NpcType type, NpcAccessPoint excluded)
    {
        List<NpcAccessPoint> compatibleAPs = new List<NpcAccessPoint>();

        foreach(var ap in _accessPoints)
        {
            if (ap != excluded && type.IsAccessPointCompatible(ap.type))
            {
                compatibleAPs.Add(ap);
            }
        }

        return compatibleAPs;
    }

    private List<GameObject> GetPrefabsForAccessPointType(NpcAccessPoint.AccessPointType type)
    {
        List<GameObject> compatiblePrefabs = new List<GameObject>();

        foreach(var npc in _npcs)
        {
            if (npc.type.IsAccessPointCompatible(type))
            {
                compatiblePrefabs.Add(npc.prefab);
            }
        }

        return compatiblePrefabs;
    }

    private void SpawnNpc(NpcAccessPoint accessPoint)
    {
        List<GameObject> compatiblePrefabs = GetPrefabsForAccessPointType(accessPoint.type);
        
        GameObject randomPrefab = compatiblePrefabs[Random.Range(0, compatiblePrefabs.Count - 1)];

        NpcType randomNpc = randomPrefab.GetComponent<NpcType>();
        if (randomNpc != null)
        {
            List<NpcAccessPoint> compatibleDestinations = GetAccessPointsForNpcType(randomNpc, accessPoint);

            if (randomPrefab != null && compatibleDestinations != null && compatibleDestinations.Count > 0)
            {
                accessPoint.SpawnNpc(randomPrefab, compatibleDestinations);
                _availableAccessPoints.Remove(accessPoint);
                _currentNpcCount++;
            }
            else
            {
                Debug.LogError("Failed to spawn an NPC");
            }
        }
        else
        {
            Debug.LogError("An NPC is missing its NpcType component");
        }
    }

    private class CompleteNpc
    {
        public CompleteNpc(GameObject prefab, NpcType type)
        {
            this.prefab = prefab;
            this.type = type;
        }

        public GameObject prefab { get; set; }
        public NpcType type { get; set; }
    }
}
