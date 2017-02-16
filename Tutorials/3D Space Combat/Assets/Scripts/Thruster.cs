using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour {

    [SerializeField]
    private ParticleSystem thruster;
	
	public void AdjustPower(float size)
    {
        var mainPs = thruster.main;
        mainPs.startLifetime = size;
    }
}
