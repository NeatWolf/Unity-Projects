using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcType : MonoBehaviour {

    public NpcTypeEnum type;
    public List<NpcAccessPoint.AccessPointType> compatibleAccessPoints;

    private AIWarpDrive _warpDrive;

    public enum NpcTypeEnum
    {
        MediumTransportShip,
        Carrier
    }

    void Awake()
    {
        _warpDrive = GetComponent<AIWarpDrive>();
    }

    public bool IsAccessPointCompatible(NpcAccessPoint.AccessPointType apType)
    {
        foreach(var compatibleAP in compatibleAccessPoints)
        {
            if (apType.Equals(compatibleAP))
            {
                return true;
            }
        }
        return false;
    }

    public void Dock(NpcAccessPoint.AccessPointType dockingType)
    {
        Debug.Log("Ship is ready to dock");
        switch (dockingType)
        {
            case NpcAccessPoint.AccessPointType.Planet:
                Destroy(gameObject);
                break;
            case NpcAccessPoint.AccessPointType.SpaceStation:
                Destroy(gameObject);
                break;
            case NpcAccessPoint.AccessPointType.Warp:
                _warpDrive.ExitToWarp();
                break;
        }
    }

    public void Spawn(NpcAccessPoint spawnPoint)
    {
        switch (spawnPoint.type)
        {
            case NpcAccessPoint.AccessPointType.Warp:
                if (_warpDrive == null)
                {
                    _warpDrive = GetComponent<AIWarpDrive>();
                }
                _warpDrive.EnterFromWarp(transform.position, transform.rotation);
                break;
            default:
                break;
        }
    }
}
