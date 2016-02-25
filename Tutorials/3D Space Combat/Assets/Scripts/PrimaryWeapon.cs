using UnityEngine;
using System.Collections;

public class PrimaryWeapon : MonoBehaviour {

    public float fireRate;
    public Transform shotSpawn;
    public GameObject shot;

    private float nextFire;
	
	void Update ()
    {
	    if(Input.GetKey(KeyCode.Mouse0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
	}
}
