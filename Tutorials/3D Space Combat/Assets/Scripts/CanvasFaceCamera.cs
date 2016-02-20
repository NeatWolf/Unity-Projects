using UnityEngine;
using System.Collections;

public class CanvasFaceCamera : MonoBehaviour {

	void Start()
    {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
    }
	
	void Update()
    {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
	}
}
