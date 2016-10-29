using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NpcSpawner : MonoBehaviour {

    [Serializable]
    public struct SpawnableWithRate
    {
        public GameObject prefab;
        public float spawnRate;
    }

    public SpawnableWithRate[] spawnablesWithRates;

	void Start ()
    {
        // Make sure the rates all add up to 100
        float totalRate = 0;
        foreach(var rate in spawnablesWithRates)
        {
            totalRate += rate.spawnRate;
        }
        if (totalRate != 100)
        {
            Debug.LogError(string.Format("The spawn rates given did not add up to 100"));
        }
	}
	
	void Update ()
    {
	
	}
}
