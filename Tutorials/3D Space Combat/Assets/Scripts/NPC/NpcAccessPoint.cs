using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.NPC;

[RequireComponent(typeof(Collider))]
public class NpcAccessPoint : MonoBehaviour {

	public enum AccessPointType
    {
        Planet,
        SpaceStation,
        Warp
    }

    public AccessPointType type;
    public event System.EventHandler<AccessPointAvailableEventArgs> AccessPointAvailable;

    private bool _isNpcSpawning = false;
    private Collider _collider;

    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_isNpcSpawning && other.CompareTag("NPC"))
        {
            var otherNpcType = other.GetComponent<NpcType>();
            if (otherNpcType != null)
            {
                otherNpcType.Dock(type);
            }
            else
            {
                Debug.LogError("NPC is missing NpcType component");
            }

            AccessPointAvailableEventArgs args = new AccessPointAvailableEventArgs();
            args.removedNpc = true;
            OnAccessPointAvailable(args);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            _isNpcSpawning = false;
            OnAccessPointAvailable(null);
        }
    }

    private void OnAccessPointAvailable(AccessPointAvailableEventArgs args)
    {
        System.EventHandler<AccessPointAvailableEventArgs> handler = AccessPointAvailable;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    public void SpawnNpc(GameObject randomPrefab, List<NpcAccessPoint> compatibleDestinations)
    {
        if (randomPrefab == null)
        {
            throw new System.ArgumentNullException("randomPrefab");
        }
        if (compatibleDestinations == null || compatibleDestinations.Count <= 0)
        {
            throw new System.ArgumentNullException("compatibleDestinations");
        }

        _isNpcSpawning = true;
        GameObject npc = Instantiate(randomPrefab, transform.position, transform.rotation) as GameObject;
        NpcType npcType = npc.GetComponent<NpcType>();
        if (npcType != null)
        {
            npcType.Spawn(this);
        }
        else
        {
            Debug.LogError("An NPC is missing its NpcType component");
        }

        AINavigator npcNav = npc.GetComponent<AINavigator>();
        if (npcNav != null)
        {
            Transform destination = compatibleDestinations[Random.Range(0, compatibleDestinations.Count - 1)].transform;
            npcNav.Start(destination.position);
        }
        else
        {
            Debug.LogError("An NPC is missing its AINavigator component");
        }
    }
}
