using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    public Transform playerStartingTransform;
    public GameObject[] asteroidPrefabs;
    public Vector2 asteroidSizeRange;
    public Quest firstQuest;
    public Transform deimos;
    public int deimosAsteroidCount;
    public GameObject deimosTravelObjective;
    public Transform deimosSpawnPoint;
    public GameObject enemyShipPrefab;
    public AudioClip dialogue1;

    [HideInInspector]
    public bool isInCombat = false;
    [HideInInspector]
    public bool isShootingEnabled = true;
    [HideInInspector]
    public bool isCursorVisible = true;
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
        SpawnHazardsAroundSphere(deimos, 3600, 2600, deimosAsteroidCount);
        playerTransform.position = playerStartingTransform.position;
        playerTransform.rotation = playerStartingTransform.rotation;
        Player player = playerTransform.GetComponent<Player>();
        player.LockControls(true);
        player.Dock(playerStartingTransform);
        Invoke("InitializeTargetPracticeQuest", 10f);
        player.LockControls(false);
        DialogueManager.instance.BeginDialogue(dialogue1);
	}

    public void GameOver()
    {
        gameOverScreen.Display();
    }
	
    private void SpawnHazards()
    {
        //for (int i = 0; i < hazardCount; i++)
        //{
        //    GameObject hazard = hazards[Random.Range(0, hazards.Length)];
        //    Vector3 spawnPosition = new Vector3(Random.Range(-hazardSpawnPosition.x, hazardSpawnPosition.x), Random.Range(-hazardSpawnPosition.y, hazardSpawnPosition.y), Random.Range(-hazardSpawnPosition.z, hazardSpawnPosition.z));
        //    GameObject hazardGO = Instantiate(hazard, spawnPosition, Quaternion.identity) as GameObject;
        //    float hazardScale = Random.Range(hazardSizeRange.x, hazardSizeRange.y);
        //    hazardGO.transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
        //}
    }

    private void SpawnHazardsAroundSphere(Transform sphere, float outerRadius, float innerRadius, float number)
    {
        for(int i = 0; i < number; i++)
        {
            GameObject hazard = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
            Vector3 spawnPosition = Random.onUnitSphere * Random.Range(innerRadius, outerRadius) + sphere.position;
            GameObject hazardGO = Instantiate(hazard, spawnPosition, Quaternion.identity) as GameObject;
            float hazardScale = Random.Range(asteroidSizeRange.x, asteroidSizeRange.y);
            hazardGO.transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
        }
    }

    /// <summary>
    /// Initialize the 'Target Practice' quest and add it to the Quest Manager
    /// </summary>
    private void InitializeTargetPracticeQuest()
    {
        // OBJECTIVE 1 - TRAVEL TO DEIMOS
        ObjectiveTarget deimos = deimosTravelObjective.AddComponent<ObjectiveTarget>();
        Objective firstObjective = firstQuest.GetObjectiveAtIndex(0);
        firstObjective.AssignTarget(deimos);

        // OBJECTIVE 2 - DEFEAT RAIDERS
        SpawnEnemyTargetsAtObjective(firstQuest, 1, enemyShipPrefab, 5, deimosSpawnPoint.position);

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
