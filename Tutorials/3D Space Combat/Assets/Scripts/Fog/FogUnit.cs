using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogUnit : MonoBehaviour {

    private List<FogUnitInfo> _adjacentUnits;

	void Start()
    {

    }

    private class FogUnitInfo
    {
        public FogUnit Fog { get; set; }
        public Vector3 Position { get; set; }
    }
}
