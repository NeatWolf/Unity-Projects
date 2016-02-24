using UnityEngine;
using System.Collections;

public class NoWarpZone : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        print("Entered no warp zone");
        WarpDrive wd = other.gameObject.GetComponent<WarpDrive>();
        if(wd != null)
        {
            wd.IsWarpEnabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        print("Left no warp zone");
        WarpDrive wd = other.gameObject.GetComponent<WarpDrive>();
        if (wd != null)
        {
            wd.IsWarpEnabled = true;
        }
    }
}
