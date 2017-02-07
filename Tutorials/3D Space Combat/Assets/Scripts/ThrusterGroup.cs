using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterGroup : MonoBehaviour {

    public float maxSize = 1.0f;
    public float minSize = 0.2f;
    public List<Thruster> thrusters;

    void Start()
    {
        if (maxSize < minSize) throw new Exception("Max size is smaller than min size");
    }

    /// <summary>
    /// Set the power level of the thrusters in the group
    /// </summary>
    /// <param name="level">Power level between 0 and 1</param>
	public void SetPower(float level)
    {
        var power = (Mathf.Clamp01(level) * (maxSize - minSize)) + minSize;
        foreach(var thruster in thrusters)
        {
            thruster.AdjustPower(power);
        }
    }

    public void SetMaxPower()
    {
        SetPower(1f);
    }
}
