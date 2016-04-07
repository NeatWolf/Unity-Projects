using UnityEngine;
using System.Collections;

public class DockPoint : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entering dock collider");
            GameManager.instance.DockPlayer();
        }
    }
}
