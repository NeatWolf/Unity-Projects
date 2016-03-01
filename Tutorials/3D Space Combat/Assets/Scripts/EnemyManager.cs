using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    public GameObject enemy;
    public float spawnTime = 10f;
    public int spawnNumber = 5;
    public Transform[] spawnPointTransforms;
    public Transform player;
    public SpawnPoint spawnPoint;

    private SpawnPoint[] spawnPoints;

    void Start()
    {
        spawnPoints = new SpawnPoint[spawnPointTransforms.Length];
        for(int i = 0; i < spawnPointTransforms.Length - 1; i++)
        {
            spawnPoints[i] = Instantiate(spawnPoint, spawnPointTransforms[i].position, spawnPointTransforms[i].rotation) as SpawnPoint;
        }
    }

	void Update ()
    {
        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            if(spawnPoint != null && !spawnPoint.used && Vector3.Distance(player.position, spawnPoint.transform.position) < 1000)
            {
                StartCoroutine(SpawnWave(spawnPoint.transform, spawnNumber, spawnTime));
                spawnPoint.used = true;
            }
        }
	}

    IEnumerator SpawnWave(Transform spawnPoint, int number, float period)
    {
        for(int i = 0; i < number; i++)
        {
            yield return new WaitForSeconds(period);
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
