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

    private Collider _collider;
    private AINavigator _dockingNpcNav;
    private NpcType _spawningNpc;

    void Start()
    {
        _collider = GetComponent<Collider>();
        NpcSpawnManager.instance.AddAccessPoint(this);
    }

    void OnDestroy()
    {
        NpcSpawnManager.instance.RemoveAccessPoint(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<NpcType>() != _spawningNpc)
        {
            _dockingNpcNav = other.gameObject.GetComponentInParent<AINavigator>();
            if (_dockingNpcNav != null)
            {
                _dockingNpcNav.Dock(type);
            }
        }

        //if (_dockingNpcNav != null && _dockingNpcNav.Destination == this.gameObject)
        //{
        //    _dockingNpcNav.DestinationReached += OtherNav_DestinationReached;
        //}
    }

    //private void OtherNav_DestinationReached(object sender, System.EventArgs e)
    //{
    //    AINavigator senderNav = sender as AINavigator;
    //    if (senderNav == _dockingNpcNav)
    //    {
    //        _dockingNpcNav.DestinationReached -= OtherNav_DestinationReached;
    //    }

    //    if (senderNav != null)
    //    {
    //        NpcType senderType = senderNav.GetComponent<NpcType>();
    //        if (senderType != null)
    //        {
    //            senderType.Dock(type);
    //        }
    //    }
    //}

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            var args = new AccessPointAvailableEventArgs();
            args.removedNpc = other.GetComponent<NpcType>();
            OnAccessPointAvailable(args);
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

        Debug.Log(string.Format("Spawned an NPC at: {0}", Time.time));
        GameObject npc = Instantiate(randomPrefab, transform.position, transform.rotation) as GameObject;
        _spawningNpc = npc.GetComponent<NpcType>();
        if (_spawningNpc != null)
        {
            _spawningNpc.Spawn(this);
        }
        else
        {
            Debug.LogError("An NPC is missing its NpcType component");
        }

        AINavigator npcNav = npc.GetComponent<AINavigator>();
        if (npcNav != null)
        {
            GameObject destination = compatibleDestinations[Random.Range(0, compatibleDestinations.Count - 1)].gameObject;
            npcNav.Start(destination);
        }
        else
        {
            Debug.LogError("An NPC is missing its AINavigator component");
        }
    }
}
