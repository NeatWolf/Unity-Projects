using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class GameManager : MonoBehaviour
{
    public bool isTesting = false;
    public GameOverScreen gameOverScreen;
    public GameOverScreen winScreen;
    public CameraController cameraController;
    public Transform playerStartingTransform;
    public GameObject[] asteroidPrefabs;
    public Vector2 asteroidSizeRange;
    public Quest firstQuest;
    public Transform deimosTransform;
    public int deimosAsteroidCount;
    public GameObject deimosTravelObjective;
    public Transform deimosSpawnPoint;
    public GameObject earthTravelObjective;
    public Transform earthSpawnPoint;
    public GameObject enemyShipPrefab;
    public GameObject friendlyShipPrefab;
    public AudioClip dialogue1;
    public AudioClip ambienceClip;

    public float testSpawnRadius;
    public int testCount;

    [HideInInspector]
    public bool isInCombat = false;
    [HideInInspector]
    public bool isShootingEnabled = true;
    [HideInInspector]
    public bool isCursorVisible = true;
    [HideInInspector]
    public bool isMenuOpen = false;
    [HideInInspector]
    public PauseType pauseType = PauseType.none;

    public static GameManager instance;
    public static Transform playerTransform;
    public static QuestManager questManager;

    private Player player;
    private AudioSource audioAmbience;

    public enum PauseType
    {
        none,
        pauseMenu,
        questMenu,
        gameOver
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
        //DontDestroyOnLoad(gameObject);
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        questManager = gameObject.GetComponent<QuestManager>();
        audioAmbience = gameObject.AddComponent<AudioSource>();
        audioAmbience.clip = ambienceClip;
        audioAmbience.loop = true;
        audioAmbience.volume = 0.25f;
    }

	void Start ()
    {
        if (!isTesting)
        {
            // Spawn asteroids around Deimos
            //SpawnHazardsAroundSphere(deimosTransform, 9000, 5800, deimosAsteroidCount);

            // Position player at start transform
            playerTransform.position = playerStartingTransform.position;
            playerTransform.rotation = playerStartingTransform.rotation;

            // Lock player controls until after intro dialogue
            player = playerTransform.GetComponent<Player>();
            player.LockControls(true);
            StartCoroutine(player.LockControlsDelayed(false, 26.5f));
            //player.LockControls(false);

            //player.Dock(playerStartingTransform);

            audioAmbience.Play();

            // Add quest after intro dialogue
            Invoke("InitializeTargetPracticeQuest", 24f);
            DialogueManager.instance.BeginDialogue(dialogue1);

            //Invoke("KillPlayer", 10);
            //Invoke("DisplayWinScreen", 10);
        }
        else
        {
            StartTest();
        }
	}

    private void KillPlayer()
    {
        playerTransform.SendMessage("Damage", new DamageInfo(gameObject, 50));
    }

    public void GameOver()
    {
        gameOverScreen.Display();
    }

    public void DisplayWinScreen()
    {
        player.LockControls(true);
        player.LockMovement(true);
        winScreen.Display();
    }

    public void CloseWinScreen()
    {
        player.LockControls(false);
        player.LockMovement(false);
    }

    public void DockPlayer()
    {
        if (player != null)
        {
            player.Dock(playerStartingTransform);
            cameraController.PerformDock();
        }
    }

    public void UndockPlayer()
    {
        if (player != null)
        {
            player.Undock();
            cameraController.PerformUndock();
        }
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
            Debug.Log("Spawned hazard");
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
        SpawnPrefabsForObjective(firstQuest, 1, enemyShipPrefab, 5, deimosSpawnPoint.position, 50);

        // OBJECTIVE 3 - TRAVEL TO EARTH
        ObjectiveTarget earth = earthTravelObjective.AddComponent<ObjectiveTarget>();
        Objective thirdObjective = firstQuest.GetObjectiveAtIndex(2);
        thirdObjective.AssignTarget(earth);
        // Setup spawn of enemies and friendlies so they only spawn when player reaches this objective
        thirdObjective.OnStarted += ThirdObjective_OnStarted;
        //TODO: unsubscribe to this!!!

        // OBJECTIVE 4 - AID IN THE FIGHT
        // AI only start battling when player approaches the area

        questManager.Add(firstQuest);
        questManager.SetActiveQuest(firstQuest);
    }

    private void ThirdObjective_OnStarted(Objective sender)
    {
        SpawnPrefabsForObjective(firstQuest, 3, enemyShipPrefab, 20, earthSpawnPoint.position, 100f);
        SpawnPrefabs(friendlyShipPrefab, 20, earthSpawnPoint.position, 100f);
    }

    private void SpawnPrefabsForObjective(Quest quest, int ObjectiveIndex, GameObject shipPrefab, int spawnCount, Vector3 spawnCenter, float spawnRadius)
    {
        List<ObjectiveTarget> objectiveTargets = new List<ObjectiveTarget>();

        List<GameObject> spawned = SpawnPrefabs(shipPrefab, spawnCount, spawnCenter, spawnRadius);

        foreach(var unit in spawned)
        {
            ObjectiveTarget target = unit.AddComponent<ObjectiveTarget>();
            objectiveTargets.Add(target);
        }

        Objective objective = quest.GetObjectiveAtIndex(ObjectiveIndex);
        objective.AssignTargets(objectiveTargets.ToArray());
    }

    private List<GameObject> SpawnPrefabs(GameObject prefab, int count, Vector3 center, float radius)
    {
        List<GameObject> spawned = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(center.x - radius, center.x + radius), Random.Range(center.y - radius, center.y + radius), Random.Range(center.z - radius, center.z + radius));
            spawned.Add(Instantiate(prefab, spawnPosition, Quaternion.identity) as GameObject);
        }
        return spawned;
    }

    private void StartTest()
    {
        //SpawnHazardsAroundSphere(deimosTransform, 9000, 5800, deimosAsteroidCount);
        //SpawnPrefabs(enemyShipPrefab, testCount, Vector3.zero, testSpawnRadius);
        //SpawnPrefabs(friendlyShipPrefab, testCount, Vector3.zero, testSpawnRadius);
    }
}
