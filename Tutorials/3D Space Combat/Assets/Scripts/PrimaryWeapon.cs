using UnityEngine;
using System.Collections;

public class PrimaryWeapon : MonoBehaviour {

    public float fireRate;
    public Transform[] shotSpawns;
    public GameObject shotPrefab;

    private float nextFire;
	
	void Update ()
    {
	    if(Input.GetKey(KeyCode.Mouse0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            foreach(var sp in shotSpawns)
            {
                Instantiate(shotPrefab, sp.position, sp.rotation);
            }
        }
	}
}
