using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    public GameObject enemy;
    public float spawnTime = 10f;
    public Transform[] spawnPoints;
    public Transform player;

	void Start ()
    {
        foreach(Transform spawnPoint in spawnPoints)
        {
            if(Vector3.Distance(player.position, spawnPoint.position) < 1000)
            {
                StartCoroutine(SpawnWave(spawnPoint, 5, 5));
            }
        }
	}

    IEnumerator SpawnWave(Transform spawnPoint, int number, float period)
    {
        for(int i = 0; i < number; i++)
        {
            yield return new WaitForSeconds(period);
            Spawn(spawnPoint);
        }
    }

    private void Spawn(Transform spawnPoint)
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length - 1);
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
