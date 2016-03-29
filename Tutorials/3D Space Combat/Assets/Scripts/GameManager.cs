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
    public Transform deimosSpawnPoint;

    [HideInInspector]
    public bool isInCombat = false;
    [HideInInspector]
    public bool isPaused = false;
    [HideInInspector]
    public PauseType pauseType = PauseType.none;

    public static GameManager instance;
    public static Transform playerTransform;
    public static QuestManager questManager;

    public enum PauseType
    {
        none,
        pauseMenu,
        questMenu
    }

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
        SpawnEnemyTargetsAtObjective(firstQuest, 0, enemyShipPrefab, 1, new Vector3(-50f, 0f, 100f));

        // OBJECTIVE 2 - TRAVEL TO DEIMOS
        ObjectiveTarget deimos = deimosTravelObjective.AddComponent<ObjectiveTarget>();
        Objective secondObjective = firstQuest.GetObjectiveAtIndex(1);
        secondObjective.AssignTarget(deimos);

        // OBJECTIVE 3 - DEFEAT ENEMIES NEAR DEIMOS
        //SpawnEnemyTargetsAtObjective(firstQuest, 2, enemyShipPrefab, 5, deimosSpawnPoint.position);

        questManager.Add(firstQuest);
        questManager.SetActiveQuest(firstQuest);
    }

    private void SpawnEnemyTargetsAtObjective(Quest quest, int ObjectiveIndex, GameObject shipPrefab, int count, Vector3 spawnPosition)
    {
        List<ObjectiveTarget> enemiesObj1 = new List<ObjectiveTarget>();
        for (int i = 0; i < count; i++)
        {
            GameObject enemyShip = Instantiate(shipPrefab, new Vector3((i * 10f) + spawnPosition.x, spawnPosition.y, spawnPosition.z), Quaternion.identity) as GameObject;
            ObjectiveTarget enemyTarget = enemyShip.AddComponent<ObjectiveTarget>();
            enemiesObj1.Add(enemyTarget);
        }

        Objective objective = quest.GetObjectiveAtIndex(ObjectiveIndex);
        objective.AssignTargets(enemiesObj1.ToArray());
    }
}
