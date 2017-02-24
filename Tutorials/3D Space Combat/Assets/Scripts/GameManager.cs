using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class GameManager : MonoBehaviour
{
    public bool isTesting = false;
    [SerializeField]
    private GameOverScreen gameOverScreen;
    [SerializeField]
    private GameOverScreen winScreen;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private Player player;
    [Header("Asteroids")]
    [SerializeField]
    private GameObject[] asteroidPrefabs;
    [SerializeField]
    private Vector2 asteroidSizeRange;
    [SerializeField]
    private Quest firstQuest;
    [Header("Deimos")]
    [SerializeField]
    private Transform deimosTransform;
    [SerializeField]
    private int deimosAsteroidCount;
    [SerializeField]
    private GameObject deimosTravelObjective;
    [SerializeField]
    private Transform deimosSpawnPoint;
    [Header("Earth")]
    [SerializeField]
    private GameObject earthTravelObjective;
    [SerializeField]
    private Transform earthSpawnPoint;
    [SerializeField]
    [Header("Ship Prefabs")]
    private GameObject enemyShipPrefab;
    [SerializeField]
    private GameObject friendlyShipPrefab;
    [Header("Dialogue")]
    [SerializeField]
    private AudioClip openingDialogue;
    [Header("Testing")]
    [SerializeField]
    private float testSpawnRadius;
    [SerializeField]
    private int testCount;

    public bool IsInCombat
    {
        get { return _isInCombat; }
        set { _isInCombat = value; }
    }
    
    public bool IsShootingEnabled
    {
        get { return _isShootingEnabled; }
        set { _isShootingEnabled = value; }
    }

    public bool IsCursorVisible
    {
        get { return _isCursorVisible; }
        set { _isCursorVisible = value; }
    }

    public bool IsMenuOpen
    {
        get { return _isMenuOpen; }
        set { _isMenuOpen = value; }
    }

    public PauseTypeEnum PauseType
    {
        get { return _pauseType; }
        set { _pauseType = value; }
    }

    public Player Player
    {
        get { return player; }
    }

    public CameraController CameraController
    {
        get { return cameraController; }
    }

    public static GameManager instance;
    public static Transform playerTransform;
    public static QuestManager questManager;

    private Transform _playerStartingTransform;
    private bool _isInCombat = false;
    private bool _isShootingEnabled = true;
    private bool _isCursorVisible = true;
    private bool _isMenuOpen = false;
    private PauseTypeEnum _pauseType = PauseTypeEnum.none;

    public enum PauseTypeEnum
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
    }

	void Start ()
    {
        if (!isTesting)
        {
            IsCursorVisible = false;

            // Lock player controls until after intro dialogue
            player = playerTransform.GetComponent<Player>();
            player.LockControls(true);
            player.LockMovement(true);
            StartCoroutine(player.LockControlsDelayed(false, 41f));
            StartCoroutine(player.LockMovementDelayed(false, 41f));
            Invoke("EnableReticle", 41f);

            // Add quest after intro dialogue
            Invoke("InitializeTargetPracticeQuest", 27f);
            DialogueManager.instance.BeginDialogue(openingDialogue);

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
        HealthController health = playerTransform.GetComponent<HealthController>();
        if (health != null)
        {
            health.Damage(new DamageInfo(gameObject, 50));
        }
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

    private void EnableReticle()
    {
        IsCursorVisible = true;
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
        Invoke("InitializeTargetPracticeQuest", 1f);
        Invoke("LevelUpTest", 10f);
        //SpawnHazardsAroundSphere(deimosTransform, 9000, 5800, deimosAsteroidCount);
        SpawnPrefabs(enemyShipPrefab, testCount, Vector3.zero, testSpawnRadius);
        SpawnPrefabs(friendlyShipPrefab, testCount, Vector3.zero, testSpawnRadius);
    }

    private void LevelUpTest()
    {
        player.GetComponent<LevelUpSystem>().GainExperience(50);
    }
}
