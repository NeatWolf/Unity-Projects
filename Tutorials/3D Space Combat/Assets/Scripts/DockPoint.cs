using UnityEngine;
using System.Collections;

public class DockPoint : MonoBehaviour {

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entering dock collider");
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.Dock(transform);
                //CameraController camController = Camera.main.GetComponent<CameraController>();
                //if(camController != null)
                //{
                //    camController.PerformDock();
                //}
            }
        }
    }
}
