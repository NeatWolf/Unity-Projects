using UnityEngine;
using System.Collections;

public class NPCSpawnPoint : MonoBehaviour {

    [SerializeField]
    private Animation enterAnim;
    [SerializeField]
    private Animation exitAnim;
    [SerializeField]
    private Vector2 waitBetweenSpawns;
    [SerializeField]
    private GameObject[] shipPrefabs;

    private void SpawnNPC(NPCSpawnPoint destination)
    {
        GameObject ship = Instantiate(shipPrefabs[Random.Range(0, shipPrefabs.Length - 1)], transform.position, transform.rotation) as GameObject;
        AINavigator shipNav = ship.GetComponent<AINavigator>();
    }
}
