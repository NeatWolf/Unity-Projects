using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int hazardCount;
    public GameObject[] asteroidPrefabs;
    public GameObject enemyShipPrefab;
    public Vector3 hazardSpawnPosition;
    public Vector2 hazardSizeRange;
    public Transform planet;
    public Quest firstQuest;
    public GameObject deimosTravelObjective;

    [HideInInspector]
    public bool isInCombat = false;
    [HideInInspector]
    public bool isPaused = false;

    public static GameManager instance;
    public static Transform playerTransform;
    public static QuestManager questManager;

    public delegate void NewMissionEventHandler(Mission mission);
    public static event NewMissionEventHandler NewMissionAcquired;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        questManager = gameObject.GetComponent<QuestManager>();
    }

	void Start ()
    {
        SpawnHazards();
        Invoke("CreateAndAddTestMission", 2);
	}
	
    void SpawnHazards()
    {
        //for (int i = 0; i < hazardCount; i++)
        //{
        //    GameObject hazard = hazards[Random.Range(0, hazards.Length)];
        //    Vector3 spawnPosition = new Vector3(Random.Range(-hazardSpawnPosition.x, hazardSpawnPosition.x), Random.Range(-hazardSpawnPosition.y, hazardSpawnPosition.y), Random.Range(-hazardSpawnPosition.z, hazardSpawnPosition.z));
        //    GameObject hazardGO = Instantiate(hazard, spawnPosition, Quaternion.identity) as GameObject;
        //    float hazardScale = Random.Range(hazardSizeRange.x, hazardSizeRange.y);
        //    hazardGO.transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
        //}
        SpawnHazardsAroundSphere(planet, 3600, 2600, hazardCount);
    }

    private void SpawnHazardsAroundSphere(Transform sphere, float outerRadius, float innerRadius, float number)
    {
        for(int i = 0; i < number; i++)
        {
            GameObject hazard = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
            Vector3 spawnPosition = Random.onUnitSphere * Random.Range(innerRadius, outerRadius) + sphere.position;
            GameObject hazardGO = Instantiate(hazard, spawnPosition, Quaternion.identity) as GameObject;
            float hazardScale = Random.Range(hazardSizeRange.x, hazardSizeRange.y);
            hazardGO.transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
        }
    }

    private void CreateAndAddTestMission()
    {
        // OBJECTIVE 1 - DEFEAT ENEMIES OVER MARS
        // Create enemy ships for objective 1
        List<ObjectiveTarget> enemiesObj1 = new List<ObjectiveTarget>();
        for(int i = 0; i < 2; i++)
        {
            GameObject enemyShip = Instantiate(enemyShipPrefab, new Vector3((i * 10f) - 50f, 0f, 100f), Quaternion.Euler(0f, 180f, 0f)) as GameObject;
            ObjectiveTarget enemyTarget = enemyShip.AddComponent<ObjectiveTarget>();
            enemiesObj1.Add(enemyTarget);
        }

        Objective firstObjective = firstQuest.GetObjectiveAtIndex(0);
        firstObjective.AssignTargets(enemiesObj1.ToArray());

        // OBJECTIVE 2 - TRAVEL TO DEIMOS
        ObjectiveTarget deimos = deimosTravelObjective.AddComponent<ObjectiveTarget>();
        
        Objective secondObjective = firstQuest.GetObjectiveAtIndex(1);
        secondObjective.AssignTarget(deimos);

        //questManager.AddQuest(firstQuest);
    }
}
