using UnityEngine;
using System.Collections;

public class NPCSpawnPoint : MonoBehaviour {

    public Animation enterAnim;
    public Animation exitAnim;
    public Vector2 waitBetweenSpawns;
    public GameObject[] shipPrefabs;

	void Start ()
    {

	}
	
	void Update ()
    {
	
	}

    private void SpawnNPC(NPCSpawnPoint destination)
    {
        GameObject ship = Instantiate(shipPrefabs[Random.Range(0, shipPrefabs.Length - 1)], transform.position, transform.rotation) as GameObject;
        AINavigator shipNav = ship.GetComponent<AINavigator>();
    }
}
