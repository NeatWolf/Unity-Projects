using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private float spawnTime = 10f;
    [SerializeField]
    private int spawnNumber = 5;
    [SerializeField]
    private Transform[] spawnPointTransforms;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private SpawnPoint spawnPoint;

    private SpawnPoint[] _spawnPoints;

    void Start()
    {
        _spawnPoints = new SpawnPoint[spawnPointTransforms.Length];
        for(int i = 0; i < spawnPointTransforms.Length - 1; i++)
        {
            _spawnPoints[i] = Instantiate(spawnPoint, spawnPointTransforms[i].position, spawnPointTransforms[i].rotation) as SpawnPoint;
        }
    }

	void Update ()
    {
        foreach(SpawnPoint spawnPoint in _spawnPoints)
        {
            if(spawnPoint != null && !spawnPoint.Used && Vector3.Distance(player.position, spawnPoint.transform.position) < 1000)
            {
                StartCoroutine(SpawnWave(spawnPoint.transform, spawnNumber, spawnTime));
                spawnPoint.Used = true;
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
