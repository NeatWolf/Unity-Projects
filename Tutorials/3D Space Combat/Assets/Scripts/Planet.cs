﻿using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    [SerializeField]
    private Renderer atmosphere;

	void Start ()
    {
        atmosphere.material.SetFloat("_Brightness", 0f);
    }
	
	void Update ()
    {
        float distance = Vector3.Distance(GameManager.playerTransform.position, transform.position);
        float ratio = distance / transform.localScale.x;

        // Roughly where the bloom starts looking reasonable
        if(ratio < 7f)
        {
            float brightness = -(ratio - 7f) * (3f / 7f);
            atmosphere.material.SetFloat("_Brightness", brightness);
        }
    }
}
