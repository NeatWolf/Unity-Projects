using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.NPC;

public class NpcSpawnManager : MonoBehaviour {

    [System.Serializable]
    public class CompleteNpc
    {
        public GameObject prefab;
        public int maxCount;

        public int Count { get; set; }

        public NpcType Type
        {
            get
            {
                return prefab.GetComponent<NpcType>();
            }
        }
    }

    public List<CompleteNpc> npcs;

    public static NpcSpawnManager instance;

    private List<NpcAccessPoint> _accessPoints = new List<NpcAccessPoint>();
    private List<NpcAccessPoint> _availableAccessPoints = new List<NpcAccessPoint>();

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

    void Start()
    {
        //_accessPoints = new List<NpcAccessPoint>(FindObjectsOfType<NpcAccessPoint>());
        //_availableAccessPoints = new List<NpcAccessPoint>(_accessPoints);

        //for (int i = 0; i < _accessPoints.Count; i++)
        //{
        //    _accessPoints[i].AccessPointAvailable += AccessPoint_AccessPointAvailable;
        //    SpawnNpc(_accessPoints[i]);
        //}
    }

    public void AddAccessPoint(NpcAccessPoint accessPoint)
    {
        accessPoint.AccessPointAvailable += AccessPoint_AccessPointAvailable;
        _accessPoints.Add(accessPoint);
        _availableAccessPoints.Add(accessPoint);
        AccessPoint_AccessPointAvailable(accessPoint, null);
    }

    public void RemoveAccessPoint(NpcAccessPoint accessPoint)
    {
        _accessPoints.Remove(accessPoint);
        _availableAccessPoints.Remove(accessPoint);
    }

    private void AccessPoint_AccessPointAvailable(object sender, AccessPointAvailableEventArgs e)
    {
        if (e != null && e.removedNpc != null)
        {
            foreach(var npc in npcs)
            {
                if (npc.Type == e.removedNpc)
                {
                    npc.Count--;
                    break;
                }
            }
        }

        NpcAccessPoint accessPoint = sender as NpcAccessPoint;
        if (accessPoint != null)
        {
            if (!_availableAccessPoints.Contains(accessPoint))
            {
                _availableAccessPoints.Add(accessPoint);
            }

            SpawnNpc(_availableAccessPoints[Random.Range(0, _availableAccessPoints.Count - 1)]);
        }
    }

    public List<NpcAccessPoint> GetDestinationsForNpcType(NpcType type, NpcAccessPoint excluded)
    {
        List<NpcAccessPoint> compatibleAPs = new List<NpcAccessPoint>();

        foreach(var ap in _accessPoints)
        {
            if (ap != null)
            {
                // Make sure we are not using destinations from the same ship/planet that the NPC was spawned
                if (ap.transform.parent != null && excluded != null && excluded.transform.parent != null)
                {
                    if (ap.transform.parent == excluded.transform.parent)
                    {
                        continue;
                    }
                }
                if (ap != excluded && type.IsAccessPointCompatible(ap.type))
                {
                    compatibleAPs.Add(ap);
                }
            }
        }

        return compatibleAPs;
    }

    private List<CompleteNpc> GetPrefabsForAccessPointType(NpcAccessPoint.AccessPointType type)
    {
        List<CompleteNpc> compatibleNPCs = new List<CompleteNpc>();

        foreach(var npc in npcs)
        {
            if (npc.Type.IsAccessPointCompatible(type))
            {
                if (npc.Count < npc.maxCount)
                {
                    compatibleNPCs.Add(npc);
                }
            }
        }

        return compatibleNPCs;
    }

    private void SpawnNpc(NpcAccessPoint accessPoint)
    {
        List<CompleteNpc> compatiblePrefabs = GetPrefabsForAccessPointType(accessPoint.type);

        if (compatiblePrefabs != null && compatiblePrefabs.Count > 0)
        {
            CompleteNpc randomNpc = compatiblePrefabs[Random.Range(0, compatiblePrefabs.Count - 1)];

            if (randomNpc != null)
            {
                List<NpcAccessPoint> compatibleDestinations = GetDestinationsForNpcType(randomNpc.Type, accessPoint);

                if (randomNpc != null && compatibleDestinations != null && compatibleDestinations.Count > 0)
                {
                    accessPoint.SpawnNpc(randomNpc.prefab, compatibleDestinations);
                    _availableAccessPoints.Remove(accessPoint);
                    randomNpc.Count++;
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
        else
        {
            Debug.Log("All NPCs types have been maxed out");
        }
    }
}
