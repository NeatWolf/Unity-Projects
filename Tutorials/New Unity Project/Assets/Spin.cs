using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

    public float rate = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, rate, 0);
	}
}
